<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="销售开单">

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
                        <span slot="right" style="width:70px">单位：{{model.unit}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">订单数量：</span>
                        <yd-input slot="right" v-model="model.count" required placeholder="请输入加油数量"></yd-input>
                        <span slot="right" style="width:70px">单位：{{model.unit}}</span>
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
                    <yd-cell-item arrow v-show="model.isInvoice">
                        <span slot="left">票类：</span>
                        <select slot="right" v-model="model.ticketType">
                            <option value="-1">请选择票类</option>
                            <option value="0">普通票</option>
                            <option value="1">专用票</option>
                        </select>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">开票单位：</span>
                        <yd-input slot="right" v-model="model.billingCompany" placeholder="请输入开票单位"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="model.billingPrice" placeholder="请输入开票单价，默认同上"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
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

                <div>
                    <yd-button size="large" type="primary" @click.native="buttonclick">提交</yd-button>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <weui-search v-model="sv" />
                <yd-cell-group>
                    <yd-cell-item arrow v-for="o in orders" :key="o.id" @click.native="godetail(o.id)">
                        <span slot="left">{{o.carNo}}</span>
                        <span slot="left" class="color_lightgray" style="margin-left:10px">{{o.oilName}} {{o.count}}{{o.unit}} ￥{{o.totalMoney}}</span>
                        <span slot="right">{{formatDate(o.oilDate)}}</span>
                        <span slot="right" :class="classState(o.state)" style="padding-left:10px">{{getStateName(o.state)}}</span>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-actionsheet :items="oiloptions" v-model="oilshow" cancel="取消"></yd-actionsheet>
        <!--右滑菜单-->
        <yd-popup v-model="salesplanshow" position="right">
            <yd-cell-group>
                <div style="text-align: center">
                    <yd-button style="width:80%;margin:10px 0 10px 0" type="primary" @click.native="emptyclick()">散客</yd-button>
                </div>
                <weui-search v-model="sv" />
                <yd-cell-item arrow @click.native="planitemclick(s)" v-for="s in salesplans" :key="s.id">
                    <span slot="left">{{s.carNo}}</span>
                    <span slot="left" style="color:lightgray;margin-left:10px">{{s.createdBy}}</span>
                    <span slot="right">{{formatShortDate(s.oilDate)}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
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
        <!--右滑菜单 end-->
    </div>
</template>

<style src="./plan.css" />
<script src="./order.ts" />