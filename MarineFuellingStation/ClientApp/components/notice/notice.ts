import ComponentBase from "../../componentbase";
import axios from "axios";
import moment from "moment";
import { Component } from 'vue-property-decorator';

@Component
export default class NoticeComponent extends ComponentBase {
    showAdd: boolean = false;
    isPrevent: boolean = true;
    notices: server.notice[];
    notice: server.notice;
    actItems: ydui.actionSheetItem[];
    
    filterCType: Array<helper.filterBtn>;
    state: number;

    actBtnId: number; //当前激活状态的条件button

    page: number;
    scrollRef: any;
    pSize: number = 30;

    toApps: Array<string>;

    constructor() {
        super();

        this.notice = new Object as server.notice;
        this.notices = new Array<server.notice>();
        this.toApps = new Array<string>();

        this.notice.name = '';

        this.filterCType = [
            { id: 0, name: '启用', value: 1, actived: true },
            { id: 1, name: '未启用', value: 0, actived: false }
        ];

        this.actBtnId = 0;
        this.actItems = new Array();

        this.state = 1;
        this.getNotices();
    }

    switchBtn(o: helper.filterBtn, idx: number) {
        o.actived = true;
        if (idx != this.actBtnId) {
            this.filterCType[this.actBtnId].actived = false;
            this.actBtnId = idx;

            this.state = o.value as number;
            this.page = 1;
            this.getNotices();
        }
    }

    mounted() {
        this.$emit('setTitle', '通知');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }

    changeIsUse(model: server.notice) {
        axios.put('/api/notice/ChangeIsUse', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.notice>;
            if (jobj.code == 0) {
                this.toastSuccess("操作成功")
                this.getNotices();
            }
        })
    }

    godetail(id: number) {
        this.$router.push('/sales/notice/' + id + '/auditing')
    }

    loadList() {
        this.getNotices((list: server.notice[]) => {
            this.notices = this.page > 1 ? [...this.notices, ...list] : this.notices;
            this.scrollRef = (<any>this).$refs.infinitescroll;
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

    //获得计划列表
    getNotices(callback?: Function) {
        if (this.page == null) this.page = 1;
        if (this.pSize == null) this.pSize = 30;
        if (this.state == null) this.state = 1;
        let isUse = this.state == 0 ? "false" : "true";
        axios.get('/api/notice/GetByIsUse?'
            + 'page=' + this.page
            + '&pageSize=' + this.pSize
            + '&isUse=' + isUse).then((res) => {
                let jobj = res.data as server.resultJSON<server.notice[]>;
                if (jobj.code == 0) {
                    if (callback) {
                        callback(jobj.data);
                    }
                    else {
                        this.notices = jobj.data;
                        console.log(this.notices);
                        this.page++;
                    }
                }
            });
    }

    postNotice() {
        if (this.toApps.length > 0)
            this.notice.toApps = this.toApps.length > 1 ? this.toApps.join('|') : this.toApps[0];
        else {
            this.toastError("请选择获取通知的应用");
            return;
        }
        if (this.notice.content == '') { this.toastError("请输入内容"); return; }
        axios.post('/api/notice', this.notice).then((res) => {
            let jobj = res.data as server.resultJSON<server.notice>;
            if (jobj.code == 0) {
                this.notice = jobj.data;
                this.toastSuccess('添加成功')
                this.page = 1;
                this.getNotices();
            }
        });
    }
}