import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class CheckinComponent extends ComponentBase {
    checkinData: work.checkinData[];
    page: number;//第N页
    pSize: number = 10;//分页中每页显示的记录数
    scrollRef: any;

    constructor() {
        super();
        this.checkinData = new Array<work.checkinData>();
        this.getCheckindata(0);
    }
    
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 考勤记录');
    };

    getCheckindata(id: number) {
        axios.get('/api/oa/GetUserCheckinData').then((res) => {
            let jobj = res.data as work.checkinDataResult;
            if (jobj.errcode == 0) {
                this.checkinData = jobj.checkindata;
            }
        });
    }
}