import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class HomeComponent extends Vue {
    mounted() {
        this.$dialog.loading.open('很快加载好了');

        setTimeout(() => {
            this.$dialog.loading.close();
        }, 2000);
    }
}