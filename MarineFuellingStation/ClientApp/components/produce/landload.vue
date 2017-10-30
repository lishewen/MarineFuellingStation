<style>
    .center {
        text-align: center;
    }
</style>
<template>
    <div id="root">
        <div style="text-align: center; margin-top: .4rem">
            <yd-button style="width:90%" type="primary" @click.native="showOrdersclick">销售单{{order.name? '：' + order.name : ''}}</yd-button>
        </div>
        <yd-step :current="currStep" style="margin: .4rem 0 .4rem">
            <yd-step-item>
                <span slot="bottom">选择油仓</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">空车过磅</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">加油</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">油车过磅</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">完工</span>
            </yd-step-item>
        </yd-step>
        <!--1-选择油仓-->
        <div class="center" v-show="currStep == 1">
            <yd-button style="width:90%" type="primary" @click.native="showStores = true">选择销售仓</yd-button>
        </div>
        <!--2-空车过磅-->
        <yd-cell-group title="空车过磅" v-show="currStep == 2">
            <yd-cell-item>
                <span slot="left">磅秤数：</span>
                <yd-input slot="right" v-model="order.emptyCarWeight" type="number" required placeholder="请输入磅秤数"></yd-input>
                <span slot="right">吨</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">图片上传：</span>
                <input slot="left" type="file" value="选择图片" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />
            </yd-cell-item>
        </yd-cell-group>
        <div class="center" v-show="currStep == 2">
            <yd-button style="width:90%" type="primary" @click.native="changeState(3)">前往加油</yd-button>
        </div>
        <!--3-加油-->
        <div class="center" v-show="currStep == 3">
            <yd-cell-item>
                <span slot="left">加油后表数1：</span>
                <yd-input slot="right" v-model="order.instrument1" type="number" required placeholder="请输入装油表数1"></yd-input>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">加油后表数2：</span>
                <yd-input slot="right" v-model="order.instrument2" type="number" required placeholder="请输入装油表数2"></yd-input>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">加油后表数3：</span>
                <yd-input slot="right" v-model="order.instrument3" type="number" required placeholder="请输入装油表数3"></yd-input>
            </yd-cell-item>
            <yd-button style="width:90%" type="primary" @click.native="changeState(4)">加油完毕，前往过磅</yd-button>
        </div>
        <!--4-油车过磅-->
        <yd-cell-group title="油车过磅" v-show="currStep == 4">
            <yd-cell-item>
                <span slot="left">磅秤数：</span>
                <yd-input slot="right" v-model="order.oilCarWeight" type="number" required placeholder="请输入磅秤数"></yd-input>
                <span slot="right">吨</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">图片上传：</span>
                <input slot="left" type="file" value="选择图片" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />
            </yd-cell-item>
        </yd-cell-group>
        <div class="center" v-show="currStep == 4">
            <yd-button style="width:90%" type="primary" @click.native="changeState(5)">完工确认</yd-button>
        </div>
        <!--popup订单选择-->
        <yd-popup v-model="showOrders" position="right" width="70%">
            <yd-pullrefresh :callback="getOrders">
                <yd-cell-group>
                    <yd-cell-item v-for="o in orders" v-show="o.state != 4" :key="o.id" @click.native="orderclick(o)" arrow>
                        <div slot="left" style="padding:.2rem 0 .2rem">
                            <p>{{o.name}}</p>
                            <p style="color: gray">车牌：{{o.carNo}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">{{o.product.name}}</p>
                            <p style="color:gray">{{o.count}}吨</p>
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
    </div>
</template>

<script src="./landload.ts" />