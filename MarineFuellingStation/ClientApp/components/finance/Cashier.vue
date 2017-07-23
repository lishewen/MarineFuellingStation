<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="待结算">
                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow @click.native="show2 = true">
                        <div slot="left">
                            <p style="font-size: 18px">船00001</p>
                            <p style="color:lightgray;font-size:14px">XS17070001</p>
                        </div>
                        <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                            <p style="color:gray;font-size:22px">￥20000</p>
                            <p style="color:lightgray;font-size:14px;margin-top:5px">柴油 / 200L / 4.13</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show2 = true">
                        <div slot="left">
                            <p style="font-size: 18px">桂DCT200</p>
                            <p style="color:lightgray;font-size:14px">XS17070001</p>
                        </div>
                        <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                            <p style="color:gray;font-size:22px">￥20000</p>
                            <p style="color:lightcoral;font-size:14px;margin-top:5px">开票</p>
                            <p style="color:lightgray;font-size:14px;margin-top:5px">柴油 / 200吨 / 4.13</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="已结算">
                
                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p style="font-size: 18px">船00001</p>
                            <p style="color:lightgray;font-size:14px">XS17070001</p>
                        </div>
                        <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                            <p style="color:gray;font-size:22px">￥20000</p>
                            <p style="color:lightcoral;font-size:14px;margin-top:5px">2017-07-07 08:30</p>
                            <p style="color:lightgray;font-size:14px">柴油 / 200L / 4.13</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p style="font-size: 18px">桂DCT200</p>
                            <p style="color:lightgray;font-size:14px">XS17070001</p>
                        </div>
                        <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                            <p style="color:gray;font-size:22px">￥20000</p>
                            <p style="color:lightcoral;font-size:14px;margin-top:5px">2017-07-07 08:30</p>
                            <p style="color:lightgray;font-size:14px">柴油 / 200吨 / 4.13</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="挂账">
                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p style="font-size: 18px">船00001</p>
                            <p style="color:lightgray;font-size:14px">XS17070001</p>
                        </div>
                        <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                            <p style="color:gray;font-size:22px">￥20000</p>
                            <p style="color:forestgreen;font-size:14px;margin-top:5px">7天前</p>
                            <p style="color:lightgray;font-size:14px">柴油 / 200L / 4.13</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p style="font-size: 18px">船00001</p>
                            <p style="color:lightgray;font-size:14px">XS17070001</p>
                        </div>
                        <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                            <p style="color:gray;font-size:22px">￥20000</p>
                            <p style="color:forestgreen;font-size:14px;margin-top:5px">7天前</p>
                            <p style="color:lightgray;font-size:14px">柴油 / 200L / 4.13</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-popup v-model="show2" position="right">
            <yd-cell-group title="第一步：结账方式" v-show="lastshow">
                <yd-cell-item type="checkbox">
                    <span slot="left">现金</span>
                    <input slot="right" type="checkbox" value="1" v-model="checkbox2" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">刷卡一</span>
                    <input slot="right" type="checkbox" value="2" v-model="checkbox2" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">刷卡二</span>
                    <input slot="right" type="checkbox" value="3" v-model="checkbox2" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">刷卡三</span>
                    <input slot="right" type="checkbox" value="4" v-model="checkbox2" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">微信</span>
                    <input slot="right" type="checkbox" value="5" v-model="checkbox2" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">支付宝</span>
                    <input slot="right" type="checkbox" value="6" v-model="checkbox2" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <div slot="left">
                        <p> 账户余额</p>
                        <p style="color:red;font-size:12px">￥200000.00</p>
                    </div>
                    <input slot="right" type="checkbox" value="7" v-model="checkbox2" />
                </yd-cell-item>

            </yd-cell-group>
            <yd-cell-group title="第二步：结算金额" v-show="!lastshow">
                <yd-cell-item v-show="!lastshow">
                    <span slot="left" style="color:red">应收：</span>
                    <span slot="right" style="color:red;font-size:18px">￥20000.00元</span>
                </yd-cell-item>
                <yd-cell-item v-show="!lastshow">
                    <span slot="left">实收：</span>
                    <span slot="right">￥18000.00元</span>
                </yd-cell-item>
                <yd-cell-item v-show="!lastshow">
                    <span slot="left">找零：</span>
                    <span slot="right">￥2000.00元</span>
                </yd-cell-item>
            </yd-cell-group>
            <yd-cell-group title="输入金额" v-show="!lastshow">
                <yd-cell-item v-show="type1">
                    <span slot="left">现金：</span>
                    <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="type2">
                    <span slot="left">刷卡一：</span>
                    <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="type3">
                    <span slot="left">刷卡二：</span>
                    <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="type4">
                    <span slot="left">刷卡三：</span>
                    <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="type5">
                    <span slot="left">微信：</span>
                    <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                    <span slot="right">元</span>s
                </yd-cell-item>
                <yd-cell-item v-show="type6">
                    <span slot="left">支付宝：</span>
                    <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="type7">
                    <span slot="left">账户余额：</span>
                    <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="warning" @click.native="nextclick()" v-show="lastshow">下一步</yd-button>
            </div>
            <div style="text-align: center">
                <yd-button style="width:100px;margin-top:10px" type="primary" @click.native="" v-show="lastshow">挂账</yd-button>
            </div>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="payclick()" v-show="!lastshow">结账</yd-button>
            </div>
            <div style="text-align: center">
                <yd-button style="width:100px;margin-top:10px" type="warning" @click.native="lastclick()" v-show="!lastshow">上一步</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./cashier.ts" />