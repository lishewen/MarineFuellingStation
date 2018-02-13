import ComponentBase from "../../ComponentBase";
import { Component } from 'vue-property-decorator';
import axios from "axios";
import moment from "moment";

@Component
export default class CheckinComponent extends ComponentBase {
    showSelectTime: boolean = false;
    startDate: string;
    endDate: string;
    type: excel.dataType;

    constructor() {
        super();

        this.startDate = this.formatDate(new Date());
        this.endDate = this.startDate;
        this.type = excel.dataType.未指定;
    }
    
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 导出excel');
    };

    exportClientsclick() {
        this.type = excel.dataType.客户;
        this.showSelectTime = true;
    }

    submitclick() {
        switch (this.type) {
            case excel.dataType.客户:
                this.getExportClients();
                break;
        }
    }

    isPreventSubmmit() {
        let start = moment(this.startDate);
        let end = moment(this.endDate);
        return end.dayOfYear() < start.dayOfYear();
    }

    getExportClients() {
        axios.get('/api/Client/ExportExcel'
            + "?start=" + this.startDate + " 00:00"
            + "&end=" + this.endDate + " 23:59"
        ).then((res) => {
            let jobj = res.data as server.resultJSON<string>;
            if (jobj.code == 0)
                console.log(jobj.data)
            else
                this.toastError(jobj.msg)
        });
    }
}