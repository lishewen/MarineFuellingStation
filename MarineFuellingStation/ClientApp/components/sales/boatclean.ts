import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class BoatCleanComponent extends Vue {
    model: server.boatClean;

    radio2: string = '1';
    unit: string = '升';
    carNo: string = '';
    sv: string = "";

    constructor() {
        super();

        this.model = (new Object()) as server.boatClean;
        this.model.name = '';
        this.model.responseId = "梧海事清油（      ）第      号";
        this.model.address = "广西梧州市云龙桥下游500米对开河边";
        this.model.company = "广西梧州市汇保源防污有限公司";
        this.model.phone = "07742031178";
        this.model.isInvoice = false;

        this.getBoatCleanNo();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 船舶清污');
        //观察者模式
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.unit = '升';
                    break;
                case "2":
                    this.unit = '吨';
                    break;
                case "3":
                    this.unit = '桶';
                    break;
            }
        });
    };

    buttonclick() {
        //信息验证

        this.postBoatClean(this.model);
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getBoatCleanNo() {
        axios.get('/api/BoatClean/BoatCleanNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                this.model.name = jobj.data;
        });
    }

    postBoatClean(model: server.boatClean) {
        axios.post('/api/BoatClean', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.boatClean>;
            if (jobj.code == 0) {
                this.getBoatCleanNo();
                (<any>this).$dialog.toast({
                    mes: jobj.msg,
                    timeout: 1500,
                    icon: 'success'
                });
            }
        });
    }
}