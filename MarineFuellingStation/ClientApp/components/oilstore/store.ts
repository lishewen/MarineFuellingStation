import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class StoreComponent extends ComponentBase {
    carNo: string = "";
    stshow: boolean = false;
    newstshow: boolean = false;
    radio1: string = "1";
    isAddStore: boolean = true;
    isAddStoreType: boolean = true;
    model: server.store;
    stName: string = '';
    sts: server.storeType[];
    currentst: server.storeType;

    constructor() {
        super();

        this.sts = new Array<server.storeType>();
        this.currentst = new Object() as server.storeType;
        this.model = new Object() as server.store;
        this.model.name = '';
        this.model.volume = 0;
        this.model.storeTypeId = -1;

        this.getStoreTypes();
    }

    stClick(st: server.storeType) {
        this.currentst = st;
        this.stshow = true;
    }

    addStoreclick() {
        if (!this.validate()) return;
        this.postStore(this.model);
    }

    editStoreclick(st: server.store) {
        this.isAddStore = false;
        this.stshow = false;
        this.getStore(st.id);
    }

    editStoreTypeclick() {
        this.isAddStoreType = false;
        this.stshow = false;
        this.newstshow = true;
        this.getStoreType(this.currentst.id);
    }

    validate() {
        //信息验证
        if (this.model.name == '') {
            this.toastError('名称不能为空');
            return false;
        }
        if (this.model.volume <= 0) {
            this.toastError('请输入容量');
            return false;
        }
        if (this.model.storeTypeId <= 0) {
            this.toastError('必须选择分类');
            return false;
        }
        return true;
    }

    saveStoreclick() {
        if (!this.validate()) return;
        this.putStore(this.model);
    }
    saveStoreTypeclick() {
        let stmodel = (new Object()) as server.storeType;
        stmodel.id = this.currentst.id;
        stmodel.name = this.stName;
        this.putStoreType(stmodel);
    }

    sumVolume(st: server.storeType): number {
        let result: number = 0;
        st.stores.forEach((s) => {
            result += s.volume;
        });
        return result;
    }

    sumValue(st: server.storeType): number {
        let result: number = 0;
        st.stores.forEach((s) => {
            result += s.value;
        });
        return result;
    }

    sumCost(st: server.storeType): number {
        let result: number = 0;
        st.stores.forEach((s) => {
            result += s.cost;
        });
        return result;
    }

    newstShowClick() {
        this.stName = '';
        this.newstshow = true;
        this.isAddStoreType = true;
    }

    getStore(id: number) {
        axios.get('/api/Store/' + id.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.store>;
            if (jobj.code == 0) {
                this.model = jobj.data;
                this.model.isForLand = this.model.isForLand.toString();
                console.log(this.model.isForLand)
            }
        });
    }

    getStoreType(id: number) {
        axios.get('/api/StoreType/' + id.toString()).then((res) => {
            let jobj = res.data as server.resultJSON<server.store>;
            if (jobj.code == 0) {
                this.stName = jobj.data.name;
            }
        });
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
                this.toastSuccess(jobj.msg);
                //将新增的分类加入到列表中
                this.sts.push(jobj.data);
                //关闭popup
                this.newstshow = false;
            }
        });
    }

    postStore(model: server.store) {
        axios.post('/api/Store', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.store>;
            if (jobj.code == 0) {
                this.toastSuccess(jobj.msg);
            }
        });
    }

    putStore(model: server.store) {
        axios.put('/api/Store', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.store>;
            if (jobj.code == 0) {
                this.toastSuccess(jobj.msg);
            }
        });
    }

    putStoreType(model: server.storeType) {
        axios.put('/api/StoreType', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.storeType>;
            if (jobj.code == 0) {
                this.toastSuccess(jobj.msg);
                this.getStoreTypes();
            }
        });
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 油仓');
        this.$watch('radio1', (v, ov) => {
            switch (v) {
                case "0":
                    this.model.storeClass = server.storeClass.销售仓;
                    break;
                case "1":
                    this.model.storeClass = server.storeClass.存储仓;
                    break;
            }
        });
    };

    change(label: string, tabkey: string) {
        if (label == '添加') {
            this.model = (new Object()) as server.store;
            this.model.name = '';
            this.model.volume = 0;
            this.model.storeTypeId = -1;
            this.model.isForLand = "false"
        }
        if (label == '所有分类')
            this.isAddStore = true;
        console.log(label)
    }
}