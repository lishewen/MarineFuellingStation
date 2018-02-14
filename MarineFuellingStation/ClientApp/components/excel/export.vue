<template>
    <div id="root">
        <div class="align-center">
            <yd-button @click.native="exportClientsclick" style="width: 90%; height: 38px; margin: .2rem 0">导出客户数据</yd-button>
        </div>
        <div class="align-center">
            <yd-button @click.native="exportOrdersclick" style="width: 90%; height: 38px; margin: .2rem 0">导出订单数据</yd-button>
        </div>
        <div class="align-center">
            <yd-button @click.native="exportStoresclick" style="width: 90%; height: 38px; margin: .2rem 0">导出油仓数据</yd-button>
        </div>

        <!--popup时间段选择-->
        <yd-popup v-model="showSelectTime" position="right" width="70%">
            <yd-cell-group title="请选择">
                <yd-cell-item v-show="type != 3">
                    <span slot="left">从：</span>
                    <yd-datetime type="date" v-model="startDate" slot="right"></yd-datetime>
                </yd-cell-item>
                <yd-cell-item v-show="type != 3">
                    <span slot="left">到：</span>
                    <yd-datetime type="date" v-model="endDate" slot="right"></yd-datetime>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button size="large" @click.native="submitclick" :disabled="isPreventSubmmit()">提交</yd-button>
        </yd-popup>
    </div>
</template>

<script src="./export.ts" />

