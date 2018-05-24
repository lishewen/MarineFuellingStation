import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component({
    components: {
        //路由懒加载的同时，组件也必须懒加载
        WeuiSearch: resolve => require(['../weui-search/search.vue'], resolve)
    }
})
export default class YDUIComponent extends Vue {
    sv: string = '';

    handleClick(): void {
        (<any>this).$dialog.alert({ mes: 'Hello World!' + this.sv });
    }
}