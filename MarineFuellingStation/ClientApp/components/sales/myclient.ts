import ComponentBase from "../../componentbase";
import axios from "axios";
import moment from "moment";
import { Component } from 'vue-property-decorator';

@Component
export default class MyClientComponent extends ComponentBase {
    clients: server.client[];
    companys: server.company[];
    modelCompany: server.company;
    client: server.client;
    radio2: string = "1";
    show1: boolean = false;
    show2: boolean = false;
    showAct: boolean = false;
    showRemark: boolean = false;
    showCompanys: boolean = false;
    showCompanyInput: boolean = false;
    remark: string = "";
    carNo: string = "";
    sv: string = "";
    svCompany: string = "";
    actItems: ydui.actionSheetItem[];

    filterCType: Array<helper.filterBtn>; filterPType: Array<helper.filterBtn>; filterBalances: Array<helper.filterBtn>; filterCycle: Array<helper.filterBtn>;

    lastBtnId: number; lastBtnId1: number; lastBtnId2: number; lastBtnId3: number;//最后激活状态的条件button
    ctype: server.clientType; ptype: server.salesPlanState; balances: number; cycle: number;//筛选条件用到的变量

    page: number;
    scrollRef: any;
    pSize: number = 30;

    constructor() {
        super();

        this.clients = new Array<server.client>();
        this.companys = new Array<server.company>();
        this.modelCompany = (new Object()) as server.company;

        this.filterCType = [
            { id: 0, name: '全部', value: server.clientType.全部, actived: true },
            { id: 1, name: '个人', value: server.clientType.个人, actived: false },
            { id: 2, name: '公司', value: server.clientType.公司, actived: false },
            { id: 3, name: '无销售员', value: server.clientType.无销售员, actived: false }
        ];
        this.filterPType = [
            { name: '已计划', value: server.salesPlanState.未审批, actived: false },
            { name: '已完成', value: server.salesPlanState.已完成, actived: false },
            { name: '已审批', value: server.salesPlanState.已审批, actived: false }
        ];
        this.filterBalances = [
            { name: '少于1000', value: 1000, actived: false },
            { name: '少于10000', value: 10000, actived: false }
        ]
        this.filterCycle = [
            { name: '7天不计划', value: 7, actived: false },
            { name: '15天不计划', value: 15, actived: false },
            { name: '30天不计划', value: 30, actived: false },
            { name: '90天不计划', value: 90, actived: false }
        ]

        this.actItems = new Array();

        this.lastBtnId = 0; this.lastBtnId1 = -1; this.lastBtnId2 = -1; this.lastBtnId3 = -1;
        this.page = 1;
        this.getClients();
    }

