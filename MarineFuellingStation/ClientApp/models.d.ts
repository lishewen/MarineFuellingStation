declare namespace server {
    export class resultJSON<T>{
        code: number;
        msg: string;
        data: T
    }
}

declare namespace ydui {
    export class actionSheetItem {
        label: string;
        method: Function;
    }
}

declare namespace work {
    export interface department {
        id: number;
        name: string;
        parentid: number;
        order: number;
    }

    export interface departmentListResult {
        errcode: number;
        errmsg: string;
        department: department[];
    }
}