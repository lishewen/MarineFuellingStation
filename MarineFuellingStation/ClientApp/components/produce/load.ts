import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    radio2: string = "1";
    carNo: string = "";
    show1: boolean = false;
    show2: boolean = false;
    show3: boolean = false;
    picked: string = "Lucy";
    selectedstore: string = "";
    showclick(): void {
        this.show1 = true;
    };
    
    saveclick2(): void {
        this.show2 = false;
        this.selectedstore = "1#加油船759 /";
    };
    openConfrim(): void {
        (<any>this).$dialog.confirm({
            title: '确认操作',
            mes: '卸油结束？',
            opts: () => {
                (<any>this).$dialog.toast({ mes: '确认', timeout: 1000 });
            }
        })
    };
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 水上装油');
        this.$watch('show1', (v, ov) => {
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}