import Vue from 'vue';
import { Component } from 'vue-property-decorator';
@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class MyOrderComponent extends Vue {
    radio1: string = "2";
    show2: boolean = false;
    carNo: string = "";
    sv: string = "";
    filterclick(): void {
        this.show2 = false;
    };
   
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
}