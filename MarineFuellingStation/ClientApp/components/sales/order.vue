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
                        <div v-show="selectedplanNo == '无计划或散客'">
                            <yd-cell-item>
                                <yd-radio-group slot="left" v-model="type">
                                    <yd-radio val="0">水上</yd-radio>
                                    <yd-radio val="1">陆上</yd-radio>
                                </yd-radio-group>
                            </yd-cell-item>
                            <yd-cell-item>
                                <yd-radio-group slot="left" v-model="model.orderType">
                                    <yd-radio val="0" v-show="type == 0">水上加油</yd-radio>
                                    <yd-radio val="2" v-show="type == 0">机油</yd-radio>
                                    <yd-radio val="1" v-show="type == 1">陆上装车</yd-radio>
                                    <yd-radio val="4" v-show="type == 1">汇鸿车辆加油</yd-radio>
                                    <yd-radio val="5" v-show="type == 1">外来车辆加油</yd-radio>
                                </yd-radio-group>
                            </yd-cell-item>
                            <yd-cell-item>
                                <span slot="left">{{strCarOrBoat}}：</span>
                                <yd-input slot="right" v-model="model.carNo" required placeholder="请输入"></yd-input>
                            </yd-cell-item>
                        </div>
                    </yd-cell-group>
                    <yd-cell-group title="计划" v-show="hasplan">
                        <yd-cell-item>
                            <span slot="left">{{strCarOrBoat}}：</span>
                            <span slot="right">{{salesplan.carNo}}</span>
                        </yd-cell-item>
                        <yd-cell-item>
                            <span slot="left">计划单价：</span>
                            <span slot="right">￥{{salesplan.price}} / {{salesplan.unit}}</span>
                        </yd-cell-item>
                        <yd-cell-item>
                            <span slot="left">计划数量：</span>
                            <span slot="right">{{salesplan.count}}{{model.unit}}</span>
                        </yd-cell-item>
                        <yd-cell-item>
                            <span slot="right" style="font-weight: bold">总价：￥{{Math.round(salesplan.totalMoney)}}</span>
                        </yd-cell-item>
                    </yd-cell-group>
                    <yd-button size="large" @click.native="goStep2" :disabled="model.carNo == null || model.carNo == '' || (type == null && !hasplan)">下一步</yd-button>
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

                    <yd-cell-group title="水上施工信息" v-show="model.orderType == 0">
                        <yd-cell-item arrow @click.native="selectStoreclick">
                            <span slot="right">{{selectStoreText}}</span>
                        </yd-cell-item>
                        <yd-cell-item arrow @click.native="selectWorkerclick">
                            <span slot="right">{{selectWorkerText}}</span>
                        </yd-cell-item>
                    </yd-cell-group>

                    <div>
                        <yd-button size="large" type="primary" @click.native="buttonclick" :disabled="isPrevent">提交</yd-button>
                    </div>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <yd-search v-model="ordersv" />
                <div style="text-align: center;padding: 10px 0 10px">
                    <span v-for="(f, index) in filterOrderType">
                        <yd-button type="warning" v-if="f.actived" @click.native="switchOrderTypeBtn(f, index)">{{f.name}}</yd-button>
                        <yd-button type="hollow" v-if="!f.actived" @click.native="switchOrderTypeBtn(f, index)">{{f.name}}</yd-button>
                    </span>
                </div>
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                        <yd-cell-item slot="list" arrow v-for="o in orders" :key="o.id" @click.native="showMenuclick(o)">
                            <div slot="left" style="line-height: .4rem;margin: 10px 0">
                                <p>{{o.carNo}} - <span class="col-green">￥{{o.totalMoney}}</span></p>
                                <p class="col-light-gray">{{o.name}}</p>
                                <p class="col-light-gray">{{o.product.name}} / ￥{{o.price}} x {{o.count}}{{o.unit}}</p>
                            </div>
                            <div slot="right">
                                <p :class="classState(o.state)" style="padding-left:10px">{{strOrderState(o.state)}}</p>
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
        <!--popup选择计划单-->
        <yd-popup v-model="salesplanshow" position="right" width="70%">
            <yd-cell-group>
                <div class="align-center">
                    <yd-button style="width:90%;height: 38px; margin: 5px 0" type="primary" @click.native="emptyclick()">无计划或散客</yd-button>
                </div>
                <yd-search v-model="sv" />
                <div style="text-align: center;padding: 10px 0 10px">
                    <span v-for="(f, index) in filterCType">
                        <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index)">{{f.name}}</yd-button>
                        <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index)">{{f.name}}</yd-button>
                    </span>
                </div>
                <yd-infinitescroll :callback="loadList_sp" ref="spInfinitescroll">
                    <yd-cell-item slot="list" arrow @click.native="planitemclick(s)" v-for="s in salesplans" :key="s.id">
                        <div slot="left" style="line-height: 20px">
                            <p v-if="s.state == 0" class="col-coral">{{s.carNo}}</p>
                            <p v-if="s.state != 0" class="col-green">{{s.carNo}}</p>
                            <p class="col-light-gray">{{s.createdBy}}</p>
                        </div>
                        <div slot="right" style="line-height: 20px; margin: 5px 0">
                            <p>预约{{formatShortDate(s.oilDate)}}</p>
                            <p class="col-light-gray">开单{{formatShortDate(s.createdAt)}}</p>
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
        <!--popup销售仓选择-->
        <yd-popup v-model="showStores" position="right">
            <yd-cell-group title="请选择销售仓">
                <yd-cell-item v-for="s in stores" :key="s.id" @click.native="storeclick(s)">
                    <div slot="left">
                        <p>{{s.name}}</p>
                    </div>
                    <div slot="right">
                        <p class="col-light-gray">{{s.value}}</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--popup生产人员选择-->
        <yd-popup v-model="showWorkers" position="right">
            <yd-cell-group title="请选择生产人员">
                <yd-cell-item v-for="w,idx in workers" :key="idx" @click.native="workerclick(w)">
                    <div slot="left">
                        <p>{{w.name}}</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--popup生产人员选择-->
        <yd-popup v-model="showAddDelReason" position="right">
            <yd-cell-group title="请输入作废单据原因">
                <yd-cell-item>
                    <yd-textarea slot="right" v-model="delReason" placeholder="请输入本次作废单据原因" maxlength="100"></yd-textarea>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button size="large" type="primary" @click.native="delOrderclick" :disabled="delReason == null || delReason == ''">提交</yd-button>
        </yd-popup>
        <!--右滑菜单 end-->
        <!--打印菜单-->
        <yd-actionsheet :items="menus" v-model="showMenus" cancel="取消"></yd-actionsheet>
    </div>
</template>

<style src="./plan.css" />
<script src="./order.ts" />

