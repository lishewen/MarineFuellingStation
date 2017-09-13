﻿import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class OilStoreComponent extends ComponentBase {
    carNo: string = "";
    progress1: number = 0.4;
    progress2: number = 0.4;
    show1: boolean = false;

    survey: server.survey;
    sts: server.storeType[];
    salesSts: server.store[];

    constructor() {
        super();

        this.sts = new Array<server.storeType>();
        this.salesSts = new Array<server.store>();
        this.survey = new Object() as server.survey;
        this.getStoreTypes();
    }
    
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 油仓情况');
    };
    /**
     * 当前数量百分比
     */
    getPercent(val: number, vol: number) {
        return Math.round(val / vol * 10) / 10;
    }

    getStoreTypes() {
        axios.get('/api/StoreType').then((res) => {
            let jobj = res.data as server.resultJSON<server.storeType[]>;
            if (jobj.code == 0) {
                this.sts = jobj.data;
                jobj.data.forEach((o, i) => {
                    o.stores.forEach((s, ii) => {
                        if (s.storeClass == server.storeClass.销售仓)
                            this.salesSts.push(s);
                    });
                })
                console.log(this.sts);
            }
        });
    }

    validate() {
        if (this.survey.temperature == '') {
            this.toastError('油温不能为空');
            return false;
        }
        if (this.survey.density == '') {
            this.toastError('密度不能为空');
            return false;
        }
        if (this.survey.height == '') {
            this.toastError('油高不能为空');
            return false;
        }
        return true;
    }

    postSurveyclick() {
        if (!this.validate()) return;
        axios.post('/api/Survey', this.currentproduct).then((res) => {
            let jobj = res.data as server.resultJSON<server.survey>;
            if (jobj.code == 0) {
                this.toastSuccess(jobj.msg);
            }
        });
    }

    change(label: string, tabkey: string) {
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}