import Vue from 'vue';

export default {
    data() {
        return {
            progress1: 0.1,
            progress2: 0.4
        }
    },
    mounted() {
        this.$emit('setTitle', '油库状态');
    }
}