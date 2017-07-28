import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import axios from "axios";
@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class UserComponent extends Vue {
    radio1: string = "1";
    radio2: string = "1";
    show1: boolean = false;
    carNo: string = "";
    sv: string = "";
    users: work.userlist[];
    selectdepartmentname: string = '请选择部门';
    departmentshow: boolean = false;
    /** 部门字典 */
    departmentdict: { [index: number]: string; } = {};
    departmentoptions: ydui.actionSheetItem[];

    myItems1: object = [
        {
            label: '致电13907741118？',
            method: () => {
                window.location.href = 'tel: 13907741118'
            }
        }, {
            label: '详细资料'
        }
    ];

    constructor() {
        super();

        this.users = new Array();
        this.departmentoptions = new Array();

        this.getDepartments();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 员工资料');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
        if (label == '列表')
            this.getUsers();
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
                        method: () => {
                            this.selectdepartmentname = o.name;
                        }
                    });
                });
            }
        });
    }
}