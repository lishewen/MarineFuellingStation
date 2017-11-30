import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class InAndOutLogComponent extends ComponentBase {
    inAndOutLogs: server.inAndOutLog[];
    inLogs: server.inAndOutLog[];
    outLogs: server.inAndOutLog[];
    storeTypes: server.storeType[];
    logType: server.logType;
    page: number;//第N页
    pSize: number = 10;//分页中每页显示的记录数
    scrollRef: any;

    constructor() {
        super();

        this.inAndOutLogs = new Array<server.inAndOutLog>();
        this.inLogs = new Array<server.inAndOutLog>();
        this.outLogs = new Array<server.inAndOutLog>();
        this.storeTypes = new Array<server.storeType>();

        this.getStoreTypes();
        this.getInAndOutLogs();
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
                this.logType = server.logType.全部;
                break;
            case "出仓":
                this.logType = server.logType.出仓;
                break;
            case "入仓":
                this.logType = server.logType.入仓;
                break;
        }
        (<any>this).$refs.infinitescroll.$emit('ydui.infinitescroll.reInit');
        (<any>this).$refs.infinitescroll1.$emit('ydui.infinitescroll.reInit');
        (<any>this).$refs.infinitescroll2.$emit('ydui.infinitescroll.reInit');
        this.inAndOutLogs = null; this.outLogs = null; this.inLogs = null;
        this.page = 1;
        this.getInAndOutLogs();
    }
    
    loadList() {
        this.getInAndOutLogs((list: server.inAndOutLog[]) => {
            switch (this.logType) {
                case server.logType.全部:
                    this.inAndOutLogs = this.page > 1 ? [...this.inAndOutLogs, ...list] : this.inAndOutLogs;
                    this.scrollRef = (<any>this).$refs.infinitescroll;
                    break;
                case server.logType.出仓:
                    this.outLogs = this.page > 1 ? [...this.outLogs, ...list] : this.outLogs;
                    this.scrollRef = (<any>this).$refs.infinitescroll1;
                    break;
                case server.logType.入仓:
                    this.inLogs = this.page > 1 ? [...this.inLogs, ...list] : this.inLogs;
                    this.scrollRef = (<any>this).$refs.infinitescroll2;
                    break;
            }
            if (list.length < this.pSize) {
                this.scrollRef.$emit("ydui.infinitescroll.loadedDone");
                return;
            }

            //通知加载数据完毕
            this.scrollRef.$emit("ydui.infinitescroll.finishLoad");
            
            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
            console.log("page = " + this.page)
        });
    }

    getInAndOutLogs(callback?: Function) {
        if (this.page == null) this.page = 1;
        axios.get("/api/InAndOutLog/GetIncludeStore?"
            + "page=" + this.page
            + "&type=" + this.logType
            ).then((res) => {
            let jobj = res.data as server.resultJSON<server.inAndOutLog[]>;
            if (jobj.code == 0) {
                if (callback){
                    callback(jobj.data);
                }
                else {
                    switch (this.logType) {
                        case server.logType.全部:
                            this.inAndOutLogs = jobj.data;
                            break;
                        case server.logType.出仓:
                            this.outLogs = jobj.data;
                            break;
                        case server.logType.入仓:
                            this.inLogs = jobj.data;
                            break;
                    }
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