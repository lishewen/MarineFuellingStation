<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="销售开单">
                <yd-cell-group title="请选择" style="padding-top: 20px">
                    <yd-cell-item arrow @click.native="show4 = true">
                        <span slot="left">计划单：</span>
                        <span slot="right">{{selectedplanNo}}</span>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-popup v-model="show4" position="right">
                    <yd-cell-group>
                        <yd-cell-item arrow @click.native="planitemclick()">
                            <span slot="left">船0001</span>
                            <span slot="left" style="color:lightgray;margin-left:10px">李四</span>
                            <span slot="right">07-07</span>
                        </yd-cell-item>
                        <yd-cell-item arrow @click.native="planitemclick()">
                            <span slot="left">船0002</span>
                            <span slot="left" style="color:lightgray;margin-left:10px">张三</span>
                            <span slot="right">07-07</span>
                        </yd-cell-item>
                        <yd-cell-item arrow @click.native="planitemclick()">
                            <span slot="left">船0002</span>
                            <span slot="left" style="color:lightgray;margin-left:10px">王五</span>
                            <span slot="right">07-07</span>
                        </yd-cell-item>
                    </yd-cell-group>
                    <div style="text-align: center">
                        <yd-button style="width:100px" type="primary" @click.native="emptyclick()">没有计划</yd-button>
                    </div>
                </yd-popup>

                <yd-cell-group title="请选择">
                    <yd-cell-item>
                        <yd-radio-group slot="left" v-model="radio2">
                            <yd-radio val="1">水上</yd-radio>
                            <yd-radio val="2">陆上</yd-radio>
                            <yd-radio val="3">机油</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="单号：XS2017070700001">
                    <yd-cell-item>
                        <span slot="left">船号：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入您的船号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item arrow>
                        <span slot="left">油品：</span>
                        <select slot="right">
                            <option value="">请选择油品</option>
                            <option value="1">93#</option>
                            <option value="2">95#</option>
                            <option value="3">97#</option>
                        </select>
                    </yd-cell-item>

                    <yd-cell-item v-show="hasplan">
                        <span slot="left">计划单价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入单价"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">订单单价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入单价"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item v-show="hasplan">
                        <span slot="left">计划数量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入加油数量"></yd-input>
                        <span slot="right" style="width:70px">单位：{{unit}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">订单数量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入加油数量"></yd-input>
                        <span slot="right" style="width:70px">单位：{{unit}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">总价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="单价 x 数量"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">销售员：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="默认开单员"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-cell-group title="选填">
                    <yd-cell-item>
                        <span slot="left">是否开票</span>
                        <span slot="right">
                            <yd-switch v-model="isinvoice"></yd-switch>
                        </span>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">开票单位：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入开票单位"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入开票单价，默认同上"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入开票，默认同上"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="不显示">
                    <yd-cell-item>
                        <span slot="left">●实际数量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入"></yd-input>
                        <span slot="right" style="width:70px">单位：{{unit}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●开单时间：</span>
                        <input slot="right" class="cell-input" type="date" value="2017-08-19" placeholder="">
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●开单员：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="开单员"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●生产员：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="多个，生产流程时录入"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●开始装油时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●结束装油时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●表1读数（升）：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●表2读数（升）：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●表3读数（升）：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●密度：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●油温（度）：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●实际与订单数量差值：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●空车过磅值（皮重）：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入，陆上装油才需要"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●油车过磅值（毛重）：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入，陆上装油才需要"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●过磅差值（油重）：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="生产流程时录入，陆上装油才需要"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●销售提成：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="结算后，按公式自动计算"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary">提交</yd-button>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <yd-cell-group>
                    <yd-cell-item arrow>
                        <span slot="left">船0001</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:red; padding-left:10px">&#12288;已完成</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:green; padding-left:10px">&#12288;装油中</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:darkorange; padding-left:10px">装油结束</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:blue; padding-left:10px">&#12288;已开单</span>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<style src="./plan.css" />
<script src="./order.ts"/>