import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class MoveStoreActComponent extends ComponentBase {
    carNo: string = "";
    show2: boolean = false;
    movestores: server.moveStoreGET[];
    fnmovestores: server.moveStoreGET[];

    constructor() {
        super();

        this.movestores = new Array<server.moveStoreGET>();
        this.fnmovestores = new Array<server.moveStoreGET>();

        this.getMoveStores();
        this.getFnMoveStores();
    }

    actConfrim(): void {
        (<any>this).$dialog.confirm({
            title: '确认操作',
            mes: '开始施工？',
            opts: () => {
                (<any>this).$dialog.toast({ mes: '确认', timeout: 1000 });
            }
        })
    };
    saveclick2(): void {
        this.show2 = false;
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 生产施工');
    };

    formatDate(d: Date): string {
        return moment(d).format('MM-DD hh:mm');
    }

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
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
}