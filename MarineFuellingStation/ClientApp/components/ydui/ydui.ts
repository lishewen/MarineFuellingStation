import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class YDUIComponent extends Vue {
    sv: string = '';

    handleClick(): void {
        (<any>this).$dialog.alert({ mes: 'Hello World!' + this.sv });
    }
}