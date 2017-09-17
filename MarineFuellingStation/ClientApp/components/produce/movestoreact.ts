import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class MoveStoreActComponent extends ComponentBase {
    carNo: string = "";
    show2: boolean = false;
    actBtnId: number; actBtnId1: number;

    constructor() {
        super();
        
    }
    
    saveclick2(): void {
        this.show2 = false;
    }
   
    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 水上装油');
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
    
}