    loadList() {
        this.getClients((list: server.client[]) => {
            this.clients = this.page > 1 ? [...this.clients, ...list] : this.clients;
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

    switchBtn(currBtn: helper.filterBtn, currBtnIdx: number, group: string) {
        switch (group) {
            case "客户类型":
                if (currBtnIdx != this.lastBtnId) {
                    currBtn.actived = true;
                    this.ctype = <server.clientType>currBtn.value;
                    this.filterCType[this.lastBtnId].actived = false;
                    this.lastBtnId = currBtnIdx;
                }
                break;
            case "计划单":
                currBtn.actived = !currBtn.actived;
                this.ptype = <server.salesPlanState>currBtn.value;
                if (currBtnIdx != this.lastBtnId1 && this.lastBtnId1 != -1) {
                    this.filterPType[this.lastBtnId1].actived = false;
                    this.lastBtnId1 = currBtnIdx;
                }
                else
                    this.lastBtnId1 = currBtnIdx;
                break;
            case "账户余额":
                currBtn.actived = !currBtn.actived;
                this.balances = <number>currBtn.value;
                if (currBtnIdx != this.lastBtnId2 && this.lastBtnId2 != -1) {
                    this.filterBalances[this.lastBtnId2].actived = false;
                    this.lastBtnId2 = currBtnIdx;
                }
                else
                    this.lastBtnId2 = currBtnIdx;
                break;
            case "周期":
                currBtn.actived = !currBtn.actived;
                this.cycle = <number>currBtn.value;
                if (currBtnIdx != this.lastBtnId3 && this.lastBtnId3 != -1) {
                    this.filterCycle[this.lastBtnId3].actived = false;
                    this.lastBtnId3 = currBtnIdx;
                }
                else
                    this.lastBtnId3 = currBtnIdx;
                break;
        }
        if (currBtn.actived) {
            this.page = 1;
            this.getClients();
        }
    }

    filterclick(): void {
        this.show2 = false;
        this.page = 1;
        this.getClients();
    };

    //显示actionsheet
    clientclick(c: server.client) {
        console.log(c);
        let elseActItems = new Array();
        this.client = c;
        if (c.remark != null) this.remark = c.remark;
        this.showAct = true;
        //actionsheet
        this.actItems = new Array();
        if (c.isMark)
            this.actItems.push(
                {
                    label: '取消标记',
                    callback: () => {
                        this.putMark(c, false);
                    }
                },
            );
        else
            this.actItems.push(
                {
                    label: '标记',
                    callback: () => {
                        this.putMark(c, true);
                    }
                },
            );
        elseActItems = [
            {
                label: '详细信息',
                callback: () => {
                    this.godetail(c.id);
                }
            },
            {
                label: '备注',
                callback: () => {
                    this.showRemark = true;
                }
            },
            {
                label: '清空所有标记',
                callback: () => {
                    this.putClearAllMark();
                }
            }
        ];
        if (!c.followSalesman || c.followSalesman == "")
            elseActItems.unshift(
                {
                    label: '申请成为我的客户',
                    callback: () => {
                        this.getApplyBeMyClient(c.carNo, c.id, c.placeType);
                    }
                }
            );
        if (this.ctype == server.clientType.个人 || this.ctype == server.clientType.全部)
            elseActItems.unshift(
                {
                    label: '申请编入公司成员',
                    callback: () => {
                        this.showCompanys = true;
                        this.getCompanys();
                    }
                }
            )
        this.actItems = [...this.actItems, ...elseActItems];
    }

    classMark(isMark: boolean) {
        return isMark ? {color_blue: true} : {color_blue: false};
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的客户');
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.show1 = false;
                    break;
                case "2":
                    this.show1 = true;
                    break;
            }
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
    }
    godetail(id: number) {
        this.$router.push('/sales/myclient/' + id);
    }
    searchSubmit(value: string) {
        this.sv = value;
        this.page = 1;
        this.getClients();
    }
    searchCompanySubmit(value: string) {
        this.svCompany = value;
        this.getCompanys();
    }
    companyclick(coId: number, coName: string) {
        this.getApplyClientToCompany(this.client.id, coId, this.client.carNo, coName);
    }
    showAddCompanyclick() {
        this.modelCompany = new Object as server.company;
        this.showCompanyInput = true;
    }
    //提交新增公司
    addcompanyclick() {
        if (this.modelCompany.name == '' || this.modelCompany.name == null) {
            this.toastError("名称不能为空");
            return;
        }

        this.postCompany(this.modelCompany);
    }

    //获得我的客户列表
    getClients(callback?: Function) {
        if (this.ctype == null) this.ctype = server.clientType.全部;
        if (this.ptype == null) this.ptype = -1;//-1标识没有选择任何项
        if (this.balances == null) this.balances = -1;
        if (this.cycle == null) this.cycle = -1;
        if (this.page == null) this.page = 1;
        if (this.pSize == null) this.pSize = 30;
        if (this.sv == null) this.sv = "";
        
        axios.get('/api/Client/GetClients'
            + '?ctype=' + this.ctype.toString()
            + '&ptype=' + this.ptype.toString()
            + '&balances=' + this.balances.toString()
            + '&cycle=' + this.cycle.toString()
            + '&kw=' + this.sv
            + '&isMy=true'
            + '&page=' + this.page
            + '&pageSize=' + this.pSize
        ).then((res) => {
            let jobj = res.data as server.resultJSON<server.client[]>;
            if (jobj.code == 0) {
                if (callback) {
                    callback(jobj.data);
                }
                else {
                    this.clients = jobj.data;
                    console.log(this.clients);
                    this.page++;
                }
            }
            else
                this.toastError('无法获取客户数据，请重试')
        });
    }
    getCompanys() {
        if (this.svCompany == null) this.svCompany = "";
        axios.get('/api/Company/' + this.svCompany).then(res => {
            let jobj = res.data as server.resultJSON<server.company[]>;
            if (jobj.code == 0)
                this.companys = jobj.data;
            else
                this.toastError(jobj.msg);
        })
    }
    //申请成为我的客户
    getApplyBeMyClient(carNo: string, cid: number, placeType: server.placeType) {
        axios.get('/api/Client/ApplyBeMyClient'
            + '?carNo=' + carNo
            + "&id=" + cid
            + "&placetype=" + placeType
            ).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.toastSuccess('已向上级提出申请')
            }
        });
    }
    //申请将客户编入公司成员
    getApplyClientToCompany(cId: number, coId: number, carNo: string, companyName: string) {
        axios.get('/api/Client/ApplyClientToCompany'
            + '?carNo=' + carNo
            + "&cid=" + cId
            + "&coid=" + coId
            + "&companyName=" + companyName
        ).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.toastSuccess('已向上级提出申请');
                this.showCompanys = false;
            }
        });
    }
    //新增公司
    postCompany(model: server.company) {
        axios.post('/api/Company', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.company>;
            if (jobj.code == 0) {
                this.modelCompany.name = '';
                this.toastSuccess('操作成功');
                this.modelCompany = new Object as server.company;
            }
            else
                this.toastError(jobj.msg);
        });
    }

    putMark(c: server.client, isMark: boolean) {
        c.isMark = isMark;
        axios.put('/api/Client/MarkTag', c).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.client = jobj.data;
                this.toastSuccess('标记成功')
            }
        }); 
    }

    //备注提交
    putReMark(c: server.client) {
        c.remark = this.remark;
        axios.put('/api/Client/Remark', c).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.client = jobj.data;
                this.toastSuccess('操作成功')
            }
        });
    }

    putClearAllMark() {
        axios.put('/api/Client/ClearMyClientMark', null).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.toastSuccess('清空标记成功')
            }
        }); 
    }
}