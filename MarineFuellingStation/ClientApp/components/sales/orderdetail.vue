<template>
    <div id="root">
        <yd-cell-group>
            <yd-cell-item v-show="model.salesPlan != null">
                <span slot="left">计划单：</span>
                <span slot="right">{{model.salesPlan == null? "" : model.salesPlan.name}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">类型：</span>
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

            <yd-cell-item v-show="model.salesPlan != null">
                <span slot="left">计划单价：</span>
                <span slot="right">{{model.salesPlan == null ? "" : model.salesPlan.price}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">订单单价：</span>
                <span slot="right">￥{{model.price}}</span>
            </yd-cell-item>

            <yd-cell-item v-show="model.salesPlan != null">
                <span slot="left">计划数量：</span>
                <span slot="right">{{model.salesPlan == null ? "" : model.salesPlan.count}}</span>
                <span slot="right" style="width:70px">单位：{{model.unit}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">订单数量：</span>
                <span slot="right">{{model.count}}</span>
                <span slot="right" style="width:70px">单位：{{model.unit}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">总价：</span>
                <span slot="right">{{model.totalMoney}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">是否开票</span>
                <span slot="right">{{getIsInvoice(model.isInvoice)}}</span>
            </yd-cell-item>
            <yd-cell-item arrow v-show="model.isInvoice">
                <span slot="left">票类：</span>
                <span slot="right">{{getTicketType(model.ticketType)}}</span>
                <select slot="right" v-model="model.ticketType">
                    <option value="-1">请选择票类</option>
                    <option value="0">循票</option>
                    <option value="1">柴票</option>
                </select>
            </yd-cell-item>
            <yd-cell-item v-show="model.isInvoice">
                <span slot="left">开票单位：</span>
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
        <yd-cell-group title="选填">
            <yd-cell-item>
                <span slot="left">运输单：</span>
                <span slot="right"></span>
            </yd-cell-item>
        </yd-cell-group>
    </div>
</template>

<script src="./orderdetail.ts" />
