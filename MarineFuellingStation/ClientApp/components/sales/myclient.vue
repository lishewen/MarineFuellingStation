<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="添加">
                <yd-cell-group title="总提：￥140" style="margin-top:10px">
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="列表">
                <yd-grids-group :rows="2" style="margin-top:10px">
                    <yd-grids-item>
                        <p slot="text">个人</p>
                    </yd-grids-item>
                    <yd-grids-item>
                        <p slot="text">公司</p>
                    </yd-grids-item>
                </yd-grids-group>
                <yd-cell-group title="总提：￥140" style="margin-top:10px">
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>船0002</p>
                            <p style="color:lightgray;font-size:12px">广西XXXX有限公司</p>
                        </div>
                        <span slot="right" style="color:gray; padding-left:10px">余额：￥20000</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>船0002</p>
                            <p style="color:lightgray;font-size:12px">个人</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>船0002</p>
                            <p style="color:lightgray;font-size:12px">个人</p>
                        </div>
                        <span slot="right" style="color:gray; padding-left:10px">余额：￥18888</span>
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
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<script src="./myclient.ts" />