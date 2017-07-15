import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    
    show4: boolean = false;
    
    //timesubmit(): void {
    //    this.show4 = false;
    //};
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的客户');

    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}