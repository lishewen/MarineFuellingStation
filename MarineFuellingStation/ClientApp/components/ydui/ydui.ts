import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import 'vue-ydui/dist/ydui.px.css';

export default {
    methods: {
        handleClick() {
            this.$dialog.alert({ mes: 'Hello World!' });
        }
    }
}