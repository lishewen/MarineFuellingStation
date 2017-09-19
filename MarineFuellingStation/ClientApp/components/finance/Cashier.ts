import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class CashierComponent extends ComponentBase {
    radio1: string = "2";
    radio2: string = "1";
    show1: boolean = false;
    show2: boolean = false;
    lastshow: boolean = true;
    carNo: string = "";
    sv: string = "";
    checkbox2: object = ["1"];

    orderPayTypes: Array<string>;
    orderPayMoneys: Array<number>;

    showInputs: Array<boolean>;
    payments: Array<server.payment>;
    selectedOrder: server.order;

    orders: server.order[];
    page: number;

    constructor() {
        super();

        this.payments = new Array<server.payment>();
        this.selectedOrder = new Object() as server.order;
        this.orderPayTypes = ["0"];
        this.orderPayMoneys = new Array<number>();
        this.showInputs = new Array<boolean>(false, false, false, false, false, false, false);
        this.orders = new Object() as server.order;
        this.getOrders();
    }

    orderclick(o: server.order) {
        this.selectedOrder = o;
        this.show2 = true;
    }

    nextclick(): void{
        this.lastshow = false;
        this.showInputs = new Array<boolean>(false, false, false, false, false, false, false);
        this.orderPayTypes.forEach((p, idx) => {
            this.showInputs[p] = true;
        });
        console.log(this.orderPayTypes);
    };
    lastclick(): void {
        this.lastshow = true;
    };

    getDiff(d: Date) {
        return moment(d).startOf('day').fromNow();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 结算');
        
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getOrders() {
        if (!this.page) this.page = 1;
        axios.get('/api/Order/GetIncludeProduct?page=' + this.page.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.order[]>;
            if (jobj.code == 0)
                this.orders = jobj.data;
        });
    }

    postPay() {
        this.orderPayTypes.forEach((p, idx) => {
            this.payments.push(
                { payTypeId: parseInt(p), money: this.orderPayMoneys[parseInt(p)] }
            );
        });
        console.log(this.payments)
    }
}