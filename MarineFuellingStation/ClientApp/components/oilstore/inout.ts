import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class InAndOutLogComponent extends ComponentBase {
    inAndOutLogs: server.inAndOutLog[];
    storeTypes: server.storeType[];
    logType: server.logType;
    page: number;

    constructor() {
        super();

        this.inAndOutLogs = new Array<server.inAndOutLog>();
        this.storeTypes = new Array<server.storeType>();

        this.getStoreTypes();
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

        this.page = 1;
        switch (label) {
            case "所有":
                this.logType = server.logType.全部;
                break;
            case "出仓":
                this.logType = server.logType.出仓;
                break;
            case "入仓":
                this.logType = server.logType.入仓;
                break;
        }
        this.inAndOutLogs = null;
        this.getInAndOutLogs();
    }

    formatDate(d: Date) {
        return moment(d).format("YYYY-MM-DD hh:mm")
    }

    loadList() {
        this.getInAndOutLogs((list: server.inAndOutLog[]) => {
            if (this.page > 1){
                //叠加新内容进inAndOutLogs
                this.inAndOutLogs = [...this.inAndOutLogs, ...list];
            }
            else
                this.inAndOutLogs = list;

            if (list.length < 30){
            //通知控件刷新完成
                (<any>this.$refs.infinitescroll).$emit('ydui.infinitescroll.loadedDone');
                return;
            }
            /* 单次请求数据完毕 */
            this.$refs.infinitescrollDemo.$emit('ydui.infinitescroll.finishLoad');

            //如果有内容则page+1，否则则把page重置为1
            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
        });
    }

    getInAndOutLogs(callback?: Function) {
        if (!this.page) this.page = 1;
        if (this.logType == null) this.logType = server.logType.全部
        axios.get("/api/InAndOutLog/GetIncludeStore?page=" + this.page + "&type=" + this.logType).then((res) => {
            let jobj = res.data as server.resultJSON<server.inAndOutLog[]>;
            if (jobj.code == 0) {
                if (callback){
                    callback(jobj.data);
                }
                else {
                    this.inAndOutLogs = jobj.data;
                    this.page++;
                }
            }
        });
    }

    getStoreTypes() {
        axios.get('/api/StoreType').then((res) => {
            let jobj = res.data as server.resultJSON<server.storeType[]>;
            if (jobj.code == 0) {
                this.storeTypes = jobj.data;
            }
        });
    }
    
}