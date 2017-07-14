import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class OrderComponent extends Vue {
    radio2: string = '1';
    unit: string = '升';
    carNo: string = '';
    isinvoice: boolean = false;
    show4: boolean = false;
    selectedplanNo: string = "";
    hasplan: boolean = false;
    
    planitemclick(): void {
        this.selectedplanNo = "JH201707070001";
        this.show4 = false;
        this.hasplan = true;
    };

    emptyclick(): void {
        this.selectedplanNo = "";
        this.show4 = false;
        this.hasplan = false;
    };
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 销售单');

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