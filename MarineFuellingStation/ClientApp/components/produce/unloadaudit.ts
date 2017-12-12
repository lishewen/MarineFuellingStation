import ComponentBase from "../../componentbase";
import axios from "axios";
import moment from "moment";
import { Component } from 'vue-property-decorator';

@Component
export default class UnloadAuditComponent extends ComponentBase {
    purchases: server.purchase[];
    purchase: server.purchase;
    actItems: ydui.actionSheetItem[];

    showAct: boolean = false;

    filterCType: Array<helper.filterBtn>;
    state: server.unloadState;

    actBtnId: number; //当前激活状态的条件button

    page: number;
    scrollRef: any;
    pSize: number = 30;

    constructor() {
        super();

        this.purchases = new Array<server.purchase>();
        this.filterCType = [
            { id: 0, name: '完工', value: server.unloadState.完工, actived: true },
            { id: 1, name: '已审核', value: server.unloadState.已审核, actived: false }
        ];

        this.actBtnId = 0;
        this.actItems = new Array();

        this.state = server.unloadState.完工;
        this.getPurchases();
    }

    switchBtn(o: helper.filterBtn, idx: number) {
        o.actived = true;
        if (idx != this.actBtnId) {
            this.filterCType[this.actBtnId].actived = false;
            this.actBtnId = idx;

            this.state = o.value as server.unloadState;
            this.page = 1;
            this.getPurchases();
        }
    }

    //显示actionsheet
    purchaseclick(s: server.purchase) {
        console.log(s);
        this.showAct = true;
        this.actItems = [
            {
                label: '详细信息',
                callback: () => {
                    this.godetail(s.id);
                }
            }
        ];
        if (this.state == server.unloadState.完工) {
            this.actItems.push(
                {
                    label: '重新施工',
                    callback: () => {
                        this.putRestart(s.id);
                    }
                }
            );
            this.actItems.push(
                {
                    label: '审核通过',
                    callback: () => {
                        this.putAuditingOK(s);
                    }
                }
            );
        }
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 卸油审核');
    };

    change(label: string, tabkey: string) {
        console.log(label);
    }
    godetail(id: number) {
        this.$router.push('/purchase/purchase/' + id + '/auditing')
    }

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

    strDiff(p: server.purchase) {
        let 实际转入升数: number = 0;
        let 订单升数: number = 0;
        p.toStoresList.forEach((st, i) => {
            实际转入升数 += st.count;
        });
        订单升数 = Math.round(p.count / p.density * 1000);
        return 订单升数 - 实际转入升数;
    }

    //获得计划列表
    getPurchases(callback?: Function) {
        if (this.page == null) this.page = 1;
        if (this.pSize == null) this.pSize = 30;
        if (this.state == null) this.state = server.unloadState.完工;
        axios.get('/api/Purchase/GetByState?'
            + 'page=' + this.page
            + '&pageSize=' + this.pSize
            + '&pus=' + this.state).then((res) => {
                let jobj = res.data as server.resultJSON<server.purchase[]>;
                if (jobj.code == 0) {
                    if (callback) {
                        callback(jobj.data);
                    }
                    else {
                        this.purchases = jobj.data;
                        console.log(this.purchases);
                        this.page++;
                    }
                }
            });
    }

    putAuditingOK(s: server.purchase) {
        axios.put('/api/Purchase/AuditingOK', s).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.purchase = jobj.data;
                this.toastSuccess('审核成功')
                this.page = 1;
                this.getPurchases();
            }
        });
    }
    putRestart(pid: number) {
        axios.put('/api/Purchase/UnloadRestart?pid=' + pid, null).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.toastSuccess('操作成功')
                this.page = 1;
                this.getPurchases();
            }
        });
    }
}