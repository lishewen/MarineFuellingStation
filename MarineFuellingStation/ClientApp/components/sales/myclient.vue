<template>
    <div id="root">

        <yd-cell-group>
            <div style="text-align: center;padding: 10px 0 10px">
                <span v-for="f in filterBtns">
                    <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f)">{{f.name}}</yd-button>
                    <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f)">{{f.name}}</yd-button>
                </span>
                <span><yd-button type="hollow" @click.native="show2 = true">筛选</yd-button></span>
            </div>
            <yd-cell-item arrow v-for="c in clients" :key="c.id">
                <div slot="left">
                    <p>{{c.carNo}} - {{c.contact}}</p>
                    <p style="color:lightgray;font-size:12px">{{c.company.name}}</p>
                </div>
                <div slot="right" style="text-align: left;margin-right: 5px">
                    <p style="color:gray">余额：￥{{c.company.balances}}</p>
                    <p style="color:lightcoral;line-height: 25px">最近：{{formatDate(c.lastUpdatedAt)}}</p>
                </div>
            </yd-cell-item>
        </yd-cell-group>

        <yd-popup v-model="show2" position="right">
            <yd-cell-group title="单选：计划单" style="margin-top:20px">
                <yd-flexbox style="line-height: 60px">
                    <yd-flexbox-item style="text-align: center"><yd-button type="warning">已计划</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center"><yd-button type="hollow">已完成</yd-button></yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item style="text-align: center; margin-bottom: 20px"><yd-button type="hollow">已审批</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center; margin-bottom: 20px"></yd-flexbox-item>
                </yd-flexbox>
            </yd-cell-group>
            <yd-cell-group title="单选：账户余额">
                <yd-flexbox style="line-height: 60px">
                    <yd-flexbox-item style="text-align: center"><yd-button type="warning">少于1000</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center"><yd-button type="hollow">少于10000</yd-button></yd-flexbox-item>
                </yd-flexbox>
            </yd-cell-group>
            <yd-cell-group title="单选：周期">
                <yd-flexbox style="line-height: 60px">
                    <yd-flexbox-item style="text-align: center"><yd-button type="warning">7天不计划</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center"><yd-button type="hollow">15天不计划</yd-button></yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item style="text-align: center; margin-bottom: 20px"><yd-button type="hollow">30天不计划</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center; margin-bottom: 20px"><yd-button type="hollow">90天不计划</yd-button></yd-flexbox-item>
                </yd-flexbox>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="filterclick()">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./myclient.ts" />