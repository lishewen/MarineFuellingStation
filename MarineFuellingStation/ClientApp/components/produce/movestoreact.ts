import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class MoveStoreActComponent extends ComponentBase {
    show2: boolean = false;
    movestores: server.moveStoreGET[];
    fnmovestores: server.moveStoreGET[];
    model: server.moveStore;
    menus: ydui.actionSheetItem[];
    showMenus: boolean = false;

    constructor() {
        super();

        this.movestores = new Array<server.moveStoreGET>();
        this.fnmovestores = new Array<server.moveStoreGET>();
        this.model = new Object() as server.moveStore;
        
        this.getMoveStores();
        this.getFnMoveStores();
    }

    changeState(m: server.moveStoreGET) {
        this.show2 = true;
        this.model.id = m.id;
        this.model.inStoreId = m.inStoreId;
        this.model.outStoreId = m.outStoreId;
        this.model.outPlan = m.outPlan;
    };
    overclick() {
        if (this.model.outFact == null || this.model.outFact <= 0) {
            this.toastError("实际转出数量不能为空或小于等于0")
            return;
        }
        if (this.model.inFact == null || this.model.inFact <= 0) {
            this.toastError("实际转入数量不能为空或小于等于0")
            return;
        }
        this.putInOutFact()
    }

    //添加actionsheet items
    showMenuclick(mid: number) {
        this.menus = new Array();
        this.menus = [
            {
                label: '打印到【收银台】',
                callback: () => {
                    this.getPrintMoveStore(mid, '收银台')
                }
            }
        ];
        this.showMenus = true;
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 生产施工');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        
    }

    getMoveStores() {
        axios.get('/api/MoveStore?isfinished=false').then((res) => {
            let jobj = res.data as server.resultJSON<server.moveStoreGET[]>;
            if (jobj.code == 0)
                this.movestores = jobj.data;
        });
    }

    getFnMoveStores() {
        axios.get('/api/MoveStore?isfinished=true').then((res) => {
            let jobj = res.data as server.resultJSON<server.moveStoreGET[]>;
            if (jobj.code == 0)
                this.fnmovestores = jobj.data;
        });
    }

    putState(mstore: server.moveStoreGET) {
        let m = new Object() as server.moveStore;
        m.state = mstore.state;
        m.id = mstore.id;
        axios.put('/api/MoveStore/ChangeState', m).then((res) => {
            let jobj = res.data as server.resultJSON<server.moveStore>;
            if (jobj.code == 0) {
                this.getMoveStores();
                this.toastSuccess("操作成功")
            }
            else
                this.toastError(jobj.msg);
        });
    }

    putInOutFact() {
        axios.put('/api/MoveStore/UpdateInOutFact', this.model).then((res) => {
            let jobj = res.data as server.resultJSON<server.moveStore>;
            if (jobj.code == 0) {
                let that = this;
                this.getMoveStores();
                this.getFnMoveStores();
                this.toastSuccess("操作成功");
                this.show2 = false;
                (<any>this).$dialog.confirm({
                    title: '打印',
                    mes: '是否打印到【收银台】？',
                    opts: () => {
                        that.getPrintMoveStore(jobj.data.id, "收银台");
                    }
                })
            }
            else
                this.toastError(jobj.msg);
        });
    }
}