import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class UserComponent extends ComponentBase {
    radio1: string = "1";
    radio2: string = "1";
    usershow: boolean = false;
    carNo: string = "";
    users: work.userlist[];
    selectdepartmentname: string = '请选择部门';
    departmentshow: boolean = false;
    /** 部门字典 */
    departmentdict: { [index: number]: string; } = {};
    departmentoptions: ydui.actionSheetItem[];
    userItems: ydui.actionSheetItem[];

    constructor() {
        super();

        this.users = new Array();
        this.departmentoptions = new Array();
        this.userItems = new Array();

        this.getDepartments();
        this.getUsers();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 员工资料');
    };

    //change(label: string, tabkey: string) {
    //    console.log(label);
    //    this.$emit('setTitle', this.$store.state.username + ' ' + label);
    //    if (label == '列表')
    //        this.getUsers();
    //}

    userClick(user: work.userlist) {
        this.userItems = [{
            label: '致电' + user.mobile + '？',
            callback: () => {
                window.location.href = 'tel: ' + user.mobile;
            }
        }, {
            label: '详细资料',
            callback: () => {
                this.$router.push('/user/user/' + user.userid)
            }
        }];
        this.usershow = true;
    }

    getUsers() {
        axios.get('/api/User').then((res) => {
            let jobj = res.data as work.departmentMemberInfoResult;
            if (jobj.errcode == 0)
                this.users = jobj.userlist;
        });
    }

    getDepartments() {
        axios.get('/api/Department').then((res) => {
            let jobj = res.data as work.departmentListResult;
            if (jobj.errcode == 0) {
                jobj.department.forEach((o, i) => {
                    this.departmentdict[o.id] = o.name;
                    this.departmentoptions.push({
                        label: o.name,
                        callback: () => {
                            this.selectdepartmentname = o.name;
                        }
                    });
                });
            }
        });
    }
}