<template>
    <div id="root">
        <yd-tab :callback="change">
            <yd-tab-panel label="待结算">
                <yd-cell-group>
                    <weui-search v-model="sv1" />
                    <yd-infinitescroll :callback="loadList" ref="orderinfinitescroll1">
                        <yd-cell-item slot="list" arrow @click.native="orderclick(o)" v-for="o in readypayorders" :key="o.id">
                            <div slot="left">
                                <p style="font-size: 18px">{{o.carNo}}</p>
                                <p style="color:lightgray;font-size:14px">{{o.name}}</p>
                            </div>
                            <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                                <p style="color:gray;font-size:22px">￥{{o.totalMoney}}</p>
                                <p style="color:lightgray;font-size:14px;margin-top:5px">{{o.product.name}} / {{o.count}}{{o.unit}} / {{o.price}}</p>
                                <p style="color: lightcoral" v-if="o.client != null">余额：￥{{o.client == null ? 0 : o.client.balances}}</p>
                            </div>
                        </yd-cell-item>

                        <!-- 数据全部加载完毕显示 -->
                        <span slot="doneTip">没有数据啦~</span>
                        <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                        <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                    </yd-infinitescroll>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="已结算">
                <yd-cell-group>
                    <weui-search v-model="sv2" />
                    <yd-infinitescroll :callback="loadList" ref="orderinfinitescroll2">
                        <yd-cell-item slot="list" arrow v-for="o in haspayorders" :key="o.id" @click.native="showPaymentsclick(o)">
                            <div slot="left">
                                <p style="font-size: 18px">{{o.carNo}}</p>
                                <p style="color:lightgray;font-size:14px">{{o.name}}</p>
                            </div>
                            <div slot="right" style="text-align: right;margin:10px 5px 10px 0px;line-height: 18px">
                                <p style="color:gray;font-size:22px">￥{{o.totalMoney}}</p>
                                <p style="color:lightgray;font-size:14px;margin-top:5px">{{o.product.name}} / {{o.count}}{{o.unit}} / {{o.price}}</p>
                            </div>
                        </yd-cell-item>

                        <!-- 数据全部加载完毕显示 -->
                        <span slot="doneTip">没有数据啦~</span>

                        <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                        <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                    </yd-infinitescroll>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="挂账">
                <yd-cell-group>
                    <weui-search v-model="sv3" />
                    <yd-infinitescroll :callback="loadList" ref="orderinfinitescroll3">
                        <yd-cell-item slot="list" arrow v-for="o in nopayorders" :key="o.id">
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

                        <!-- 数据全部加载完毕显示 -->
                        <span slot="doneTip">没有数据啦~</span>

                        <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                        <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                    </yd-infinitescroll>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-popup v-model="showPayTypes" position="right" width="70%">
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

                <yd-cell-item type="checkbox" v-show="selectedOrder.client != null">
                    <div slot="left">
                        <p> 账户余额</p>
                        <p style="color:red;font-size:12px">￥{{selectedOrder.client == null? 0 : selectedOrder.client.balances}}</p>
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
                    <span slot="right">￥{{getTotalPayMoney()}}元</span>
                </yd-cell-item>
                <yd-cell-item v-show="!lastshow">
                    <span slot="left">找零：</span>
                    <span slot="right">￥{{payInfact - selectedOrder.totalMoney}}元</span>
                </yd-cell-item>
            </yd-cell-group>
            <yd-cell-group title="输入金额" v-show="!lastshow">
                <yd-cell-item v-show="showInputs[0]">
                    <span slot="left">现金：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[0]"></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="showInputs[1]">
                    <span slot="left">微信：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[1]"></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="showInputs[2]">
                    <span slot="left">支付宝：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[2]"></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
                <yd-cell-item v-show="showInputs[3]">
                    <span slot="left">刷卡一：</span>
                    <yd-input slot="right" v-model="orderPayMoneys[3]"></yd-input>
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
                <yd-button style="width:80%;margin-top:10px" type="primary" v-show="lastshow" @click.native="putPayOnCredit()">挂账</yd-button>
            </div>
            <div style="text-align: center">
                <yd-button style="width:80%" type="warning" @click.native="lastclick()" v-show="!lastshow">上一步</yd-button>
            </div>
            <div style="text-align: center">
                <yd-button style="width:80%;margin-top:10px" type="primary" @click.native="validateMoney()" v-show="!lastshow" :disabled ="payInfact - selectedOrder.totalMoney < 0">结账</yd-button>
            </div>
        </yd-popup>
        <!--popup充值-->
        <yd-popup v-model="showCharge" position="right" width="70%">
            <yd-cell-group title="充值">
                <div style="text-align: center; padding: .2rem 0; font-size: .4rem">当前余额：￥{{selectedOrder.client == null ? "" : selectedOrder.client.balances}}</div>
                <yd-cell-item arrow type="label">
                    <span slot="left">支付方式：</span>
                    <select slot="right" v-model="chargeLog.payType">
                        <option value="0">现金</option>
                        <option value="1">微信</option>
                        <option value="2">支付宝</option>
                        <option value="3">刷卡一</option>
                        <option value="4">刷卡二</option>
                        <option value="5">刷卡三</option>
                    </select>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">充值金额：</span>
                    <yd-input slot="right" type="number" v-model="chargeLog.money"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:80%;margin-top:10px" type="primary" @click.native="postCharge()" :disabled="chargeLog.money <= 0">提交</yd-button>
            </div>
        </yd-popup>
        <!--popup付款金额和方式记录-->
        <yd-popup v-model="showPayments" position="right" width="50%">
            <yd-cell-group title="付款金额和方式">
                <yd-cell-item type="label" v-for="p in orderPayments" :key="p.id">
                    <span slot="left">{{strPayType(p.payTypeId)}}</span>
                    <span slot="right">￥{{p.money}}</span>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: right;padding-right:.2rem">找零：￥{{selectedOrder.totalMoney - totalPayMoney}}</div>
        </yd-popup>
        <!--actionsheet-->
        <yd-actionsheet :items="actItems" v-model="showAct" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./cashier.ts" />