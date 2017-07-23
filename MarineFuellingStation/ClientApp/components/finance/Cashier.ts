import Vue from 'vue';
import { Component } from 'vue-property-decorator';
@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class MyOrderComponent extends Vue {
    radio1: string = "2";
    radio2: string = "1";
    show1: boolean = false;
    show2: boolean = false;
    type1: boolean = false; type2: boolean = false; type3: boolean = false;
    type4: boolean = false; type5: boolean = false; type6: boolean = false;
    type7: boolean = false;
    lastshow: boolean = true;
    carNo: string = "";
    sv: string = "";
    checkbox2: object = ["1"];

    nextclick(): void{
        this.lastshow = false;  
    };
    lastclick(): void {
        this.lastshow = true;
    };

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 结算');
        this.$watch('checkbox2', (v, ov) => {
            var str = v.join(',');
            this.type1 = (str.indexOf("1") > -1) ? true : false;
            this.type2 = (str.indexOf("2") > -1) ? true : false;
            this.type3 = (str.indexOf("3") > -1) ? true : false;
            this.type4 = (str.indexOf("4") > -1) ? true : false;
            this.type5 = (str.indexOf("5") > -1) ? true : false;
            this.type6 = (str.indexOf("6") > -1) ? true : false;
            this.type7 = (str.indexOf("7") > -1) ? true : false;
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}