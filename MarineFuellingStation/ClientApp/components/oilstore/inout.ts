import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class InAndOutLogComponent extends ComponentBase {
    inAndOutLogs: server.inAndOutLog[];
    storeTypes: server.storeType[];
    page: number;

    constructor() {
        super();

        this.inAndOutLogs = new Array<server.inAndOutLog>();
        this.storeTypes = new Array<server.storeType>();

        this.getStoreTypes();
        this.getInAndOutLogs(server.logType.全部);
    }

    getType(t: server.logType) {
        if (t == server.logType.入仓)
            return "入仓"
        else
            return "出仓"
    }

    getSttName(st: server.store) {
        let sttName = "";
        if (st != null) {
            this.storeTypes.forEach((stt, idx) => {
                if (stt.id == st.storeTypeId) {
                    sttName = stt.name
                }
            })
        }
        return sttName;
    }

    classState(t: server.logType): any {
        switch (t) {
            case server.logType.出仓:
                return { color_red: true }
            case server.logType.入仓:
                return { color_green: true }
        }
    }
    
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 出入仓记录');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
        switch (label) {
            case "所有":
                this.getInAndOutLogs(server.logType.全部);
                break;
            case "出仓":
                this.getInAndOutLogs(server.logType.出仓);
                break;
            case "入仓":
                this.getInAndOutLogs(server.logType.入仓);
                break;
        }
    }

    formatDate(d: Date) {
        return moment(d).format("YYYY-MM-DD hh:mm")
    }

    getStoreTypes() {
        axios.get('/api/StoreType').then((res) => {
            let jobj = res.data as server.resultJSON<server.storeType[]>;
            if (jobj.code == 0) {
                this.storeTypes = jobj.data;
            }
        });
    }

    getInAndOutLogs(type: server.logType) {
        if (!this.page) this.page = 1;
        this.inAndOutLogs = null;
        axios.get("/api/InAndOutLog/GetIncludeStore?page=" + this.page + "&type=" + type).then((res) => {
            let jobj = res.data as server.resultJSON<server.inAndOutLog[]>;
            if (jobj.code == 0) {
                this.inAndOutLogs = jobj.data;
            }
        });
    }
}