<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="销售开单">

                <yd-popup v-model="salesplanshow" position="right">
                    <yd-cell-group>
                        <div style="text-align: center">
                            <yd-button style="width:80%;margin:10px 0 10px 0" type="primary" @click.native="emptyclick()">散客</yd-button>
                        </div>
                        <weui-search v-model="sv" />
                        <yd-cell-item arrow @click.native="planitemclick(s)" v-for="s in salesplans">
                            <span slot="left">{{s.carNo}}</span>
                            <span slot="left" style="color:lightgray;margin-left:10px">{{s.createdBy}}</span>
                            <span slot="right">{{formatShortDate(s.oilDate)}}</span>
                        </yd-cell-item>
                    </yd-cell-group>
                </yd-popup>

                <yd-cell-group :title="'单号：' + model.name" style="margin-top:20px">
                    <yd-cell-item arrow @click.native="salesplanselect">
                        <span slot="left">计划单：</span>
                        <span slot="right">{{selectedplanNo}}</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <yd-radio-group slot="left" v-model="radio2">
                            <yd-radio val="1">水上</yd-radio>
                            <yd-radio val="2">陆上</yd-radio>
                            <yd-radio val="3">机油</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">船号：</span>
                        <yd-input slot="right" v-model="model.carNo" required placeholder="请输入您的船号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item arrow @click.native="oilshow = true">
                        <span slot="left">商品：</span>
                        <span slot="right">{{oilName}}</span> 
                    </yd-cell-item>

                    <yd-cell-item v-show="hasplan">
                        <span slot="left">计划单价：</span>
                        <yd-input slot="right" v-model="model.price" readonly></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">订单单价：</span>
                        <yd-input slot="right" v-model="model.price" required placeholder="请输入单价"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item v-show="hasplan">
                        <span slot="left">计划数量：</span>
                        <yd-input slot="right" v-model="model.count" readonly></yd-input>
                        <span slot="right" style="width:70px">单位：{{unit}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">订单数量：</span>
                        <yd-input slot="right" v-model="model.count" required placeholder="请输入加油数量"></yd-input>
                        <span slot="right" style="width:70px">单位：{{unit}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">总价：</span>
                        <yd-input slot="right" v-model="model.totalMoney" placeholder="自动计算，单价 x 数量" readonly></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">是否开票</span>
                        <span slot="right">
                            <yd-switch v-model="model.isInvoice"></yd-switch>
                        </span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">票类：</span>
                        <select slot="right" v-model="model.ticketType">
                            <option value="">请选择票类</option>
                            <option value="1">普通票</option>
                            <option value="2">专用票</option>
                        </select>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">开票单位：</span>
                        <yd-input slot="right" v-model="model.billingCompany" placeholder="请输入开票单位"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="model.billingPrice" placeholder="请输入开票单价，默认同上"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="model.billingCount" placeholder="请输入开票，默认同上"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-cell-group title="选填" v-show="show2">
                    <yd-cell-item>
                        <span slot="left">是否运输</span>
                        <span slot="right">
                            <yd-switch v-model="istrans"></yd-switch>
                        </span>
                    </yd-cell-item>
                    <yd-cell-item arrow v-show="istrans" @click.native="show1 = true">
                        <span slot="left">运输单：</span>
                        <span slot="right">{{selectedtransord}}</span>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-popup v-model="show1" position="right">
                    <yd-cell-group>
                        <yd-cell-item arrow @click.native="transitemclick()">
                            <span slot="left">YS07070001</span>
                            <span slot="left" style="color:lightgray;margin-left:10px">李四</span>
                        </yd-cell-item>
                        <yd-cell-item arrow @click.native="transitemclick()">
                            <span slot="left">YS07070001</span>
                            <span slot="left" style="color:lightgray;margin-left:10px">张三</span>
                        </yd-cell-item>
                        <yd-cell-item arrow @click.native="transitemclick()">
                            <span slot="left">YS07070001</span>
                            <span slot="left" style="color:lightgray;margin-left:10px">王五</span>
                        </yd-cell-item>
                    </yd-cell-group>
                </yd-popup>

                <div>
                    <yd-button size="large" type="primary">提交</yd-button>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <weui-search v-model="sv" />
                <yd-cell-group>
                    <yd-cell-item arrow>
                        <span slot="left">船0001</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:red; padding-left:10px">&#12288;已完成</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:green; padding-left:10px">&#12288;装油中</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:darkorange; padding-left:10px">装油结束</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">93# 90L ￥3.42</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:blue; padding-left:10px">&#12288;已开单</span>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-actionsheet :items="oiloptions" v-model="oilshow" cancel="取消"></yd-actionsheet>
    </div>
</template>

<style src="./plan.css" />
<script src="./order.ts" />