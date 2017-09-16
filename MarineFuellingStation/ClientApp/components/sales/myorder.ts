import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import moment from 'moment';
import axios from "axios";

@Component
export default class MyOrderComponent extends ComponentBase {

    show4: boolean = false;
    filterBtns: Array<helper.filterBtn>;
    activedBtnId: number;
    startDate: string;
    endDate: string;
    orders: server.order[];
    page: number = 1;

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
    }
    switchBtn(o: any) {
        if (o.id != this.activedBtnId) {
            o.actived = true;
            this.filterBtns[this.activedBtnId].actived = false;
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

    refresh() {
        this.page = 1;
        this.getOrders();
    }

    formatDate(d: Date) {
        return moment(d).format('YYYY-MM-DD');
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的销售单');
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

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getOrders() {
        let sTimespan = ' 00:00';
        let eTimespan = ' 23:59';
        if (this.startDate == null) this.startDate = this.formatDate(new Date());
        if (this.endDate == null) this.endDate = this.startDate;
        if (this.page == null) this.page = 1;
        axios.get('/api/Order/GetMyOrders?page=' + this.page.toString() + '&size=30&startDate=' + this.startDate + sTimespan + '&endDate=' + this.endDate + eTimespan)
            .then((res) => {
                let jobj = res.data as server.resultJSON<server.order[]>;
                if (jobj.code == 0) {
                    this.orders = jobj.data;
                    this.page++;
                }
            });
    }
}