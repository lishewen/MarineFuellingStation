import ComponentBase from "../../ComponentBase";
import { Component } from 'vue-property-decorator';
import axios from "axios";

@Component
export default class LoadOilComponent extends ComponentBase {
    
    constructor() {
        super();
        
    }
    mounted() {
        this.$emit('setTitle', '加油');
    }
    go(app: string) {
        this.$router.push('/produce/' + app);
    }
}