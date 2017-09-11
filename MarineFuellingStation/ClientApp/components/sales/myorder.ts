import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    
    show4: boolean = false;
    filterBtns: Array<helper.filterBtn>;
    activedBtnId: number;
    
    timesubmit(): void {
        this.show4 = false;
    };

    constructor() {
        super();

        this.filterBtns = [
            { id: 0, name: '全部', actived: true },
            { id: 1, name: '个人', actived: false },
            { id: 2, name: '公司', actived: false }
        ];
        this.activedBtnId = 0;
    }
    switchBtn(o: any) {
        if (o.id != this.activedBtnId){
            o.actived = true;
            this.filterBtns[this.activedBtnId].actived = false;
            this.activedBtnId = o.id;
        }
    }
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的销售单');
        
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}