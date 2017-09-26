<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="计划开单">

                <yd-cell-group title="请选择" style="padding-top: 20px">
                    <yd-cell-item>
                        <yd-radio-group slot="left" v-model="radio2">
                            <yd-radio val="1">水上</yd-radio>
                            <yd-radio val="2">陆上</yd-radio>
                            <yd-radio val="3">机油</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-cell-group :title="'单号：' + model.name">
                    <yd-cell-item>
                        <span slot="left">船号：</span>
                        <yd-input slot="right" v-model="model.carNo" required placeholder="请输入您的船号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item arrow @click.native="oilshow = true">
                        <span slot="left">油品：</span>
                        <span slot="right">{{model.oilName}}</span>                        
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">计划单价：</span>
                        <yd-input slot="right" v-model="model.price" regex="" placeholder="请输入单价"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">计划数量：</span>
                        <yd-input slot="right" v-model="model.count" regex="" placeholder="请输入加油数量"></yd-input>
                        <span slot="right" style="width:70px">单位：{{model.unit}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">当前余油：</span>
                        <yd-input slot="right" v-model="model.remainder" regex="" placeholder="请输入客户目前剩余油量"></yd-input>
                        <span slot="right" style="width:70px">单位：{{model.unit}}</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">加油时间：</span>
                        <yd-datetime type="date" v-model="oildate" slot="right"></yd-datetime>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="选填">
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
                        <yd-input slot="right" v-model="model.billingCompany" regex="" placeholder="请输入开票单位"></yd-input>
                        <span slot="right" style="width: 1.2rem"><yd-button type="warning" @click.native="getClients">导入</yd-button></span>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="model.billingPrice" regex="" placeholder="请输入开票单价，默认同上"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="model.billingCount" regex="" placeholder="请输入开票，默认同上"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary" @click.native="buttonclick" :disabled="isPrevent">提交</yd-button>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <weui-search v-model="sv" />
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                        <yd-cell-item slot="list" arrow v-for="s in salesplans" :key="s.id" @click.native="godetail(s.id)">
                            <div slot="left" style="line-height: .4rem;margin: 10px 0">
                                <p>{{s.carNo}} - <span style="color:forestgreen">￥{{s.totalMoney}}</span></p>
                                <p class="color_lightgray">{{s.oilName}}</p>
                                <p class="color_lightgray">{{s.price}} x {{s.count}}{{s.unit}}</p>
                            </div>
                            <div slot="right">
                                <p :class="classState(s.state)" style="padding-left:10px">{{getStateName(s.state)}}</p>
                                <p>{{formatDate(s.oilDate)}}</p>
                            </div>
                        </yd-cell-item>
                        <!-- 数据全部加载完毕显示 -->
                        <span slot="doneTip">没有数据啦~~</span>
                        <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                        <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                    </yd-infinitescroll>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-actionsheet :items="oiloptions" v-model="oilshow" cancel="取消"></yd-actionsheet>
    </div>
</template>

<style src="./plan.css" />
<script src="./plan.ts" />