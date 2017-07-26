import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    radio2: string = "1";
    carNo: string = "";
    show1: boolean = false;
    show3: boolean = false;
    picked: string = "Lucy";
    showclick(): void {
        this.show1 = true;
    };

    saveclick1(): void {
        this.show1 = false;
    };
    saveclick3(): void {
        this.show3 = false;
    };
    beginConfrim(): void {
        (<any>this).$dialog.confirm({
            title: '确认操作',
            mes: '开始装油？',
            opts: () => {
                (<any>this).$dialog.toast({ mes: '确认', timeout: 1000 });
            }
        })
    };
    endConfrim(): void {
        (<any>this).$dialog.confirm({
            title: '确认操作',
            mes: '装油结束？',
            opts: () => {
                (<any>this).$dialog.toast({ mes: '确认', timeout: 1000 });
            }
        })
    };
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 陆上装油');
        this.$watch('show1', (v, ov) => {
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}