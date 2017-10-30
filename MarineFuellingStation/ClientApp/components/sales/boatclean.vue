<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="开单">
                <yd-cell-group :title="'单号：' + model.name" style="margin-top: 20px">

                    <yd-cell-item>
                        <span slot="left">船号：</span>
                        <yd-input slot="right" v-model="model.carNo" required placeholder="请输入您的船号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">金额：</span>
                        <yd-input slot="right" v-model="model.money" required placeholder="请输入金额"></yd-input>
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
                        <span slot="right">
                            <yd-switch v-model="model.isInvoice"></yd-switch>
                        </span>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">单位：</span>
                        <yd-input slot="right" v-model="model.billingCompany" regex="" placeholder="请输入单位"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="model.billingPrice" regex="" placeholder="请输入单价，默认同上"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="model.billingCount" regex="" placeholder="请输入，默认同上"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary" @click.native="buttonclick" :disabled="isPrevent">提交</yd-button>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <yd-search v-model="sv" />
                <yd-cell-group>
                    <yd-cell-item arrow v-for="s in list" :key="s.id">
                        <span slot="left">{{s.carNo}}</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">{{s.name}}</span>
                        <span slot="right">{{formatDate(s.oilDate)}}</span>
                        <span slot="right" :class="classState(s.state)" style="padding-left:10px">{{getStateName(s.state)}}</span>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<style src="./plan.css" />
<script src="./boatclean.ts"/>