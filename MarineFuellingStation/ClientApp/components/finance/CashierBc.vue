<template>
    <div id="root">
        <yd-tab :callback="change">
            <yd-tab-panel label="待结算">
                <yd-cell-group>
                    <yd-search v-model="sv1" />
                    <yd-infinitescroll :callback="loadList" ref="bcinfinitescroll1">
                        <yd-cell-item slot="list" arrow @click.native="boatclick(b)" v-for="b in readypayboats" :key="b.id">
                            <div slot="left">
                                <p class="font16">{{b.carNo}}</p>
                            </div>
                            <div slot="right" class="align-right lineheight24" style="margin:10px 5px 10px 0px;">
                                <p class="font16 col-gray">￥{{b.money}}</p>
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
                    <yd-search v-model="sv2" />
                    <yd-infinitescroll :callback="loadList" ref="bcinfinitescroll2">
                        <yd-cell-item slot="list" arrow v-for="b in hasypayboats" :key="b.id" @click.native="showPaymentsclick(b)">
                            <div slot="left">
                                <p class="font16">{{b.carNo}}</p>
                                <p class="col-light-gray font14">{{b.name}}</p>
                            </div>
                            <div slot="right" class="align-right lineheight24" style="margin:10px 5px 10px 0px;">
                                <p class="font16 col-gray">￥{{b.money}}</p>
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
                    <yd-search v-model="sv3" />
                    <yd-infinitescroll :callback="loadList" ref="bcinfinitescroll3">
                        <yd-cell-item slot="list" arrow v-for="b in nopayboats" :key="b.id" @click.native="boatclick(b)">
                            <div slot="left">
                                <p class="font16">{{b.carNo}}</p>
                                <p class="color_lightgray font14">{{b.name}}</p>
                            </div>
                            <div slot="right" class="align-right lineheight24" style="margin:10px 5px 10px 0px;">
                                <p class="col-gray font16">￥{{b.money}}</p>
                                <p class="col-green font14" style="margin-top:5px">{{getDiff(b.createdAt)}}</p>
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
            <yd-cell-group title="第一步：结账方式" v-show="showStep1">
                <yd-cell-item type="checkbox">
                    <span slot="left">现金</span>
                    <input slot="right" type="checkbox" value="现金|0" v-model="payTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">微信</span>
                    <input slot="right" type="checkbox" value="微信|1" v-model="payTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">支付宝</span>
                    <input slot="right" type="checkbox" value="支付宝|2" v-model="payTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">桂行刷卡</span>
                    <input slot="right" type="checkbox" value="桂行刷卡|3" v-model="payTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">工行刷卡</span>
                    <input slot="right" type="checkbox" value="工行刷卡|4" v-model="payTypes" />
                </yd-cell-item>

                <!--<yd-cell-item type="checkbox" v-show="selectedOrder.client != null">
                    <div slot="left">
                        <p> 账户余额</p>
                        <p style="color:red;font-size:12px">￥{{selectedOrder.client == null? 0 : selectedOrder.client.balances}}</p>
                    </div>
                    <input slot="right" type="checkbox" value="6" v-model="orderPayTypes" />
                </yd-cell-item>

                <yd-cell-item type="checkbox" v-show="selectedOrder.client.company != null">
                    <div slot="left">
                        <p> 公司账户余额</p>
                        <p style="color:red;font-size:12px">￥{{selectedOrder.client.company == null? 0 : selectedOrder.client.company.balances}}</p>
                    </div>
                    <input slot="right" type="checkbox" value="7" v-model="orderPayTypes" />
                </yd-cell-item>-->
            </yd-cell-group>

            <yd-cell-group title="第二步：结算金额" v-show="!showStep1">
                <yd-cell-item v-show="!showStep1">
                    <span slot="left" class="col-red">应收：</span>
                    <span slot="right" class="col-red font16">￥{{selectedBc.money}}元</span>
                </yd-cell-item>
                <yd-cell-item v-show="!showStep1">
                    <span slot="left">实收：</span>
                    <span slot="right">￥{{payInfact}}元</span>
                </yd-cell-item>
                <yd-cell-item v-show="!showStep1">
                    <span slot="left">找零：</span>
                    <span slot="right">￥{{payInfact - selectedBc.money}}元</span>
                </yd-cell-item>
                <yd-cell-item v-for="(py,idx) in payTypes" :key="idx">
                    <span slot="left">{{py.split('|')[0]}}：</span>
                    <yd-input slot="right" type="number" v-model="payMoneys[idx]" required></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <div class="align-center">
                <yd-button style="width:80%" type="warning" @click.native="nextclick()" v-show="showStep1">下一步</yd-button>
            </div>
            <div class="align-center">
                <yd-button style="width:80%;margin-top:10px" type="primary" v-show="showStep1" @click.native="putPayOnCredit()">挂账</yd-button>
            </div>
            <div class="align-center">
                <yd-button style="width:80%" type="warning" @click.native="lastclick()" v-show="!showStep1">上一步</yd-button>
            </div>
            <div class="align-center">
                <yd-button style="width:80%;margin-top:10px" type="primary" @click.native="validateMoney" v-show="!showStep1" :disabled="payInfact - selectedBc.money < 0">结账</yd-button>
            </div>
        </yd-popup>
        <!--popup付款金额和方式记录-->
        <yd-popup v-model="showPayments" position="right" width="50%">
            <yd-cell-group title="付款金额和方式">
                <yd-cell-item type="label" v-for="p in boatPayments" :key="p.id">
                    <span slot="left">{{strPayType(p.payTypeId)}}</span>
                    <span slot="right">￥{{p.money}}</span>
                </yd-cell-item>
            </yd-cell-group>
            <div class="align-right" style="padding-right:.2rem">找零：￥{{selectedBc.money - totalPayMoney}}</div>
        </yd-popup>
        <!--actionsheet-->
        <yd-actionsheet :items="actItems" v-model="showAct" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./cashierbc.ts" />
<style src="./../website.css"></style>