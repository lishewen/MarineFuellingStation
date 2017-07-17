<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="化验录入">
                <yd-cell-group title="必填" style="margin-top:20px">

                    <yd-cell-item>
                        <span slot="left">油仓：</span>
                        <select slot="right">
                            <option value="">请选择油仓</option>
                            <option value="1">1#船759</option>
                            <option value="2">2#地仓</option>
                            <option value="3">3#船759</option>
                        </select>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">联系人：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入联系人"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">联系电话：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入联系电话"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>

                <yd-cell-group title="不显示" style="margin-top:10px">
                    <yd-cell-item>
                        <span slot="left">●账户金额：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="收银流程充值自动计算"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary">提交</yd-button>
                </div>

            </yd-tab-panel>
            <yd-tab-panel label="记录">

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
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<script src="./assay.ts" />