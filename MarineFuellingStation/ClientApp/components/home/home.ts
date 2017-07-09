import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class HomeComponent extends Vue {
    mounted() {
        let that = <any>this;
        that.$dialog.loading.open('很快加载好了');

        setTimeout(() => {
            that.$dialog.loading.close();
        }, 2000);
    }
}