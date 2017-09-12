import Vue from 'vue';
import moment from "moment";

export default class ComponentBase extends Vue {
    formatDate(d: Date): string {
        return moment(d).format('YYYY-MM-DD');
    }
    toastError(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }
    toastSuccess(msg: string) {
        (<any>this).$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'success'
        });
    }
}