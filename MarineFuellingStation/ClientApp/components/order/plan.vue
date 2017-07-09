<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="计划开单">
                <yd-cell-group title="必填" style="padding-top: 20px">
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

                    <yd-cell-item>
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入加油数量"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入加油数量"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">加油时间：</span>
                        <input slot="right" class="cell-input" type="date" value="2017-08-19" placeholder="">
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●申请时间：</span>
                        <input slot="right" class="cell-input" type="date" value="2017-07-19" placeholder="">
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●总价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="单价 x 数量"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-cell-group title="选填">
                    <yd-cell-item>
                        <span slot="left">是否开票</span>
                        <span slot="right">
                            <yd-switch v-model="switch1"></yd-switch>
                        </span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">开票单位：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入开票单位"></yd-input>
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
                        <span slot="right" style="color:red; padding-left:10px">未审批</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:green; padding-left:10px">已审批</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:blue; padding-left:10px">已完成</span>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<style src="./plan.css" />
<script src="./plan.ts"/>