import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class UserDetailComponent extends ComponentBase {
    model: server.userDTO;
    /** 部门字典 */
    departmentdict: { [index: number]: string; } = {};

    constructor() {
        super();

        this.model = new Object as server.userDTO;
        this.model.workInfo = new Object as work.memberResult;
        this.model.workInfo.gender = 1;

        this.getDepartments();
    }

    buttonclick() {
        this.postUser(this.model);
    }

    mounted() {
        let id = this.$route.params.id;
        this.getUser(id, () => {
            //设置返回键的连接
            this.$emit('setTitle', this.model.workInfo.name + ' 员工资料', '/user/user');
        });
    };

    getUser(id: string, callback: Function) {
        axios.get('/api/User/' + id).then((res) => {
            let jobj = res.data as server.resultJSON<server.userDTO>;
            if (jobj.code == 0) {
                this.model = jobj.data;
                callback();
            }
        });
    }

    postUser(model: server.userDTO) {
        axios.post('/api/User', model).then((res) => {
            let jobj = res.data as server.resultJSON<server.userDTO>;
            if (jobj.code == 0) {
                this.model = jobj.data;
                this.toastSuccess(jobj.msg);
            }
        });
    }

    getDepartments() {
        axios.get('/api/Department').then((res) => {
            let jobj = res.data as work.departmentListResult;
            if (jobj.errcode == 0) {
                jobj.department.forEach((o, i) => {
                    this.departmentdict[o.id] = o.name;
                });
            }
        });
    }
}