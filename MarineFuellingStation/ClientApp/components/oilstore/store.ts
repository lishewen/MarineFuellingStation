import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class StoreComponent extends Vue {
    carNo: string = "";
    show1: boolean = false;
    newstshow: boolean = false;
    radio1: string = "1";
    model: server.store;
    stName: string = '';
    sts: server.storeType[];

    constructor() {
        super();

        this.model = (new Object()) as server.store;
        this.sts = new Array();

        this.getStoreTypes();
    }

    toastError(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }

    newstShowClick() {
        this.stName = '';
        this.newstshow = true;
    }

    getStoreTypes() {
        axios.get('/api/StoreType').then((res) => {
            let jobj = res.data as server.resultJSON<server.storeType[]>;
            if (jobj.code == 0) {
                this.sts = jobj.data;
            }
        });
    }

    postStoreType() {
        if (this.stName == '') {
            this.toastError('分类名称不能为空');
            return;
        }

        let stmodel = (new Object()) as server.storeType;
        stmodel.name = this.stName;
        axios.post('/api/StoreType', stmodel).then((res) => {
            let jobj = res.data as server.resultJSON<server.storeType>;
            if (jobj.code == 0) {
                (<any>this).$dialog.toast({
                    mes: jobj.msg,
                    timeout: 1500,
                    icon: 'success'
                });
                //将新增的分类加入到列表中
                this.sts.push(jobj.data);
                //关闭popup
                this.newstshow = false;
            }
        });
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 油仓');

        this.$watch('radio1', (v, ov) => {
            switch (v) {
                case "1":
                    this.model.storeClass = server.storeClass.销售仓;
                    break;
                case "2":
                    this.model.storeClass = server.storeClass.存储仓;
                    break;
            }
        });
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}