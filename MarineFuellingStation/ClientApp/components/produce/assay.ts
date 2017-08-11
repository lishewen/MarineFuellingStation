import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class MyOrderComponent extends Vue {
    radio2: string = "1";
    carNo: string = "";
    show1: boolean = true;
    sv: string = "";
    filterclick(): void {
    };
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 化验');
        this.$watch('radio2', (v, ov) => {
            this.show1 = (v == "1") ? true : false;
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}