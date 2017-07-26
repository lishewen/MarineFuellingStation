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
    show3: boolean = true;
    show4: boolean = false;
    show5: boolean = false;
    carNo: string = "";
    sv: string = "";
    picked: string = "";
    filterclick(): void {
        this.show2 = false;
    };
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的客户');
        this.$watch('radio2', (v, ov) => {
            switch (v) {
                case "1":
                    this.show1 = false;
                    break;
                case "2":
                    this.show1 = true;
                    break;
            }
        });
        this.$watch('radio1', (v, ov) => {
            switch (v) {
                case "1":
                    this.show3 = false;
                    break;
                case "2":
                    this.show3 = true;
                    break;
            }
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}