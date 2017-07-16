declare namespace server {
    export class resultJSON<T>{
        code: number;
        msg: string;
        data: T
    }
}