import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    radio2: string = "1";
    carNo: string = "";
    show1: boolean = false;
    picked: object = ['生产员1'];
    filterclick(): void {
    };
    selectproducerclick(): void {
        this.show1 = false;
    };
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 生产开单');
        this.$watch('radio2', (v, ov) => {

        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}