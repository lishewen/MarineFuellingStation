import Vue from 'vue';
import moment from "moment";
import axios from "axios";

export default class ComponentBase extends Vue {
    /** 日期时间格式处理 2017-12-12 08:00 */
    formatDate(d: Date, f: string = 'YYYY-MM-DD HH:mm'): string {
        return moment(d).format(f);
    }
    /** 短日期格式 12-30 */
    formatShortDate(d: Date): string {
        return moment(d).format('MM-DD');
    }
    /** 距离现在相隔的时间 of: 'day'|'hour'|'minute'*/
    getDiffDate(d: Date, str: any) {
        moment.locale('zh-cn');
        return moment(d).startOf(str).fromNow();
    }
    /** 联系人 */
    toastError(msg: string) {
        this.$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }
    /** 联系人 */
    toastSuccess(msg: string) {
        this.$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'success'
        });
    }

    //========================== 打印单据 ==============================

    //打印“调拨单”
    getPrintOrder(id: number, to: string) {
        axios.get('/api/Order/PrintOrder?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.order>;
                if (jobj.code == 0) {
                    this.toastSuccess('调拨单打印指令已发出')
                }
            });
    }
    //打印“陆上送货单”
    getPrintDeliver(id: number, to: string) {
        axios.get('/api/Order/PrintDeliver?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.order>;
                if (jobj.code == 0) {
                    this.toastSuccess('陆上送货单打印指令已发出')
                }
            });
    }
    //打印“陆上装车单”
    getPrintLandload(id: number, to: string) {
        axios.get('/api/Order/PrintLandload?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.order>;
                if (jobj.code == 0) {
                    this.toastSuccess('陆上装车单打印指令已发出')
                }
            });
    }
    //打印“出库石化过磅单”
    getPrintPonderation(id: number, to: string) {
        axios.get('/api/Order/PrintPonderation?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.order>;
                if (jobj.code == 0) {
                    this.toastSuccess('出库石化过磅单打印指令已发出')
                }
            });
    }
    //打印预付款确认单 个人/公司
    postPrintPrepay(cl: server.chargeLog, to: string) {
        let actName = cl.isCompany ? "PrintCompanyPrepay" : "PrintClientPrepay";
        axios.post('/api/ChargeLog/' + actName + '?' +
            '&to=' + to, cl).then((res) => {
                let jobj = res.data as server.resultJSON<server.chargeLog>;
                if (jobj.code == 0) {
                    this.toastSuccess('预付款确认单打印指令已发出')
                }
            });
    }
    //打印“船舶清污完工证”
    getPrintBoatClean(id: number, to: string) {
        axios.get('/api/BoatClean/PrintBoatClean?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.boatClean>;
                if (jobj.code == 0) {
                    this.toastSuccess('完工证打印指令已发出')
                }
            });
    }
    //打印“船舶清污收款单”
    getPrintBcCollection(id: number, to: string) {
        axios.get('/api/BoatClean/PrintBcCollection?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.boatClean>;
                if (jobj.code == 0) {
                    this.toastSuccess('收款单打印指令已发出')
                }
            });
    }
    //打印“生产转仓单”
    getPrintMoveStore(id: number, to: string) {
        axios.get('/api/MoveStore/PrintMoveStore?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.moveStore>;
                if (jobj.code == 0) {
                    this.toastSuccess('生产转仓单打印指令已发出')
                }
            });
    }
    //打印“陆上卸油单”
    getPrintUnload(id: number, to: string) {
        axios.get('/api/Purchase/PrintUnload?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.purchase>;
                if (jobj.code == 0) {
                    this.toastSuccess('陆上卸油单打印指令已发出')
                }
            });
    }
    //打印“卸油过磅单”
    getPrintUnloadPond(id: number, to: string) {
        axios.get('/api/Purchase/PrintUnloadPond?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.purchase>;
                if (jobj.code == 0) {
                    this.toastSuccess('卸车石化过磅单打印指令已发出')
                }
            });
    }
    //打印“化验单”
    getPrintAssay(id: number, to: string) {
        axios.get('/api/Assay/PrintAssay?' +
            'id=' + id +
            '&to=' + to).then((res) => {
                let jobj = res.data as server.resultJSON<server.assay>;
                if (jobj.code == 0) {
                    this.toastSuccess('化验单打印指令已发出')
                }
            });
    }
}