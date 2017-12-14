<template>
    <div id="root">
        <yd-tab :callback="change">
            <yd-tab-panel label="销售开单">
                <!--第一步-->
                <div v-show="showStep1">
                    <yd-cell-group :title="strCarOrBoat + '：' + model.carNo">
                        <yd-cell-item arrow @click.native="salesplanselect">
                            <span slot="left">计划单：</span>
                            <span slot="right">{{selectedplanNo}}</span>
                        </yd-cell-item>
                        <yd-cell-item v-show="selectedplanNo == '无计划或散客'">
                            <yd-radio-group slot="left" v-model="radio2">
                                <yd-radio val="0">水上</yd-radio>
                                <yd-radio val="1">陆上</yd-radio>
                                <yd-radio val="2">机油</yd-radio>
                            </yd-radio-group>
                        </yd-cell-item>
                        <yd-cell-item v-show="selectedplanNo == '无计划或散客'">
                            <span slot="left">{{strCarOrBoat}}：</span>
                            <yd-input slot="right" v-model="model.carNo" required placeholder="请输入"></yd-input>
                        </yd-cell-item>
                    </yd-cell-group>
                    <yd-cell-group title="计划" v-show="hasplan">
                        <yd-cell-item>
                            <span slot="left">{{strCarOrBoat}}：</span>
                            <span slot="right">{{salesplan.carNo}}</span>
                        </yd-cell-item>
                        <yd-cell-item>
                            <span slot="left">计划单价：</span>
                            <span slot="right">{{salesplan.price}}</span>
                        </yd-cell-item>
                        <yd-cell-item>
                            <span slot="left">计划数量：</span>
                            <span slot="right">{{salesplan.count}}{{model.unit}}</span>
                        </yd-cell-item>
                        <yd-cell-item>
                            <span slot="right" style="font-weight: bold">总价：￥{{Math.round(salesplan.totalMoney)}}</span>
                        </yd-cell-item>
                    </yd-cell-group>
                    <yd-button size="large" @click.native="goStep2" :disabled="model.carNo == null || model.carNo == ''">下一步</yd-button>
                </div>
                <!--第二步-->
                <yd-cell-group title="客户信息" v-show="showStep2">
                    <yd-cell-item>
                        <span slot="left">个人账户：</span>
                        <span slot="right">￥{{client != null ? client.balances : ""}}</span>
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
                <!--第三步-->
                <div v-show="showStep3">
                    <yd-cell-group :title="'单号：' + model.name">
                        <yd-cell-item v-show="selectedplanNo != '无计划或散客'" arrow @click.native="showSalesmansclick">
                            <span slot="left">销售员：</span>
                            <span slot="right">{{model.salesman}}</span>
                        </yd-cell-item>

                        <yd-cell-item arrow @click.native="oilshow = true">
                            <span slot="left">商品：</span>
                            <span slot="right">{{oilName}}</span>
                        </yd-cell-item>

                        <yd-cell-item>
                            <span slot="left">订单单价：</span>
                            <yd-input slot="right" type="number" v-model="model.price" required :placeholder="strMinPriceTip()"></yd-input>
                        </yd-cell-item>

                        <yd-cell-item>
                            <span slot="left">订单数量：</span>
                            <yd-input slot="right" type="number" v-model="model.count" required placeholder="请输入加油数量"></yd-input>
                            <span slot="right" style="width:70px">单位：{{model.unit}}</span>
                        </yd-cell-item>

                        <yd-cell-item>
                            <span slot="right" style="font-weight: bold">总计：￥{{Math.round(model.totalMoney)}}</span>
                        </yd-cell-item>

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
                            <yd-input slot="right" v-model="model.billingCompany" placeholder="请输入单位"></yd-input>
                            <span slot="right" style="width: 1.2rem"><yd-button type="warning" @click.native="getClients">导入</yd-button></span>
                        </yd-cell-item>
                        <yd-cell-item v-show="model.isInvoice">
                            <span slot="left">单价：</span>
                            <yd-input slot="right" v-model="model.billingPrice" type="number" placeholder="请输入单价，默认同上"></yd-input>
                        </yd-cell-item>
                        <yd-cell-item v-show="model.isInvoice">
                            <span slot="left">数量：</span>
                            <yd-input slot="right" v-model="model.billingCount" type="number" placeholder="请输入，默认同上"></yd-input>
                        </yd-cell-item>
                    </yd-cell-group>

                    <yd-cell-group title="送货单选项" v-show="model.orderType == 1">
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
                <yd-search v-model="ordersv" />
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                        <yd-cell-item slot="list" arrow v-for="o in orders" :key="o.id" @click.native="showMenuclick(o)">
                            <div slot="left" style="line-height: .4rem;margin: 10px 0">
                                <p>{{o.carNo}} - <span class="col-green">￥{{o.totalMoney}}</span></p>
                                <p class="col-light-gray">{{o.name}}</p>
                                <p class="col-light-gray">{{o.product.name}} / ￥{{o.price}} x {{o.count}}{{o.unit}}</p>
                            </div>
                            <div slot="right">
                                <p :class="classState(o.state)" style="padding-left:10px">{{getStateName(o.state)}}</p>
                                <p>{{formatDate(o.oilDate)}}</p>
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
        <!--选择计划单-->
        <yd-popup v-model="salesplanshow" position="right" width="70%">
            <yd-cell-group>
                <div class="align-center">
                    <yd-button style="width:80%;margin:10px 0 10px 0" type="primary" @click.native="emptyclick()">无计划或散客</yd-button>
                </div>
                <yd-search v-model="sv" />
                <yd-infinitescroll :callback="loadList_sp" ref="spInfinitescroll">
                    <yd-cell-item slot="list" arrow @click.native="planitemclick(s)" v-for="s in salesplans" :key="s.id">
                        <div slot="left">
                            <p>{{s.carNo}}</p>
                            <p class="col-gray">{{s.createdBy}}</p>
                        </div>
                        <div slot="right">
                            <p :class="s.state == 0 ? 'color_red' : 'color_green'">{{strPlanState(s)}}</p>
                            <p>预约{{formatShortDate(s.oilDate)}}</p>
                        </div>
                    </yd-cell-item>
                    <!-- 数据全部加载完毕显示 -->
                    <span slot="doneTip">没有数据啦~~</span>
                    <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                    <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                </yd-infinitescroll>
            </yd-cell-group>
        </yd-popup>
        <!--popup销售列表-->
        <yd-popup v-model="showSalesmans" position="right">
            <yd-cell-group title="必填">
                <yd-cell-item arrow type="radio" v-for="s in sales" :key="s.userid" @click.native="selectsalesclick(s)">
                    <span slot="left">{{s.name}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--右滑菜单 end-->
        <!--打印菜单-->
        <yd-actionsheet :items="menus" v-model="showMenus" cancel="取消"></yd-actionsheet>
    </div>
</template>

<style src="./plan.css" />
<script src="./order.ts" />

