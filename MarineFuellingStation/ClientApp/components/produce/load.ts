import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class LoadComponent extends ComponentBase {
    carNo: string = "";

    order: server.order;
    orders: server.order[];
    store: server.store;
    stores: server.store[];

    currStep: number = 0;
    page: number = 1;
    showOrders: boolean = false;
    showStores: boolean = false;

    constructor() {
        super();

        this.order = new Object as server.order;
        this.orders = new Array<server.order>();
        this.store = new Object as server.store;
        this.stores = new Array<server.store>();

        this.getOrders();
        this.getStores();
    }
    
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 水上装油');
    };

    orderclick(o: server.order) {
        this.order = o;
        this.showOrders = false;
        console.log(this.order);
        this.matchCurrStep();
        
    }
    matchCurrStep() {
        if (this.order.state == server.orderState.已开单) this.currStep = 1;
        if (this.order.state == server.orderState.装油中) this.currStep = 2;
        if (this.order.state == server.orderState.已完成) this.currStep = 3;
    }

    storeclick(st: server.store) {
        this.store = st;
        this.order.store = st;
        this.showStores = false;
        this.changeState(server.orderState.装油中);
        console.log(this.store);
    }

    changeState(nextState: server.orderState) {
        if (this.currStep == server.orderState.选择销售仓) {
            if (this.store == null) {
                this.toastError("请选择销售仓")
                return;
            }
        }
        this.putState(nextState);
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getOrders() {
        if (this.page == null) this.page = 1;
        axios.get('/api/Order/GetIncludeProduct/' + server.salesPlanType.水上.toString() + '?page=' + this.page.toString())
            .then((res) => {
                let jobj = res.data as server.resultJSON<server.order[]>;
                if (jobj.code == 0) {
                    this.orders = jobj.data;
                    this.page++;
                }
            });
    }

    getStores() {
        axios.get('/api/Store/GetByClass?sc=' + server.storeClass.销售仓.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.store[]>;
            if (jobj.code == 0)
                this.stores = jobj.data;
        });
    }

    putState(state: server.orderState) {
        this.order.state = state;
        axios.put('/api/Order/ChangeState', this.order).then((res) => {
            let jobj = res.data as server.resultJSON<server.order>;
            if (jobj.code == 0) {
                this.order = jobj.data;
                this.matchCurrStep();
                if (this.order.state == server.orderState.已完成) this.getOrders();
            }
            else
                this.toastError(jobj.msg);
        });
    }
}