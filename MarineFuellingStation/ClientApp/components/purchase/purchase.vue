<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="计划开单">
                <yd-cell-group title="单号：CG17070001" style="margin-top:20px">

                    <yd-cell-item>
                        <span slot="left">商品：</span>
                        <select slot="right">
                            <option value="">请选择油品</option>
                            <option value="1">93#</option>
                            <option value="2">95#</option>
                            <option value="3">97#</option>
                        </select>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入单价"></yd-input>
                        <span slot="right" style="width: 50px">元 / 吨</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入数量"></yd-input>
                        <span slot="right">吨</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">始发地：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入始发地"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">出发时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入出发时间，选填"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">预计到达时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入预计到达时间，选填"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>
                
                <yd-cell-group title="运输">
                    <yd-cell-item>
                        <span slot="left">车牌号一：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入车牌号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">挂车号：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入挂车号，选填"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">司机一：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">身份证一：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入身份证号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">电话号码一：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入电话号码"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">司机二：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">身份证二：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入身份证号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">电话号码二：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入电话号码"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="不显示" style="margin-top:10px">
                    <yd-cell-item>
                        <span slot="left">●开单员：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●订单创建时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●采购金额：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="单价 x 数量"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary">提交</yd-button>
                </div>
            </yd-tab-panel>
            <yd-tab-panel label="列表">

                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>广州黄埔港</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 桂CDD222</p>
                        </div>
                        <div slot="right" style="text-align: right;margin-right: 5px">
                            <p>
                                <span style="color:forestgreen;">柴油</span>
                                <span style="color:gray; font-size: 22px"s>35吨</span>
                            </p>
                            <p style="color:lightcoral;line-height: 25px">到达：07-07 08:30</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>广西贵港</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 桂CDD222</p>
                        </div>
                        <div slot="right" style="text-align: right;margin-right: 5px">
                            <p>
                                <span style="color:forestgreen;">循环油</span>
                                <span style="color:gray; font-size: 22px" s>35吨</span>
                            </p>
                            <p style="color:lightcoral;line-height: 25px">到达：07-07 08:30</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>

    </div>
</template>

<script src="./purchase.ts" />