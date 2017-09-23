<template>
    <div id="root">
        <yd-cell-group>
            <yd-cell-item>
                <span slot="left">油品：</span>
                <span slot="right">{{model.product == null? "" : model.product.name}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">单价：</span>
                <span slot="right">{{model.price}}</span>
                <span slot="right" style="width: 50px">元 / 吨</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">数量：</span>
                <span slot="right">{{model.count}}</span>
                <span slot="right">吨</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">始发地：</span>
                <span slot="right">{{model.origin}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">出发时间：</span>
                <span slot="right">{{formatDate(model.startTime)}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">预计到达：</span>
                <span slot="right">{{formatDate(model.arrivalTime)}}</span>
            </yd-cell-item>

        </yd-cell-group>

        <yd-cell-group title="运输">
            <yd-cell-item>
                <span slot="left">车牌号一：</span>
                <span slot="right">{{model.carNo}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">挂车号：</span>
                <span slot="right">{{model.trailerNo}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">司机一：</span>
                <span slot="right">{{model.driver1}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">身份证一：</span>
                <span slot="right">{{model.idCard1}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">电话号码一：</span>
                <span slot="right">{{model.phone1}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">司机二：</span>
                <span slot="right">{{model.driver2}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">身份证二：</span>
                <span slot="right">{{model.idCard2}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">电话号码二：</span>
                <span slot="right">{{model.phone2}}</span>
            </yd-cell-item>
        </yd-cell-group>
    </div>
</template>

<script src="./purchasedetail.ts" />
