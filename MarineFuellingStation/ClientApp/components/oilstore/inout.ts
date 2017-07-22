import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class MyOrderComponent extends Vue {
    carNo: string = "";
    
    actConfrim(): void {
        (<any>this).$dialog.confirm({
            title: '确认操作',
            mes: '开始施工？',
            opts: () => {
                (<any>this).$dialog.toast({ mes: '确认', timeout: 1000 });
            }
        })
    };
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 出入仓记录');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}