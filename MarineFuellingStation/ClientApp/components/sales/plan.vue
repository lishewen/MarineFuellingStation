<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="计划开单">

                <yd-cell-group title="请选择" style="padding-top: 20px">
                    <yd-cell-item>
                        <yd-radio-group slot="left" v-model="radio2">
                            <yd-radio val="1">水上</yd-radio>
                            <yd-radio val="2">陆上</yd-radio>
                            <yd-radio val="3">机油</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-cell-group :title="'单号：' + model.name">
                    <yd-cell-item>
                        <span slot="left">船号：</span>
                        <yd-input slot="right" v-model="model.carNo" required placeholder="请输入您的船号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item arrow>
                        <span slot="left">油品：</span>
                        <select slot="right" v-model="model.productId" @change="changeProduct">
                            <option value="">请选择油品</option>
                            <option v-for="option in options" :value="option.id">{{option.name}}</option>
                        </select>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">计划单价：</span>
                        <yd-input slot="right" v-model="model.price" regex="" placeholder="请输入单价"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">计划数量：</span>
                        <yd-input slot="right" v-model="model.count" regex="" placeholder="请输入加油数量"></yd-input>
                        <span slot="right" style="width:70px">单位：{{model.unit}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">当前余油：</span>
                        <yd-input slot="right" v-model="model.remainder" regex="" placeholder="请输入客户目前剩余油量"></yd-input>
                        <span slot="right" style="width:70px">单位：{{model.unit}}</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">加油时间：</span>
                        <yd-datetime type="date" v-model="oildate" slot="right"></yd-datetime>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="选填">
                    <yd-cell-item>
                        <span slot="left">是否开票</span>
                        <span slot="right">
                            <yd-switch v-model="model.isInvoice"></yd-switch>
                        </span>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">开票单位：</span>
                        <yd-input slot="right" v-model="model.billingCompany" regex="" placeholder="请输入开票单位"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="model.billingPrice" regex="" placeholder="请输入开票单价，默认同上"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="model.billingCount" regex="" placeholder="请输入开票，默认同上"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary" @click.native="buttonclick">提交</yd-button>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <yd-cell-group>
                    <yd-cell-item arrow v-for="s in salesplans">
                        <span slot="left">{{s.carNo}}</span>
                        <span slot="left" class="color_lightgray" style="margin-left:10px">{{s.oilName}} {{s.count}}{{s.unit}} ￥{{s.totalMoney}}</span>
                        <span slot="right">{{formatDate(s.oilDate)}}</span>
                        <span slot="right" :class="classState(s.state)" style="padding-left:10px">{{getStateName(s.state)}}</span>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<style src="./plan.css" />
<script src="./plan.ts"/>