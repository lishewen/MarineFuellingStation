import ComponentBase from "../../componentbase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class AccountComponent extends ComponentBase {
    
    constructor() {
        super();
        
    }
    mounted() {
        this.$emit('setTitle', '账户');
    }
    go(app: string) {
        this.$router.push(app);
    }
}