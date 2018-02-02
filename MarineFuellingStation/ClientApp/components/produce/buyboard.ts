import ComponentBase from "../../ComponentBase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class BuyboardComponent extends ComponentBase {
    list: server.purchase[];
    constructor() {
        super();

        this.list = new Array<server.purchase>();
        this.getPurchases();
    }
    mounted() {
        this.$emit('setTitle', '进油看板');
    }
    toastError(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }

    godetail(id: number) {
        this.$router.push('/purchase/purchase/' + id + '/board');
    }
    getPurchases() {
        axios.get('/api/Purchase/GetAllByState?pus=' + server.unloadState.已开单).then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase[]>;
            if (jobj.code == 0)
                this.list = jobj.data;
        });
    }
}