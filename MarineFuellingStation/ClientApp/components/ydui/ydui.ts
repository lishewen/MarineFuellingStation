import Vue from 'vue';
import { Component } from 'vue-property-decorator';

export default {
    methods: {
        handleClick() {
            this.$dialog.alert({ mes: 'Hello World!' });
        }
    }
}