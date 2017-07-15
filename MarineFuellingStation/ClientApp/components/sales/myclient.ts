import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    radio2: string = "1";
    show1: boolean = false;
    show2: boolean = false;
    carNo: string = "";
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
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}