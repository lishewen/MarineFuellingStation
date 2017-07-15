<template>
    <div id="root">
        <yd-grids-group :rows="5">
            <yd-grids-item>
                <p slot="text">今日</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">昨日</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">本周</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">本月</p>
            </yd-grids-item>
            <yd-grids-item @click.native="show4 = true">
                <p slot="text">指定日期</p>
            </yd-grids-item>
        </yd-grids-group>
        <div style="text-align:center;margin-top:10px;font-size:18px">
            2017-07-07 至 2017-07-08
        </div>
        <yd-cell-group title="总提：￥140" style="margin-top:10px">
            <yd-cell-item arrow>
                <div slot="left">
                    <p>船0002</p>
                    <p style="color:lightgray;font-size:12px">XS2017070700001</p>
                </div>
                <span slot="right" style="padding-right:10px;color:red">提：￥100</span>
                <span slot="right" style="color:blue; padding-left:10px">已完成</span>
            </yd-cell-item>
            <yd-cell-item arrow>
                <div slot="left">
                    <p>船0002</p>
                    <p style="color:lightgray;font-size:12px">XS2017070700001</p>
                </div>
                <span slot="right" style="padding-right:10px;color:red">提：￥20</span>
                <span slot="right" style="color:blue; padding-left:10px">已完成</span>
            </yd-cell-item>
            <yd-cell-item arrow>
                <div slot="left">
                    <p>船0002</p>
                    <p style="color:lightgray;font-size:12px">XS2017070700001</p>
                </div>
                <span slot="right" style="padding-right:10px;color:red">提：￥20</span>
                <span slot="right" style="color:blue; padding-left:10px">已完成</span>
            </yd-cell-item>
        </yd-cell-group>
        <yd-popup v-model="show4" position="right">
            <yd-cell-group>
                <yd-cell-item>
                    <span slot="left">从：</span>
                    <input slot="right" class="cell-input" type="date" value="2017-07-19" placeholder="">
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">到：</span>
                    <input slot="right" class="cell-input" type="date" value="2017-07-19" placeholder="">
                </yd-cell-item>
                <div style="padding:0px 10px 10px 10px">
                    <yd-button size="large" type="primary" @click.native="timesubmit()">提交</yd-button>
                </div>
            </yd-cell-group>
        </yd-popup>
    </div>
</template>

<script src="./myorder.ts"/>