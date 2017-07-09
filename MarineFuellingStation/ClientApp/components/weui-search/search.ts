import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component({
    name: 'weui-search',
    props: {
        value: String,
        autofocus: Boolean,
        show: Boolean,
        placeholder: {
            type: String,
            default: '搜索'
        },
        cancelText: {
            type: String,
            default: '取消'
        },
        result: Array
    },
    watch: {
        currentValue(val) {
            this.$emit('input', val);
        },

        value(val) {
            (<Search>this).currentValue = val;
        }
    }
})
export default class Search extends Vue {
    name: string = 'weui-search';
    autofocus: boolean;
    isActive: boolean;
    value: string;
    currentValue: string;

    data() {
        return {
            isActive: false,
            currentValue: this.value
        }
    }

    mounted() {
        if (this.autofocus) {
            console.log('fuck');
            (<any>this.$refs.searchInput).focus();
            this.isActive = true;
        }
    }

    textClick(e) {
        // focus the input
        (<any>this.$refs.searchInput).focus()
        this.isActive = true
    }

    searchClear() {
        this.currentValue = ''
    }

    searchCancel() {
        this.searchClear()
        this.isActive = false
    }
}