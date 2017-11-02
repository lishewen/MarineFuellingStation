<template>
    <div id="root">
        <div v-show="model.payState == 1" style="background-color: yellowgreen;padding: 10px; text-align: center; color: white">已结算</div>
        <yd-cell-group>
            <yd-cell-item v-show="model.salesPlan != null">
                <span slot="left">计划单：</span>
                <span slot="right">{{model.salesPlan == null? "" : model.salesPlan.name}}</span>
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
                <span slot="right">{{model.count}}{{model.unit}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">总价：</span>
                <span slot="right">{{model.totalMoney}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="right">{{getIsInvoice(model.isInvoice)}}</span>
            </yd-cell-item>
            <yd-cell-item arrow v-show="model.isInvoice">
                <span slot="right">{{getTicketType(model.ticketType)}}</span>
                <select slot="right" v-model="model.ticketType">
                    <option value="-1">请选择类型</option>
                    <option value="0">循</option>
                    <option value="1">柴</option>
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
            <yd-cell-item>
                <span slot="left">备注：</span>
                <span slot="right">{{model.remark}}</span>
            </yd-cell-item>
        </yd-cell-group>
        <yd-cell-group :title="strOrderState(model)">
            <yd-cell-item>
                <span slot="left">施工人员：</span>
                <span slot="right">{{model.lastUpdatedBy}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">密度：</span>
                <span slot="right">{{model.density}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">实际加油：</span>
                <span slot="right">{{model.oilCount}}{{model.unit}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">与订单误差：</span>
                <span slot="right">{{strDiffOil(model)}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="right">注：负数为实际少于订单</span>
            </yd-cell-item>
            <yd-cell-item>
                <div slot="left">毛重图片：</div>
                <div slot="right"><div class="img-wrap"><img :src="this.model.oilCarWeightPic" /></div></div>
            </yd-cell-item>
            <yd-cell-item>
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
