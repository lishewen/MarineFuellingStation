<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="待结算">
                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow @click.native="orderclick(o)" v-for="o in orders" v-show="o.state == 5">
                        <div slot="left">
                            <p style="font-size: 18px">{{o.carNo}}</p>
                            <p style="color:lightgray;font-size:14px">{{o.name}}</p>
                        </div>
                        <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                            <p style="color:gray;font-size:22px">￥{{o.totalMoney}}</p>
                            <p style="color:lightgray;font-size:14px;margin-top:5px">{{o.product.name}} / {{o.count}}{{o.unit}} / {{o.price}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="已结算">
                
                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow @click.native="" v-for="o in orders" v-show="o.state == 6">
                        <div slot="left">
                            <p style="font-size: 18px">{{o.carNo}}</p>
                            <p style="color:lightgray;font-size:14px">{{o.name}}</p>
                        </div>
                        <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                            <p style="color:gray;font-size:22px">￥{{o.totalMoney}}</p>
                            <p style="color:lightgray;font-size:14px;margin-top:5px">{{o.product.name}} / {{o.count}}{{o.unit}} / {{o.price}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="挂账">
                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow @click.native="" v-for="o in orders" v-show="o.state == 7">
                        <div slot="left">
                            <p style="font-size: 18px">{{o.carNo}}</p>
                            <p style="color:lightgray;font-size:14px">{{o.name}}</p>
                        </div>
                        <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                            <p style="color:gray;font-size:22px">￥{{o.totalMoney}}</p>
                            <p style="color:forestgreen;font-size:14px;margin-top:5px">{{getDiff(o.createdAt)}}</p>
                            <p style="color:lightgray;font-size:14px;margin-top:5px">{{o.product.name}} / {{o.count}}{{o.unit}} / {{o.price}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-popup v-model="show2" position="right" width="70%">
            <yd-cell-group title="第一步：结账方式" v-show="lastshow">
                <yd-cell-item type="checkbox">
                    <span slot="left">现金</span>
                    <input slot="right" type="checkbox" value="0" v-model="orderPayTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">微信</span>
                    <input slot="right" type="checkbox" value="1" v-model="orderPayTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">支付宝</span>
                    <input slot="right" type="checkbox" value="2" v-model="orderPayTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">刷卡一</span>
                    <input slot="right" type="checkbox" value="3" v-model="orderPayTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">刷卡二</span>
                    <input slot="right" type="checkbox" value="4" v-model="orderPayTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">刷卡三</span>
                    <input slot="right" type="checkbox" value="5" v-model="orderPayTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <div slot="left">
                        <p> 账户余额</p>
                        <p style="color:red;font-size:12px">￥200000.00</p>
                    </div>
                    <input slot="right" type="checkbox" value="6" v-model="orderPayTypes" />
                </yd-cell-item>

            </yd-cell-group>
            <yd-cell-group title="第二步：结算金额" v-show="!lastshow">
                <yd-cell-item v-show="!lastshow">
                    <span slot="left" style="color:red">应收：</span>
                    <span slot="right" style="color:red;font-size:18px">￥{{selectedOrder.totalMoney}}元</span>
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
                <yd-cell-item v-show="showInputs[0]">
                    <span slot="left">现金：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[0]" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="showInputs[1]">
                    <span slot="left">微信：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[1]" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="showInputs[2]">
                    <span slot="left">支付宝：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[2]" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="showInputs[3]">
                    <span slot="left">刷卡一：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[3]" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="showInputs[4]">
                    <span slot="left">刷卡二：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[4]" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="showInputs[5]">
                    <span slot="left">刷卡三：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[5]" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="showInputs[6]">
                    <span slot="left">账户余额：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[6]" placeholder=""></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:80%" type="warning" @click.native="nextclick()" v-show="lastshow">下一步</yd-button>
            </div>
            <div style="text-align: center">
                <yd-button style="width:80%;margin-top:10px" type="primary" @click.native="" v-show="lastshow">挂账</yd-button>
            </div>
            <div style="text-align: center">
                <yd-button style="width:80%;margin-top:10px" type="warning" @click.native="lastclick()" v-show="!lastshow">上一步</yd-button>
            </div>
            <div style="text-align: center">
                <yd-button style="width:80%" type="primary" @click.native="postPay()" v-show="!lastshow">结账</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./cashier.ts" />