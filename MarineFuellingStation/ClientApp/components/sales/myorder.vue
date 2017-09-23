<style>
    .navBtn {
        width: 1rem;
        height: .8rem;
    }
</style>
<template>
    <div id="root">
        <yd-pullrefresh :callback="getOrders">
            <div style="text-align:center;margin-top:10px;font-size:18px">
                <div style="display: flex; line-height: .6rem">
                    <yd-datetime type="date" v-model="startDate" slot="right"></yd-datetime>
                    <div>至</div>
                    <yd-datetime type="date" v-model="endDate" slot="right"></yd-datetime>
                    <yd-button type="primary" style="width: 2rem; height: .6rem; margin-right: .2rem" @click.native="query">查询</yd-button>
                </div>
            </div>
            <div style="text-align: center;padding: 10px 0 10px">
                <div v-for="(f,idx) in filterBtns" style="display: inline-block" :key="f.id">
                    <yd-button class="navBtn" type="warning" v-if="f.actived" @click.native="switchBtn(f,idx)">{{f.name}}</yd-button>
                    <yd-button class="navBtn" type="hollow" v-if="!f.actived" @click.native="switchBtn(f,idx)">{{f.name}}</yd-button>
                </div>
            </div>
            <yd-cell-group :title="getTotalSalesComm()" style="margin-top:10px">
                <yd-cell-item arrow v-for="o in orders" :key="o.id" @click.native="godetail(o.id)" style="padding: .2rem 0 .2rem">
                    <div slot="left">
                        <p>{{o.carNo}}</p>
                        <p style="color:lightgray;font-size:12px">{{o.name}}</p>
                    </div>
                    <div slot="right" style="color:lightcoral;width:80px;text-align:left">提：￥{{o.salesCommission}}</div>
                    <div slot="right" style="padding-right: .1rem">
                        <p style="color:forestgreen; padding-left:10px">{{strState(o)}}</p>
                        <p style="color:gray;margin-top:0.1rem">{{strPayState(o)}}</p>
                    </div> 
                </yd-cell-item>
            </yd-cell-group>
        </yd-pullrefresh>
</div>
</template>

<script src="./myorder.ts" />