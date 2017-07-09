import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class AppComponent extends Vue {
    title: string = this.$store.state.username + '正在登录中。。。';

    setTitle(title: string): void {
        this.title = title;
    }
}
