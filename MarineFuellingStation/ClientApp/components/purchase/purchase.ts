import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class PurchaseComponent extends Vue {
    model: server.purchase;

    radio1: string = "2";
    show2: boolean = false;
    carNo: string = "";
    sv: string = "";
    filterclick(): void {
        this.show2 = false;
    };
    constructor() {
        super();

        this.model = (new Object()) as server.purchase;
        this.model.name = '';

        this.getPurchaseNo();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的计划');
        this.$watch('radio1', (v, ov) => {
            switch (v) {
                case "1":
                    this.show2 = false;
                    break;
                case "2":
                    this.show2 = true;
                    break;
            }
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getPurchaseNo() {
        axios.get('/api/Purchase/PurchaseNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                this.model.name = jobj.data;
        });
    }
}