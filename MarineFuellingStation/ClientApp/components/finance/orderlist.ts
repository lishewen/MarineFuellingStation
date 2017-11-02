import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import moment from 'moment';
import axios from "axios";

@Component
export default class OrderListComponent extends ComponentBase {

    show4: boolean = false;
    showSalesmans: boolean = false;
    filterBtns: Array<helper.filterBtn>;
    activedBtnId: number;
    startDate: string;
    endDate: string;
    orders: server.order[];
    sales: work.userlist[];
    selectedsales: string = "";
    page: number = 1;

    scrollRef: any;
    pSize: number = 30;

    timesubmit(): void {
        this.show4 = false;
    };

    constructor() {
        super();

        this.orders = new Array<server.order>();

        this.filterBtns = [
            { id: 0, name: '今日', actived: true },
            { id: 1, name: '昨日', actived: false },
            { id: 2, name: '本周', actived: false },
            { id: 3, name: '本月', actived: false }

        ];
        this.startDate = this.formatDate(new Date());
        this.endDate = this.startDate;
        this.activedBtnId = 0;

        this.getOrders();
        this.getSales();
    }

    loadList() {
        this.getOrders((list: server.order[]) => {
            this.orders = this.page > 1 ? [...this.orders, ...list] : this.orders;
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

    switchBtn(o: any, idx: number) {
        if (o.id != this.activedBtnId && this.activedBtnId != -1) {
            o.actived = true;
            this.filterBtns[this.activedBtnId].actived = false;
            this.activedBtnId = o.id;
        }
        if (this.activedBtnId == -1) {
            o.actived = true;
            this.activedBtnId = o.id;
        }   
        this.matchDate(o);
    }

    matchDate(o: any) {
        let today = this.formatDate(new Date());
        switch (o.name) {
            case '今日':
                this.startDate = today;
                this.endDate = today;
                break;
            case '昨日':
                this.startDate = this.formatDate(moment(today).add(-1).toDate());
                this.endDate = this.startDate;
                break;
            case '本周':
                this.startDate = this.formatDate(moment().weekday(1).toDate());
                this.endDate = this.formatDate(moment().weekday(7).toDate());
                break;
            case '本月':
                this.startDate = this.formatDate(moment().startOf('month').toDate());
                this.endDate = this.formatDate(moment().endOf('month').toDate());
                break;
        }
        this.refresh();
    }

    getTotalSalesComm() {
        let sum = 0;
        this.orders.forEach((o, idx) => {
            sum += o.salesCommission;
        });
        return "总提：￥" + sum;
    }

    godetail(id: number) {
        this.$router.push('/sales/order/' + id + '/myorder');
    }

    query() {
        if (this.activedBtnId != -1)
            this.filterBtns[this.activedBtnId].actived = false;
        this.activedBtnId = -1;
        this.refresh();
    }

    selectsalesclick(s: work.userlist) {
        this.selectedsales = s.name;
        this.refresh();
        this.showSalesmans = false;
    }

    refresh() {
        this.page = 1;
        this.getOrders();
    }

    formatDate(d: Date) {
        return moment(d).format('YYYY-MM-DD');
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 销售单查询');
    };

    strState(o: server.order) {
        let sta = o.state;
        switch (sta) {
            case server.orderState.已开单:
                return "已开单";
            case server.orderState.装油中:
                return "装油中";
            case server.orderState.已完成:
                return "已完成"
        }
        return;
    }

    strPayState(o: server.order) {
        let sta = o.payState;
        switch (sta) {
            case server.payState.未结算:
                return "未结算";
            case server.payState.挂账:
                return "挂账";
            case server.payState.已结算:
                return "已结算"
        }
        return;
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getOrders(callback?: Function) {
        let sTimespan = ' 00:00';
        let eTimespan = ' 23:59';
        if (this.startDate == null) this.startDate = this.formatDate(new Date());
        if (this.endDate == null) this.endDate = this.startDate;
        if (this.page == null) this.page = 1;
        if (this.selectedsales == null) this.selectedsales = "";
        axios.get('/api/Order/GetOrders?page=' + this.page.toString()
            + '&size=30&startDate=' + this.startDate + sTimespan
            + '&endDate=' + this.endDate + eTimespan
            + '&sales=' + this.selectedsales)
            .then((res) => {
                let jobj = res.data as server.resultJSON<server.order[]>;
                if (jobj.code == 0) {
                    if (callback) {
                        callback(jobj.data);
                    }
                    else {
                        this.orders = jobj.data;
                        this.page++;
                    }
                }
            });
    }

    //获得销售员
    getSales() {
        axios.get('/api/User/Salesman').then((res) => {
            let jobj = res.data as work.tagMemberResult;
            if (jobj.errcode == 0)
                this.sales = jobj.userlist;
        });
    }
}