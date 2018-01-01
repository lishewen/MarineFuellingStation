import Vue from 'vue';
import moment from "moment";
import axios from "axios";

export default class ComponentBase extends Vue {
    $wechat: any;

    SDKRegister(that: ComponentBase, callback: Function) {
        let model = new Object as server.jSSDKPostModel;
        model.originalUrl = 'https://vue.car0774.com/#' + that.$route.path;
        axios.post('/home/GetJSSDKConfig', model).then(res => {
            let data = res.data as work.JsSdkUiPackage;
            that.$wechat.config({
                debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                appId: data.appId, // 必填，公众号的唯一标识
                timestamp: data.timestamp, // 必填，生成签名的时间戳
                nonceStr: data.nonceStr, // 必填，生成签名的随机串
                signature: data.signature, // 必填，签名，见附录1
                jsApiList: [
                    'checkJsApi',
                    'chooseImage',
                    'onMenuShareTimeline',
                    'onMenuShareAppMessage'
                ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            })
        });
        that.$wechat.ready((res) => {
            // 如果需要定制ready回调方法
            if (callback) {
                callback.call(that)
            }
        });
    }

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
    /** 操作失败提示 */
    toastError(msg: string) {
        this.$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }
    /** 操作成功提示 */
    toastSuccess(msg: string) {
        this.$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'success'
        });
    }
    /** 继续开单提示对话框 */
    addNextConfirm() {
        this.$dialog.confirm({
            title: '操作成功',
            mes: '操作成功，是否继续开单？',
            opts: () => {
                window.location.reload();
            }
        });
    }
    /** 取两位小数 */
    round(val: number) {
        return Math.round(val * 100) / 100;
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
    //========================== enum value to key ==============================
    strSalesPlanState(st: server.salesPlanState) {
        switch (st) {
            case server.salesPlanState.未审批:
                return "未审批";
            case server.salesPlanState.已审批:
                return "已审批";
            case server.salesPlanState.已完成:
                return "已完成";
        }
    }
    /**
     * 取得船舶清污状态，已开单|施工中|已完成
     * @param s
     */
    strBoatCleanState(s: server.boatCleanState): string {
        switch (s) {
            case server.boatCleanState.已开单:
                return '已开单';
            case server.boatCleanState.施工中:
                return '施工中';
            case server.boatCleanState.已完成:
                return '已完成';
        }
    }
    /**
     * 取得充值或消费类型，充值|消费
     * @param t
     */
    strChargeType(t: server.chargeType) {
        if (t == server.chargeType.充值) {
            return "充值"
        }
        else if (t == server.chargeType.消费) {
            return "消费"
        }
    }
    /**
     * 取得开票类型，循票|柴票
     * @param tt
     */
    strTicketType(tt: server.ticketType) {
        switch (tt) {
            //case server.ticketType.普通票:
            //    return "普通票";
            //case server.ticketType.专用票:
            //    return "专用票";
            case server.ticketType.循票:
                return "循";
            case server.ticketType.柴票:
                return "柴";
        }
    }
    /**
     * 取得出入仓类型，出仓|入仓
     * @param t
     */
    strLogType(t: server.logType) {
        if (t == server.logType.入仓)
            return "入仓"
        else
            return "出仓"
    }
    /**
     * 取得订单状态，已完成|装油中|已开单
     * @param s
     */
    strOrderState(s: server.orderState): string {
        switch (s) {
            case server.orderState.已完成:
                return '已完成';
            case server.orderState.装油中:
                return '装油中';
            case server.orderState.已开单:
                return '已开单';
        }
    }
    /**
     * 取得付款状态
     * @param o
     */
    strPayState(s: server.payState) {
        switch (s) {
            case server.payState.未结算:
                return "未结算";
            case server.payState.挂账:
                return "挂账";
            case server.payState.已结算:
                return "已结算"
        }
    }
    /**
     * 取得订单支付方式，现金|微信|支付宝|桂行刷卡|刷卡三|账户扣减|公司账户扣减
     * @param pt
     */
    strOrderPayType(pt: server.orderPayType) {
        switch (pt) {
            case server.orderPayType.现金:
                return "现金"
            case server.orderPayType.微信:
                return "微信"
            case server.orderPayType.支付宝:
                return "支付宝"
            case server.orderPayType.桂行刷卡:
                return "桂行刷卡"
            case server.orderPayType.工行刷卡:
                return "工行刷卡"
            case server.orderPayType.刷卡三:
                return "刷卡三"
            case server.orderPayType.账户扣减:
                return "账户扣减"
            case server.orderPayType.公司账户扣减:
                return "公司账户扣减"
        }
    }
}