<template>
    <div id="root">
        <yd-grids-group :rows="4">
            <yd-grids-item>
                <p slot="text">全部</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">个人</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">公司</p>
            </yd-grids-item>
            <yd-grids-item @click.native="show2 = true">
                <p slot="text">筛选</p>
            </yd-grids-item>
        </yd-grids-group>

        <yd-cell-group>
            <yd-cell-item arrow>
                <div slot="left">
                    <p>船0002 【张三】</p>
                    <p style="color:lightgray;font-size:12px">广西XXXX有限公司</p>
                </div>
                <div slot="right" style="text-align: left;margin-right: 5px">
                    <p style="color:gray">余额：￥20000</p>
                    <p style="color:lightcoral;line-height: 25px">最近：2017-07-07</p>
                </div>
            </yd-cell-item>
            <yd-cell-item arrow>
                <div slot="left">
                    <p>船0002 【张三】</p>
                    <p style="color:lightgray;font-size:12px">个人</p>
                </div>
                <div slot="right" style="text-align: left;margin-right: 5px">
                    <p style="color:lightgray">无余额</p>
                    <p style="color:lightcoral;line-height: 25px">最近：2017-07-07</p>
                </div>
            </yd-cell-item>
            <yd-cell-item arrow>
                <div slot="left">
                    <p>船0002 【张三】</p>
                    <p style="color:lightgray;font-size:12px">个人</p>
                </div>
                <div slot="right" style="text-align: left;margin-right: 5px">
                    <p style="color:gray">余额：￥156</p>
                    <p style="color:lightcoral;line-height: 25px">最近：2017-07-07</p>
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