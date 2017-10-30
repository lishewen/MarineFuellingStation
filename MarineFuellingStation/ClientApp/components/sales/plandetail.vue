<template>
    <div id="root">
        <yd-cell-group>
            <yd-cell-item>
                <span slot="left">单号</span>
                <span slot="right">{{model.name}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">类型</span>
                <span slot="right">{{getSpType(model.salesPlanType)}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">船号：</span>
                <span slot="right">{{model.carNo}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油品：</span>
                <span slot="right">{{model.oilName}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">计划单价：</span>
                <span slot="right">{{model.price}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">计划数量：</span>
                <span slot="right">{{model.count}}</span>
                <span slot="right" style="width:70px">单位：{{model.unit}}</span>
            </yd-cell-item>
            <!--应客户要求暂时取消当前余油-->
            <!--<yd-cell-item>
                <span slot="left">当前余油：</span>
                <span slot="right">{{model.remainder}}</span>
                <span slot="right" style="width:70px">单位：{{model.unit}}</span>
            </yd-cell-item>-->
            <yd-cell-item>
                <span slot="left">加油时间：</span>
                <span slot="right">{{formatDate(model.oilDate)}}</span>
            </yd-cell-item>
        </yd-cell-group>

        <yd-cell-group title="代号信息" v-show="model.isInvoice">
            <yd-cell-item>
                <span slot="left">单位：</span>
                <span slot="right">{{model.billingCompany}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">单价：</span>
                <span slot="right">{{model.billingPrice}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">数量：</span>
                <span slot="right">{{model.billingCount}}</span>
            </yd-cell-item>
        </yd-cell-group>
    </div>
</template>

<script src="./plandetail.ts" />
