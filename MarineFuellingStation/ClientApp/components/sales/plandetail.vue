<template>
    <div id="root">
        <div v-show="model.state == 0" style="background-color: lightcoral;padding: 10px; text-align: center; color: white">未审批</div>
        <div v-show="model.state == 1" style="background-color: yellowgreen;padding: 10px; text-align: center; color: white">已审批</div>
        <div v-show="model.state == 2" style="background-color: yellowgreen;padding: 10px; text-align: center; color: white">已完成</div>
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
                <span slot="left">{{model.salesPlanType == 0? "船号": "车牌号"}}：</span>
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
                <span slot="right">{{model.count}}{{model.unit}}</span>
            </yd-cell-item>

            <yd-cell-item v-show="model.isDeliver">
                <span slot="right">送货上门</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.isDeliver">
                <span slot="left">运费：</span>
                <span slot="right">{{model.deliverMoney}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.isDeliver">
                <span slot="left">打印显示单价：</span>
                <span slot="right">{{model.isPrintPrice ? "显示" : "不显示"}}</span>
            </yd-cell-item>

            <!--应客户要求暂时取消当前余油-->
            <!--<yd-cell-item>
                <span slot="left">当前余油：</span>
                <span slot="right">{{model.remainder}}</span>
                <span slot="right" style="width:70px">单位：{{model.unit}}</span>
            </yd-cell-item>-->
            <yd-cell-item>
                <span slot="left">预计加油时间：</span>
                <span slot="right">{{formatDate(model.oilDate)}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">备注：</span>
                <span slot="right">{{model.remark}}</span>
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
