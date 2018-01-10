import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import ComponentBase from '../../ComponentBase';
import wx from 'wx-sdk-ts';

@Component
export default class HomeComponent extends ComponentBase {
    beforeCreate() {
        this.$wechat = wx;
        this.SDKRegister(this, () => {
            this.$wechat.checkJsApi({
                jsApiList: ['chooseImage'], // 需要检测的JS接口列表，所有JS接口列表见附录2,
                success: function (res) {
                    console.log(res);
                }
            });
        });
    }

    mounted() {
        this.$dialog.loading.open('很快加载好了');

        setTimeout(() => {
            this.$dialog.loading.close();
        }, 2000);
    }
}