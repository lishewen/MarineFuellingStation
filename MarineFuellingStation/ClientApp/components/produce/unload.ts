import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class MyOrderComponent extends ComponentBase {
    radio2: string = "1";
    carNo: string = "";
    showPurchases: boolean = false;
    picked: string = "Lucy";

    purchases: server.purchase[];
    purchase: server.purchase;

    constructor() {
        super();

        this.purchases = new Array<server.purchase>();
        this.purchase = new Object as server.puchase;
        this.getPurchases();
    }

    purchaseclick(pu: server.purchase){
        this.purchase = pu;
        this.showPurchases = false;
        this.purchase.state = 0
    }

    changeState(state: server.unloadState) {
        if (state == server.unloadState.油车过磅) {
            if (this.purchase.scaleWithCar == 0 || !this.purchase.scaleWithCar) {
                this.toastError("磅秤数据不能为空")
                return;
            }
        }
        console.log(this.purchase);
        console.log(state);
        this.putState(state);
    }
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 陆上卸油');
        this.$watch('show1', (v, ov) => {
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getPurchases() {
        axios.get('/api/Purchase').then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase[]>;
            if (jobj.code == 0)
                this.purchases = jobj.data;
        });
    }

    putState(state: server.unloadState) {
        let model = this.purchase;
        model.state = state;
        axios.put('/api/Purchase/ChangeState', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase>;
            if (jobj.code == 0) {
                this.purchase = jobj.data;
            }
            else
                this.toastError(jobj.msg);
        });
    }
}