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

    stypeFrom: helper.filterBtn[];
    stypeTo: helper.filterBtn[];
    actBtnId: number; actBtnId1: number;

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
        this.store = new Array<server.store>();
        this.manufacturer = new Array<work.userlist>();

        this.stypeFrom = new Array<helper.filterBtn>();
        this.stypeTo = new Array<helper.filterBtn>();
        this.actBtnId = -1;
        this.actBtnId1 = -1;

        this.getStoreTypes();

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

    switchBtn(o: helper.filterBtn, idx: number, group: string) {
        
        switch (group) {
            case "转出仓":
                o.actived = true;
                this.model.outStoreId = parseInt(o.value.toString());
                if (idx != this.actBtnId && this.actBtnId != -1) {
                    this.stypeFrom[this.actBtnId].actived = false;
                    this.actBtnId = idx;
                }
                else
                    this.actBtnId = idx;
                break;
            case "转入仓":
                o.actived = true;
                this.model.inStoreId = parseInt(o.value.toString());
                if (idx != this.actBtnId1 && this.actBtnId1 != -1) {
                    this.stypeTo[this.actBtnId1].actived = false;
                    this.actBtnId1 = idx;
                }
                else
                    this.actBtnId1 = idx;
                break;
        }
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

    getStoreTypes() {
        axios.get('/api/StoreType').then((res) => {
            let jobj = res.data as server.resultJSON<server.storeType[]>;
            if (jobj.code == 0) {
                jobj.data.forEach((st, idx) => {
                    this.stypeFrom.push({
                        id: idx,
                        name: st.name,
                        value: st.id,
                        actived: false
                    });
                    this.stypeTo.push({
                        id: idx,
                        name: st.name,
                        value: st.id,
                        actived: false
                    });
                });
                console.log(this.stypeFrom);
            }
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