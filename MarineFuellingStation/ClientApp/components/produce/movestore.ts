import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class MoveStoreComponent extends Vue {
    model: server.moveStore;
    manufacturer: work.userlist[];
    picked: string[];
    store: server.store[];
    selectedOutStore: number | string = '';
    selectedInStore: number | string = '';

    radio2: string = "1";
    carNo: string = "";
    show1: boolean = false;

    filterclick(): void {
    };

    selectproducerclick(): void {
        this.model.manufacturer = this.picked.join('|');
        this.show1 = false;
    };

    constructor() {
        super();

        this.picked = new Array<string>();
        this.model = (new Object()) as server.moveStore;
        this.model.name = '';
        this.model.manufacturer = '';

        this.getMoveStoreNo();
        this.getManufacturer();
        this.getStore();
    }

    buttonclick() {
        //信息验证

        this.postMoveStore(this.model);
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 生产开单');
        this.$watch('radio2', (v, ov) => {

        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    getMoveStoreNo() {
        axios.get('/api/MoveStore/MoveStoreNo').then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                this.model.name = jobj.data;
        });
    }
    //** 获得生产员 */
    getManufacturer() {
        axios.get('/api/User/Manufacturer').then((res) => {
            let jobj = res.data as work.tagMemberResult;
            if (jobj.errcode == 0)
                this.manufacturer = jobj.userlist;
        });
    }

    getStore() {
        axios.get('/api/Store').then((res) => {
            let jobj = res.data as server.resultJSON<server.store[]>;
            if (jobj.code == 0)
                this.store = jobj.data;
        });
    }

    postMoveStore(model: server.moveStore) {
        axios.post('/api/MoveStore', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.moveStore>;
            if (jobj.code == 0) {
                this.getMoveStoreNo();
                (<any>this).$dialog.toast({
                    mes: jobj.msg,
                    timeout: 1500,
                    icon: 'success'
                });
            }
        });
    }
}