import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class AppComponent extends Vue {
    title: string = this.$store.state.username + '正在登录中。。。';
    backUrl: string = '#';

    setTitle(title: string, url: string = '#'): void {
        this.title = title;
        this.backUrl = url;
    }
}
