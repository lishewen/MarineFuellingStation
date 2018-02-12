import ComponentBase from "../../ComponentBase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";
import District from 'ydui-district/dist/gov_province_city_area_id';

@Component
export default class PurchaseComponent extends ComponentBase {
    model: server.purchase;
    purchases: server.purchase[];
    selectPurchase: server.purchase;
    oilshow: boolean = false;
    showMenus: boolean = false;
    showAddDelReason: boolean = false;
    oiloptions: ydui.actionSheetItem[];
    menus: ydui.actionSheetItem[];
    oilName: string = '';
    originshow: boolean = false;
    district: District = District;
    isPrevent: boolean = true;

    radio1: string = "2";
    show2: boolean = false;
    carNo: string = "";
    sv: string = "";
    page: number;
    scrollRef: any;
    pSize: number = 30;

    strShowAll: string;
    actBtnId: number;

    filterCType: Array<helper.filterBtn>; filterPType;

    isLeader: boolean = false;

    constructor() {
        super();

        this.oiloptions = (new Array()) as ydui.actionSheetItem[];
        this.menus = new Array() as ydui.actionSheetItem[];
        this.purchases = new Array<server.purchase>();
        this.selectPurchase = new Object as server.purchase;

        this.model = (new Object()) as server.purchase;
        this.model.name = '';
        this.model.price = null;
        this.model.count = null;
        this.model.origin = '';
        this.model.carNo = '';
        this.model.startTime = this.formatDate(new Date(), 'YYYY-MM-DD hh:mm');
        this.model.arrivalTime = this.formatDate(new Date(), 'YYYY-MM-DD hh:mm');

        this.filterCType = [
            { id: 0, name: '全部', value: '全部', actived: false },
            { id: 1, name: '未卸油入库', value: '未卸油入库', actived: true }
        ];
        this.actBtnId = 1;

        this.getPurchaseNo();
        this.getOilProducts();
    }

    mounted() {
        this.isLeader = this.$store.state.isLeader;

        this.$emit('setTitle', this.$store.state.username + ' 的进油计划');
        this.$watch('radio1', (v, ov) => {
            switch (v) {
                case "1":
                    this.show2 = false;
                    break;
                case "2":
                    this.show2 = true;
                    break;
            }
        });
        this.$watch('sv', (v: string, ov) => {
            //3个字符开始才执行请求操作，减少请求次数
            if (v.length >= 2 || v == "") {
                this.page = 1;
                this.getPurchases();
            }
        });
    };

    loadList() {
        this.getPurchases((list: server.purchase[]) => {
            this.purchases = this.page > 1 ? [...this.purchases, ...list] : this.purchases;
            this.scrollRef = (<any>this).$refs.infinitescroll;
            if (list.length < this.pSize) {
                this.scrollRef.$emit("ydui.infinitescroll.loadedDone");
                return;
            }

            //通知加载数据完毕
            this.scrollRef.$emit("ydui.infinitescroll.finishLoad");

            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
            console.log("page = " + this.page)
        });
    }

    origincallback(ret) {
        //console.log(ret);
        this.model.origin = ret.itemName1 + ' ' + ret.itemName2 + ' ' + ret.itemName3;
    }

    change(label: string, tabkey: string) {
        console.log(label);
        
        if (label == "单据记录") {
            this.page = 1;
            this.getPurchases();
        }
    }

    godetail(id: number) {
        this.$router.push('/purchase/purchase/' + id + '/purchase');
    }

    showMenuclick(p: server.purchase) {
        this.menus = [
            {
                label: '单据明细',
                callback: () => {
                    this.godetail(p.id)
                }
            }, {
                label: '作废单据',
                callback: () => {
                    this.selectPurchase = p;
                    if (this.selectPurchase.delReason == null) this.selectPurchase.delReason = "";
                    this.showAddDelReason = true;
                }
            }
        ];
        this.showMenus = true;
    }

    //删除单据
    delPurchaseclick() {
        if (this.selectPurchase.delReason == null || this.selectPurchase.delReason == "") { this.toastError("请填写作废原因"); return; }
        if (this.selectPurchase.state == server.unloadState.完工) {
            //如果为上级领导，则可以当状态为已完成时，还原油仓油量。否则不可以作废单据
            if (this.isLeader) {
                this.$dialog.confirm({
                    title: '提示',
                    mes: '当前单据施工状态为【已完成】，作废单据将恢复油量' + this.selectPurchase.oilCount + "升到油仓，是否确认？",
                    opts: () => {
                        this.deletePurchase();
                    }
                })
            }
            else 
                this.$dialog.alert({ mes: "只有上级领导才可以作废施工状态为【已完成】的单据!" });
        }
        else
            this.deletePurchase()
    }

    buttonclick() {
        //信息验证
        if (!this.model.productId || this.model.productId == 0) { this.toastError('必须选择油品'); return; }
        if (this.model.price == null) { this.toastError('单价不能为空，如不填写请填0'); return; }
        if (!this.model.count || this.model.count <= 0) { this.toastError('数量必须大于1'); return; }
        if (this.model.carNo == '') { this.toastError('车牌不能为空'); return; }
        this.postPurchase(this.model);
    }

    switchBtn(o: helper.filterBtn, idx: number, group: string) {
        if (group == "筛选") {
            if (idx != this.actBtnId) {
                o.actived = true;
                this.strShowAll = o.value.toString();
                this.filterCType[this.actBtnId].actived = false;
                this.actBtnId = idx;
            }
        }
        if (o.actived) {
            this.scrollRef.$emit("ydui.infinitescroll.reInit");
            this.page = 1;
            this.getPurchases();
        }
    }

    getPurchaseNo() {
        axios.get('/api/Purchase/PurchaseNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0) {
                this.model.name = jobj.data;
                this.isPrevent = false;
            }            
        });
    }

    getPurchases(callback?: Function) {
        if (!this.page) this.page = 1;
        let isShowAll = this.strShowAll == "全部" ? true : false;
        axios.get('/api/Purchase/GetByPager'
            + '?page='+ this.page
            + '&pagesize=' + this.pSize
            + '&sv=' + this.sv
            + '&isShowAll=' + isShowAll
            ).then((res) => {
                let jobj = res.data as server.resultJSON<server.purchase[]>;
                if (jobj.code == 0) {
                    if (callback) {
                        callback(jobj.data);
                    }
                    else {
                        this.purchases = jobj.data;
                        this.page++;
                    }
                }
            });
    }
    
    getOilProducts() {
        axios.get('/api/Product/PurchaseOilProducts').then((res) => {
            let jobj = res.data as server.resultJSON<server.product[]>;
            if (jobj.code == 0) {
                jobj.data.forEach((o, i) => {
                    this.oiloptions.push({
                        label: o.name,
                        callback: () => {
                            this.oilName = o.name;
                            this.model.productId = o.id;
                            this.model.price = o.lastPrice;
                            this.model.product = o;
                        }
                    });
                });
            }
        });
    }

    postPurchase(model: server.purchase) {
        this.isPrevent = true;
        axios.post('/api/Purchase', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.addNextConfirm();
            }
        });
    }

    //作废计划单据
    deletePurchase() {
        axios.delete('/api/Purchase?id=' + this.selectPurchase.id + "&delreason=" + this.selectPurchase.delReason).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.toastSuccess("作废成功！");
                this.showAddDelReason = false;
                this.page = 1;
                this.getPurchases();
            }
            else
                this.toastError(jobj.msg);
        })
    }
}