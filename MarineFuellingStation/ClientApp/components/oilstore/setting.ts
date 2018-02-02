import ComponentBase from "../../ComponentBase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class OilSettingComponent extends ComponentBase {
    stores: server.store[];
    isPrevent: boolean = false;

    constructor() {
        super();

        this.stores = new Array<server.store>();

        this.getStores();
    }

    mounted() {
        this.$emit('setTitle', ' 油仓初始化');
    };

    saveclick() {
        console.log(this.stores);
        this.postStores();
    }

    getStores() {
        axios.get('/api/Store').then((res) => {
            let jobj = res.data as server.resultJSON<server.store[]>;
            if (jobj.code == 0) {
                this.stores = jobj.data;
            }
        });
    }

    postStores() {
        this.isPrevent = true;
        axios.post('/api/Store/PostStores', this.stores).then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0) {
                this.toastSuccess(jobj.msg);
                this.isPrevent = false;
            }
        });
    }
    
}