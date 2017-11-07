<template>
    <div id="root">
        <div v-show="model.payState == 1" style="background-color: yellowgreen;padding: 10px; text-align: center; color: white">已结算</div>
        <div v-show="model.payState == 0" style="background-color: lightcoral;padding: 10px; text-align: center; color: white">未结算</div>
        <yd-grids-group :rows="2">
            <yd-grids-item>
                <div slot="text">
                    <p class="col-light-gray font16">计划</p>
                    <p style="margin-top: .2rem;font-size: .2rem">￥{{model.salesPlan == null ? "散客" : model.salesPlan.price}} x {{model.salesPlan == null ? "散客" : model.salesPlan.count}}{{model.unit}}</p>
                    <p>金额：￥{{model.salesPlan == null? "散客" : model.salesPlan.totalMoney}}</p>
                </div>
            </yd-grids-item>
            <yd-grids-item>
                <div slot="text">
                    <p class="col-light-gray font16">销售单</p>
                    <p style="margin-top: .2rem;font-size: .2rem">￥{{model.price}} x {{model.count}}{{model.unit}}</p>
                    <p>金额：￥{{model.totalMoney}}</p>
                </div>
            </yd-grids-item>
        </yd-grids-group>
        <yd-cell-group>
            <yd-cell-item v-show="model.salesPlan != null">
                <span slot="right" :class="totalMoneyClass()">
                    {{model.salesPlan != null? "差额：销售单金额 - 计划单金额 = ￥" + (model.totalMoney - model.salesPlan.totalMoney) : ""}}
                </span>
            </yd-cell-item>
            <yd-cell-item v-show="model.salesPlan != null">
                <div slot="left">计划单：</div>
                <div slot="right">
                    <p>{{model.salesPlan == null? "" : model.salesPlan.name}}</p>
                    <p>{{model.salesPlan == null? "" : model.salesPlan.createdBy}}</p>
                </div>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="right">{{getOrderType(model.orderType)}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">船号：</span>
                <span slot="right">{{model.carNo}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">商品：</span>
                <span slot="right">{{model.product == null? "" : model.product.name}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">备注：</span>
                <span slot="right">{{model.remark}}</span>
            </yd-cell-item>
        </yd-cell-group>
        <yd-cell-group title="代号信息" v-show="model.isInvoice">
            <yd-cell-item>
                <span slot="right">{{getIsInvoice(model.isInvoice)}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.isInvoice">
                <span slot="right">{{getTicketType(model.ticketType)}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.isInvoice">
                <span slot="left">单位：</span>
                <span slot="right">{{model.billingCompany}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.isInvoice">
                <span slot="left">单价：</span>
                <span slot="right">{{model.billingPrice}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.isInvoice">
                <span slot="left">数量：</span>
                <span slot="right">{{model.billingCount}}</span>
            </yd-cell-item>
        </yd-cell-group>
        <yd-cell-group :title="strOrderState(model)">
            <yd-cell-item>
                <span slot="left">施工人员：</span>
                <span slot="right">{{model.worker}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">时间：</span>
                <span slot="right">{{formatDate(model.lastUpdatedAt)}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.orderType == 1">
                <span slot="left">密度：</span>
                <span slot="right">{{model.density}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">实际加油：</span>
                <span slot="right">{{model.oilCount}}{{model.unit}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.orderType == 1">
                <span slot="left">与订单误差：</span>
                <span slot="right">{{strDiffOil(model)}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.orderType == 1">
                <span slot="right">注：负数为实际少于订单</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.orderType == 1">
                <div slot="left">毛重图片：</div>
                <div slot="right"><div class="img-wrap"><img :src="this.model.oilCarWeightPic" /></div></div>
            </yd-cell-item>
            <yd-cell-item v-show="model.orderType == 1">
                <div slot="left">皮重图片：</div>
                <div slot="right"><div class="img-wrap"><img :src="this.model.emptyCarWeightPic" /></div></div>
            </yd-cell-item>
        </yd-cell-group>
        <yd-cell-group title="运输">
            <yd-cell-item>
                <span slot="left">运输单：</span>
                <span slot="right"></span>
            </yd-cell-item>
        </yd-cell-group>
        <yd-cell-group title="付款方式与金额">
            <yd-cell-item v-for="p in model.payments" :key="p.id">
                <span slot="left">{{strPayType(p)}}：</span>
                <span slot="right">￥{{p.money}}</span>
            </yd-cell-item>
        </yd-cell-group>

    </div>
</template>

<script src="./orderdetail.ts" />
<style src="./../website.css" />