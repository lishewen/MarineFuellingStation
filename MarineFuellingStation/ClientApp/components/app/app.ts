import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class AppComponent extends Vue {
    title: string = 'XXX油站管理系统';

    mounted() {
        let that = this;
        this.$on('setTitle', (title: string) => {
            that.title = title;
        });
    }

    setTitle(title: string): void {
        this.title = title;
    }
}
