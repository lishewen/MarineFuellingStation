import Vue from 'vue';
import { Component } from 'vue-property-decorator';

var VueECharts = require('vue-echarts')

Vue.component('chart', VueECharts)

@Component({
    components: {
        WeuiSearch: require('../weui-search/search.vue')
    }
})
export default class YDUIComponent extends Vue {
    sv: string = '';
    polar: any;

    constructor() {
        super();

        let data = []

        for (let i = 0; i <= 360; i++) {
            let t = i / 180 * Math.PI
            let r = Math.sin(2 * t) * Math.cos(2 * t)
            data.push([r, i])
        }

        this.polar = {
            title: {
                text: '极坐标双数值轴'
            },
            legend: {
                data: ['line']
            },
            polar: {
                center: ['50%', '54%']
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    type: 'cross'
                }
            },
            angleAxis: {
                type: 'value',
                startAngle: 0
            },
            radiusAxis: {
                min: 0
            },
            series: [
                {
                    coordinateSystem: 'polar',
                    name: 'line',
                    type: 'line',
                    showSymbol: false,
                    data: data
                }
            ],
            animationDuration: 2000
        }
    }

    handleClick(): void {
        (<any>this).$dialog.alert({ mes: 'Hello World!' + this.sv });
    }
}