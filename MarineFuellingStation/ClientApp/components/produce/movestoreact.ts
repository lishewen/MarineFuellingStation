import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    carNo: string = "";
    show2: boolean = false;
    
    actConfrim(): void {
        (<any>this).$dialog.confirm({
            title: '确认操作',
            mes: '开始施工？',
            opts: () => {
                (<any>this).$dialog.toast({ mes: '确认', timeout: 1000 });
            }
        })
    };
    saveclick2(): void {
        this.show2 = false;
    }
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 水上装油');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}