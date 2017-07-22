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