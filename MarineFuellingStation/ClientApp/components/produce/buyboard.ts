import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class BuyboardComponent extends Vue {
    list: server.purchase[];
    constructor() {
        super();

        this.list = new Array<server.purchase>();
        this.getPurchases();
    }
    mounted() {
        this.$emit('setTitle', '采购看板');
    }
    toastError(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }

    godetail(id: number) {
        this.$router.push('/purchase/purchase/' + id);
    }
    formatDate(d: Date): string {
        return moment(d).format('MM-DD hh:mm');
    }
    getPurchases() {
        axios.get('/api/Purchase').then((res) => {
            let jobj = res.data as server.resultJSON<server.purchase[]>;
            if (jobj.code == 0)
                this.list = jobj.data;
        });
    }
}