<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="开单">
                <yd-cell-group :title="'单号：' + model.name" style="margin-top: 20px">

                    <yd-cell-item>
                        <span slot="left">船号：</span>
                        <yd-input slot="right" v-model="model.carNo" regex="" placeholder="请输入您的船号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">金额：</span>
                        <yd-input slot="right" v-model="model.money" regex="" placeholder="请输入金额"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="选填">

                    <yd-cell-item>
                        <span slot="left">航次：</span>
                        <yd-input slot="right" v-model="model.voyage" regex="" placeholder="请输入航次"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">船舶总吨：</span>
                        <yd-input slot="right" v-model="model.tonnage" regex="" placeholder="请输入船舶总吨"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">批准书文号：</span>
                        <yd-input slot="right" v-model="model.responseId" regex="" placeholder="请输入批准书文号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">作业地点：</span>
                        <yd-input slot="right" v-model="model.address" regex="" placeholder="请输入作业地点"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">作业单位：</span>
                        <yd-input slot="right" v-model="model.company" regex="" placeholder="请输入批准书文号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">联系电话：</span>
                        <yd-input slot="right" v-model="model.phone" regex="" placeholder="请输入联系电话"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">是否开票</span>
                        <span slot="right">
                            <yd-switch v-model="model.isinvoice"></yd-switch>
                        </span>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">开票单位：</span>
                        <yd-input slot="right" v-model="model.billingCompany" regex="" placeholder="请输入开票单位"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="model.billingPrice" regex="" placeholder="请输入开票单价，默认同上"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="model.billingCount" regex="" placeholder="请输入开票，默认同上"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary" @click.native="buttonclick">提交</yd-button>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <weui-search v-model="sv" />
                <yd-cell-group>
                    <yd-cell-item arrow>
                        <span slot="left">船0001</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">QW07070001</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:red; padding-left:10px">施工中</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">QW07070001</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:green; padding-left:10px">已开单</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">QW07070001</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:blue; padding-left:10px">已完成</span>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<style src="./plan.css" />
<script src="./boatclean.ts"/>