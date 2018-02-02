import ComponentBase from "../../ComponentBase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class ClientToCompanyComponent extends ComponentBase {
    model: server.client;
    toCompanyName: string;
    clientId: string;
    companyId: string;

    constructor() {
        super();

        this.model = (new Object()) as server.client;
        this.model.product = new Object() as server.product;
        this.model.company = new Object() as server.company;
    }

    mounted() {
        this.clientId = this.$route.params.cid;
        this.companyId = this.$route.params.coid;
        this.toCompanyName = this.$route.params.coname;
        this.getClient(this.clientId, () => {
            //设置返回键的连接
            this.$emit('setTitle', this.model.carNo + '明细', '/sales/myclient');
        });
    }

    getClient(id: string, callback: Function) {
        axios.get('/api/Client/GetDetail/' + id).then((res) => {
            let jobj = res.data as server.resultJSON<server.client>;
            if (jobj.code == 0) {
                this.model = jobj.data;
                callback();
            }
        });
    }
    putAddClientToCompany() {
        axios.put('/api/Client/SetClientsToCompany'
            + '?clientIds=' + this.clientId
            + '&companyId=' + this.companyId
            , null).then(res => {
                let jobj = res.data as server.resultJSON<server.client[]>;
                if (jobj.code == 0)
                    this.toastSuccess("操作成功")
                else
                    this.toastError(jobj.msg);
            })
    }
}