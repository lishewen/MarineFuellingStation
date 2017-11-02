import Vue from 'vue';
import moment from "moment";

export default class ComponentBase extends Vue {
    formatDate(d: Date, f: string = 'YYYY-MM-DD HH:mm'): string {
        return moment(d).format(f);
    }
    toastError(msg: string) {
        this.$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'error'
        });
    }
    toastSuccess(msg: string) {
        this.$dialog.toast({
            mes: msg,
            timeout: 1500,
            icon: 'success'
        });
    }
}