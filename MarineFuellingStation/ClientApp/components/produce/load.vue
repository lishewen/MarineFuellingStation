<template>
    <div id="root">
        <yd-tab :change="change">
            <div style="text-align: center; margin-top: .4rem">
                <yd-button style="width:90%" type="primary" @click.native="showOrders = true">销售单{{order.name? '：' + order.name : ''}}</yd-button>
            </div>
            <yd-step :current="currStep" style="margin: .4rem 0 .4rem">
                <yd-step-item>
                    <span slot="bottom">选择销售仓</span>
                </yd-step-item>
                <yd-step-item>
                    <span slot="bottom">加油</span>
                </yd-step-item>
                <yd-step-item>
                    <span slot="bottom">完工</span>
                </yd-step-item>
            </yd-step>
            <div class="center" v-show="currStep == 1">
                <yd-button style="width:90%" type="primary" @click.native="showStores = true">选择销售仓</yd-button>
            </div>
            <div class="center" v-show="currStep == 2">
                <yd-button style="width:90%" type="primary" @click.native="changeState(5)">完工确认</yd-button>
            </div>

            <!--popup订单选择-->
            <yd-popup v-model="showOrders" position="right" width="70%">
                <yd-pullrefresh :callback="getOrders">
                    <yd-cell-group>
                        <yd-cell-item v-for="o in orders" v-show="o.state != 5" :key="o.id" @click.native="orderclick(o)" arrow>
                            <div slot="left" style="padding:.2rem 0 .2rem">
                                <p>{{o.carNo}}</p>
                                <p style="color:lightgray">{{o.name}}</p>
                            </div>
                            <div slot="right" style="text-align: left;margin-right: 5px">
                                <p style="color:gray"></p>
                                <p style="color:gray">{{o.count}}升</p>
                            </div>
                        </yd-cell-item>
                    </yd-cell-group>
                </yd-pullrefresh>
            </yd-popup>
            <!--popup销售仓选择-->
            <yd-popup v-model="showStores" position="right">
                <yd-cell-group title="请选择销售仓">
                    <yd-cell-item v-for="s in stores" :key="s.id" @click.native="storeclick(s)">
                        <div slot="left">
                            <p>{{s.name}}</p>
                        </div>
                        <div slot="right">
                            <p style="color:lightgray">{{s.value}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-popup>
        </yd-tab>
    </div>
</template>

<script src="./load.ts" />