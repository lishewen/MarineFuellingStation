<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="开单">
                <yd-cell-group :title="'单号：' + model.name">

                    <yd-cell-item>
                        <span slot="left">船号：</span>
                        <yd-input slot="right" v-model="model.carNo" required placeholder="请输入您的船号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">公司名称：</span>
                        <yd-input slot="right" v-model="model.company" regex="" placeholder="请输入公司名称"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">金额：</span>
                        <yd-input slot="right" v-model="model.money" required placeholder="请输入金额"></yd-input>
                    </yd-cell-item>
                        
                    <yd-cell-item arrow @click.native="showWorkers = true">
                        <span slot="left">施工人员：</span>
                        <span slot="right">{{selectedworker}}</span>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="选填">

                    <yd-cell-item>
                        <span slot="left">航次：</span>
                        <yd-input slot="right" v-model="model.voyage" regex="" placeholder="请输入航次"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">船舶总吨：</span>
                        <yd-input slot="right" v-model="model.tonnage" regex="" placeholder="请输入船舶总吨"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">批准书文号：</span>
                        <yd-input slot="right" v-model="model.responseId" regex="" placeholder="请输入批准书文号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">作业地点：</span>
                        <yd-input slot="right" v-model="model.address" regex="" placeholder="请输入作业地点"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">联系电话：</span>
                        <yd-input slot="right" v-model="model.phone" regex="" placeholder="请输入联系电话"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="right">
                            <yd-switch v-model="model.isInvoice"></yd-switch>
                        </span>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">单位：</span>
                        <yd-input slot="right" v-model="model.billingCompany" regex="" placeholder="请输入单位"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="model.billingPrice" regex="" placeholder="请输入单价，默认同上"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="model.isInvoice">
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="model.billingCount" regex="" placeholder="请输入，默认同上"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary" @click.native="buttonclick" :disabled="isPrevent">提交</yd-button>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <yd-search v-model="sv" />
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                        <yd-cell-item slot="list" arrow v-for="s in boatCleans" :key="s.id" style="padding: 10px 0 10px 0" @click.native="itemclick(s)">
                            <div slot="left">
                                <p>{{s.carNo}}</p>
                                <p class="col-gray" style="margin-top: 5px">{{s.name}}</p>
                                <p class="col-light-gray">{{s.createdBy}}</p>
                            </div>
                            <div slot="right">
                                <p>{{formatDate(s.oilDate)}}</p>
                                <p :class="classState(s.state)">{{getStateName(s.state)}}</p>
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
        <!--明细-->
        <yd-popup v-model="showDetail" position="right" width="80%">
            <yd-cell-group>
                <yd-cell-item>
                    <span slot="left">船号：</span>
                    <span slot="right">{{bc.carNo}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">公司名称：</span>
                    <span slot="right">{{bc.company}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">金额：</span>
                    <span slot="right">{{bc.money}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">施工人员：</span>
                    <span slot="right">{{bc.worker}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">航次：</span>
                    <span slot="right">{{bc.voyage}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">吨位：</span>
                    <span slot="right">{{bc.tonnage}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">批文号：</span>
                    <span slot="right">{{bc.responseId}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">作业地点：</span>
                    <span slot="right">{{bc.address}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">联系电话：</span>
                    <span slot="right">{{bc.phone}}</span>
                </yd-cell-item>
                <yd-cell-item v-show="bc.isInvoice">
                    <span slot="right">代号信息</span>
                </yd-cell-item>
                <yd-cell-item v-show="bc.isInvoice">
                    <span slot="left">单位：</span>
                    <span slot="right">{{bc.billingCompany}}</span>
                </yd-cell-item>
                <yd-cell-item v-show="bc.isInvoice">
                    <span slot="left">单价：</span>
                    <span slot="right">{{bc.billingPrice}}</span>
                </yd-cell-item>
                <yd-cell-item v-show="bc.isInvoice">
                    <span slot="left">数量：</span>
                    <span slot="right">{{bc.billingCount}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--popup生产人员列表-->
        <yd-popup v-model="showWorkers" position="right">
            <yd-cell-group title="必填">
                <yd-cell-item arrow type="radio" v-for="w in workers" :key="w.userid" @click.native="selectworkerclick(w)">
                    <span slot="left">{{w.name}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
    </div>
</template>

<style src="./plan.css" />
<script src="./boatclean.ts"/>

