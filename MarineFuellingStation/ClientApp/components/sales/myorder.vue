<template>
    <div id="root">
        <yd-grids-group :rows="5">
            <yd-grids-item>
                <div slot="text">
                    今日<yd-badge type="danger" style="position:absolute;margin-top:-12px;margin-left:-5px">10</yd-badge>
                </div>
            </yd-grids-item>
            <yd-grids-item>
                <div slot="text">
                    昨日
                </div>
            </yd-grids-item>
            <yd-grids-item>
                <div slot="text">
                    本周<yd-badge type="danger" style="position:absolute;margin-top:-12px;margin-left:-5px">1000</yd-badge>
                </div>
            </yd-grids-item>
            <yd-grids-item>
                <div slot="text">
                    本月<yd-badge type="danger" style="position:absolute;margin-top:-12px;margin-left:-5px">10000</yd-badge>
                </div>
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
                    <p style="color:lightgray;font-size:12px">XS07070001</p>
                </div>
                <span slot="right" style="color:lightcoral;width:70px;text-align:left">提：￥1000</span>
                <span slot="right" style="color:blue; padding-left:10px">已完成</span>
            </yd-cell-item>
            <yd-cell-item arrow>
                <div slot="left">
                    <p>船0002</p>
                    <p style="color:lightgray;font-size:12px">XS07070001</p>
                </div>
                <span slot="right" style="color:lightcoral;width:70px;text-align:left">提：￥20</span>
                <span slot="right" style="color:blue; padding-left:10px">已完成</span>
            </yd-cell-item>
            <yd-cell-item arrow>
                <div slot="left">
                    <p>船0002</p>
                    <p style="color:lightgray;font-size:12px">XS07070001</p>
                </div>
                <span slot="right" style="color:lightcoral;width:70px;text-align:left">提：￥20</span>
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
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="timesubmit()">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./myorder.ts"/>