import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class PlanComponent extends Vue {
    carNo: string = '';

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 经营统计');
        
    };

    change(label: string, tabkey: string) {
        console.log(label);
        this.$emit('setTitle', this.$store.state.username + ' ' + label);
    }
}