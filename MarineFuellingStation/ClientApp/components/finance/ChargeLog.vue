<style>
    .clog-color_red{
        background-color:lightcoral;
    }
    .clog-color_green {
        background-color: forestgreen;
    }
    .clog-color_red, .clog-color_green {
        color: #fff;
        width: auto;
        padding: .2rem;
    }
    .clog-font_red{color: lightcoral}
    .clog-font_green {color: forestgreen}
    .clog-font_red, .clog-font_green{font-weight: bold}
</style>
<template>
    <div id="root">
        <weui-search v-model="sv" />
        <yd-cell-group title="充值记录">
            <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                <yd-cell-item slot="list" v-for="c in chargeLogs" :key="c.id" style="padding: 10px 0 10px 10px">
                    <div slot="left" :class="classChargeType(c.chargeType)">{{strChargeType(c.chargeType)}}</div>
                    <div slot="left" style="margin-left: .1rem">
                        <p>{{c.client.carNo}}</p>
                        <p style="color:gray">{{c.companyName}}</p>
                    </div>
                    <div slot="right" style="line-height: .4rem">
                        <p :class="classMoney(c.chargeType)">￥{{c.money}}</p>
                        <p>{{formatDate(c.createdAt)}}</p>
                    </div>
                </yd-cell-item>
                <!-- 数据全部加载完毕显示 -->
                <span slot="doneTip">没有数据啦~~</span>
                <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
            </yd-infinitescroll>
        </yd-cell-group>
    </div>
</template>

<script src="./chargelog.ts" />