import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    
    show4: boolean = false;
    
    timesubmit(): void {
        this.show4 = false;
    };
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的销售单');

        //观察者模式
        //this.$watch('radio2', (v, ov) => {
        //    switch (v) {
        //        case "1":
        //            this.unit = '升';
        //            break;
        //        case "2":
        //            this.unit = '吨';
        //            break;
        //        case "3":
        //            this.unit = '桶';
        //            break;
        //    }
        //});
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}