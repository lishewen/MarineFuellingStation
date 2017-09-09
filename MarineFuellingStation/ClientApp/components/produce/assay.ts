﻿import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class AssayComponent extends Vue {
    model: server.assay;

    radio2: string = "1";
    carNo: string = "";
    show1: boolean = true;
    sv: string = "";
    filterclick(): void {
    };

    constructor() {
        super();

        this.model = (new Object()) as server.assay;
        this.model.name = '';

        this.getAssayNo();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 化验');
        this.$watch('radio2', (v, ov) => {
            this.show1 = (v == "1") ? true : false;
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getAssayNo() {
        axios.get('/api/Assay/AssayNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                this.model.name = jobj.data;
        });
    }
}