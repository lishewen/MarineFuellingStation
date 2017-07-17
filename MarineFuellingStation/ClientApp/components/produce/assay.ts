import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    radio2: string = "1";
    carNo: string = "";
    filterclick(): void {
    };
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 化验');
        this.$watch('radio2', (v, ov) => {

        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}