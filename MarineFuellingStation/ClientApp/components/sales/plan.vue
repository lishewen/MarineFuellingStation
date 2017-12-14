<template>
    <div id="root">
        <yd-tab :callback="change">
            <yd-tab-panel label="计划开单">
                <yd-cell-group :title="'单号：' + model.name" v-show="showStep1">
                    <yd-cell-item>
                        <span slot="left">{{strCarOrBoat}}：</span>
                        <yd-input slot="right" v-model="model.carNo" required placeholder="请输入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">预计加油时间：</span>
                        <yd-datetime type="date" v-model="oildate" slot="right"></yd-datetime>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-button type="primary" @click.native="goStep2" size="large" :disabled="model.carNo == null || model.carNo == ''" v-show="showStep1">下一步</yd-button>
                <yd-cell-group title="客户信息" v-show="showStep2">
                    <yd-cell-item>
                        <span slot="left">个人账户：</span>
                        <span slot="right">￥{{client != null ? client.balances : ""}}</span>
                    </yd-cell-item>
                    <yd-cell-item v-show="isShowCompanyAccount()">
                        <span slot="left">{{strCompanyName()}}账户：</span>
                        <span slot="right">￥{{strCompanyBalances()}}</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">联系人：</span>
                        <div slot="right" v-show="client.contact && client.contact != ''">
                            <span slot="right">{{client.contact}}</span>
                            <yd-button type="warning" style="width: 35px" @click.native="client.contact = ''">变更</yd-button>
                        </div>
                        <yd-input slot="right" v-model="contact" ref="contact" v-show="!client.contact" required placeholder="请完善联系人资料" />
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">手机：</span>
                        <div slot="right" v-show="client.mobile && client.mobile != ''">
                            <span>{{client.mobile}}</span>
                            <yd-button type="warning" style="width: 35px" @click.native="client.mobile = ''">变更</yd-button>
                        </div>
                        <yd-input slot="right" v-model="mobile" ref="mobile" v-show="!client.mobile" type="number" regex="mobile" required placeholder="请完善11位手机资料" />
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">固定电话：</span>
                        <span slot="right">{{client ? client.phone : ""}}</span>
                        <yd-input slot="right" v-show="client && !client.phone" type="number" placeholder="请完善固定电话资料，选填" />
                    </yd-cell-item>
                    <yd-button size="large" @click.native="goStep3" :disabled="step3Prevent">下一步</yd-button>
                </yd-cell-group>
                <div v-show="showStep3">
                    <yd-cell-group title="请选择">
                        <yd-cell-item>
                            <yd-radio-group slot="left" v-model="model.salesPlanType">
                                <yd-radio val="0" :disabled="!isWaterDept">水上</yd-radio>
                                <yd-radio val="2" :disabled="!isWaterDept">机油</yd-radio>
                                <yd-radio val="1" :disabled="isWaterDept">陆上</yd-radio>
                                <yd-radio val="4" :disabled="isWaterDept">陆上公司车</yd-radio>
                                <yd-radio val="5" :disabled="isWaterDept">陆上外来车</yd-radio>
                            </yd-radio-group>
                        </yd-cell-item>
                    </yd-cell-group>
                    <yd-cell-group title="请输入">
                        <yd-cell-item arrow @click.native="oilshow = true">
                            <span slot="left">油品：</span>
                            <span slot="right">{{model.oilName}}</span>
                        </yd-cell-item>

                        <yd-cell-item>
                            <span slot="left">计划单价：</span>
                            <yd-input slot="right" v-model="model.price" required :placeholder="strMinPriceTip()"></yd-input>
                        </yd-cell-item>

                        <yd-cell-item>
                            <span slot="left">计划数量：</span>
                            <yd-input slot="right" v-model="model.count" required placeholder="请输入数量"></yd-input>
                            <span slot="right">{{model.unit}}</span>
                        </yd-cell-item>

                        <yd-cell-item>
                            <span slot="right" style="font-weight: bold">总计：￥{{Math.round(model.price * model.count)}}</span>
                        </yd-cell-item>

                        <!--<yd-cell-item>
                <span slot="left">当前余油：</span>
                <yd-input slot="right" v-model="model.remainder" regex="" placeholder="请输入客户目前剩余油量"></yd-input>
                <span slot="right" style="width:70px">单位：{{model.unit}}</span>
            </yd-cell-item>-->
                        <yd-cell-item>
                            <span slot="left">备注：</span>
                            <yd-textarea slot="right" v-model="model.remark" placeholder="请输入备注信息" maxlength="200"></yd-textarea>
                        </yd-cell-item>
                        <yd-cell-item>
                            <span slot="left">代码信息</span>
                            <span slot="right">
                                <yd-switch v-model="model.isInvoice"></yd-switch>
                            </span>
                        </yd-cell-item>
                        <yd-cell-item arrow v-show="model.isInvoice">
                            <span slot="left">必选：</span>
                            <select slot="right" v-model="model.ticketType">
                                <option value="-1">请选择</option>
                                <option value="0">循</option>
                                <option value="1">柴</option>
                            </select>
                        </yd-cell-item>
                        <yd-cell-item v-show="model.isInvoice">
                            <span slot="left">单位：</span>
                            <yd-input slot="right" v-model="model.billingCompany" regex="" placeholder="请输入单位"></yd-input>
                        </yd-cell-item>
                        <yd-cell-item v-show="model.isInvoice">
                            <span slot="left">单价：</span>
                            <yd-input slot="right" v-model="model.billingPrice" regex="" placeholder="请输入开出单价，默认同上"></yd-input>
                        </yd-cell-item>
                        <yd-cell-item v-show="model.isInvoice">
                            <span slot="left">数量：</span>
                            <yd-input slot="right" v-model="model.billingCount" regex="" placeholder="请输入开出数量，默认同上"></yd-input>
                        </yd-cell-item>
                    </yd-cell-group>

                    <yd-cell-group title="【陆上】送货单选项" v-show="showStep3 && model.salesPlanType == 1">
                        <yd-cell-item>
                            <span slot="left">送货上门</span>
                            <span slot="right"><yd-switch v-model="model.isDeliver"></yd-switch></span>
                        </yd-cell-item>
                        <yd-cell-item v-show="model.isDeliver">
                            <span slot="left">运费：</span>
                            <yd-input slot="right" v-model="model.deliverMoney" type="number" placeholder="请输入运费"></yd-input>
                            <span slot="right">元</span>
                        </yd-cell-item>
                        <yd-cell-item v-show="model.isDeliver">
                            <span slot="left">打印单价</span>
                            <span slot="right"><yd-switch v-model="model.isPrintPrice"></yd-switch></span>
                        </yd-cell-item>
                    </yd-cell-group>

                    <div>
                        <yd-button size="large" type="primary" @click.native="buttonclick" :disabled="isPrevent">提交</yd-button>
                    </div>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <yd-search v-model="sv" />
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                        <yd-cell-item slot="list" arrow v-for="s in salesplans" :key="s.id" @click.native="godetail(s.id)">
                            <div slot="left" style="line-height: .4rem;margin: 10px 0">
                                <p>{{s.carNo}} - <span class="col-green">￥{{s.totalMoney}}</span></p>
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
        <!--修改商品限价-->
        <yd-popup v-model="showPd" position="right" width="70%">
            <yd-cell-group title="限价">
                <yd-cell-item v-for="p in products" :key="p.id">
                    <span slot="left">{{p.name}}：</span>
                    <yd-input slot="right" v-model="p.minPrice" required></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
            </yd-cell-group>
            <yd-cell-group title="开票限价">
                <yd-cell-item v-for="p in products" :key="p.id">
                    <span slot="left">{{p.name}}：</span>
                    <yd-input slot="right" v-model="p.minInvoicePrice" required></yd-input>
                    <span slot="right">元</span>
                </yd-cell-item>
            </yd-cell-group>
            
            <div class="align-center">
                <yd-button type="primary" @click.native="prodsaveclick" style="width: 90%">保存</yd-button>
            </div>
        </yd-popup>
        <yd-actionsheet :items="oiloptions" v-model="oilshow" cancel="取消"></yd-actionsheet>
    </div>
</template>

<style src="./plan.css" />

<script src="./plan.ts" />
