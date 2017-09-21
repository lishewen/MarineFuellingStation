<template>
    <div id="root">
        <yd-cell-group title="必填" style="margin-top:20px">
            <yd-cell-item>
                <span slot="left">所属公司：</span>
                <span slot="right">{{model.company.name}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">船号/车号</span>
                <span slot="right">{{model.carNo}}</span>
                </yd-cell-item>

            <yd-cell-item>
                <span slot="left">跟进销售：</span>
                <span slot="right">{{model.followSalesman}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">默认商品：</span>
                <span slot="right">{{model.product.name}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">联系人：</span>
                <span slot="right">{{model.contact}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">联系电话：</span>
                <span slot="right">{{model.mobile}}</span>
            </yd-cell-item>

        </yd-cell-group>
        <yd-cell-group title="选填" style="margin-top:10px">

            <yd-cell-item>
                <span slot="left">身份证号：</span>
                <span slot="right">{{model.idCard}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">地址：</span>
                <span slot="right">{{model.address}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">固定电话：</span>
                <span slot="right">{{model.phone}}</span>
            </yd-cell-item>

        </yd-cell-group>

        <yd-cell-group title="设置（选填）" style="margin-top:10px">

            <yd-cell-item>
                <span slot="left">最高挂账金额：</span>
                <span slot="right">{{model.maxOnAccount}}</span>
            </yd-cell-item>

        </yd-cell-group>

    </div>
</template>

<script src="./clientdetail.ts" />
