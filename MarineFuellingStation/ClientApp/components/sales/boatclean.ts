import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class PlanComponent extends Vue {
    radio2: string = '1';
    unit: string = '升';
    carNo: string = '';
    isinvoice: boolean = false;
    sv: string = "";

    workplace: string = "广西梧州市云龙桥下游500米对开河边";
    workphone: string = "07742031178";
    workcompany: string = "广西梧州市汇保源防污有限公司";
    allowNo: string = "梧海事清油（      ）第      号";

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

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}