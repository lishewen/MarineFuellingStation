import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class PlanComponent extends Vue {
    radio2: string = '1';

    mounted() {
        this.$emit('setTitle', '计划开单');
    }
}