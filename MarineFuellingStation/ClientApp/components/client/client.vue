<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="添加客户">
                <yd-cell-group title="必填" style="margin-top:20px">

                    <yd-cell-item>
                        <yd-radio-group slot="left" v-model="model.placeType">
                            <yd-radio val="0">陆上</yd-radio>
                            <yd-radio val="1">水上</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>

                    <yd-cell-item>
                        <yd-radio-group slot="left" v-model="model.clientType">
                            <yd-radio val="0">个人</yd-radio>
                            <yd-radio val="1">公司</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>

                    <yd-cell-item v-show="show1" arrow @click.native="showcompany = true">
                        <span slot="left">所属公司：</span>
                        <span slot="right">{{companyName}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">{{labelBoatOrCar}}：</span>
                        <yd-input slot="right" placeholder="请输入" v-model="model.carNo" required></yd-input>
                    </yd-cell-item>

                    <yd-cell-item arrow @click.native="showsales = true">
                        <span slot="left">跟进销售：</span>
                        <span slot="right">{{model.followSalesman}}</span>
                    </yd-cell-item>

                    <yd-cell-item arrow @click.native="oilshow = true">
                        <span slot="left">默认商品：</span>
                        <span slot="right">{{oilName}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">联系人：</span>
                        <yd-input slot="right" v-model="model.contact" regex="" placeholder="请输入联系人" required></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">联系电话：</span>
                        <yd-input slot="right" v-model="model.mobile" regex="" placeholder="请输入联系电话" required></yd-input>
                    </yd-cell-item>

                </yd-cell-group>
                <yd-cell-group title="选填" style="margin-top:10px">

                    <yd-cell-item>
                        <span slot="left">身份证号：</span>
                        <yd-input slot="right" v-model="model.idCard" regex="" placeholder="请输入身份证号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">地址：</span>
                        <yd-input slot="right" v-model="model.address" regex="" placeholder="请输入地址"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">固定电话：</span>
                        <yd-input slot="right" v-model="model.phone" regex="" type="number" placeholder="请输入固定电话"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>

                <yd-cell-group title="设置（选填）" style="margin-top:10px">

                    <yd-cell-item>
                        <span slot="left">最高挂账金额：</span>
                        <yd-input slot="right" v-model="model.maxOnAccount" regex="" placeholder="0 - 不指定"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>

                <yd-cell-group title="不显示" style="margin-top:10px" v-show="nonshow">
                    <yd-cell-item>
                        <span slot="left">●账户金额：</span>
                        <yd-input slot="right" regex="" placeholder="收银流程充值自动计算"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●登记时间：</span>
                        <yd-input slot="right" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●最近消费时间：</span>
                        <yd-input slot="right" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●总消费金额：</span>
                        <yd-input slot="right" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●跟进销售：</span>
                        <yd-input slot="right" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary" @click.native="addclientclick">提交</yd-button>
                </div>
            </yd-tab-panel>
            <yd-actionsheet :items="oiloptions" v-model="oilshow" cancel="取消"></yd-actionsheet>
            <yd-tab-panel label="客户列表">
                <yd-grids-group :rows="4">
                    <yd-grids-item>
                        <p slot="text">全部</p>
                    </yd-grids-item>
                    <yd-grids-item>
                        <p slot="text">个人</p>
                    </yd-grids-item>
                    <yd-grids-item>
                        <p slot="text">公司</p>
                    </yd-grids-item>
                    <yd-grids-item @click.native="show2 = true">
                        <p slot="text">筛选</p>
                    </yd-grids-item>
                </yd-grids-group>

                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow v-for="c in clients" :key="c.id">
                        <div slot="left">
                            <p>{{c.carNo}} - {{c.contact}}</p>
                            <p style="color:lightgray;font-size:12px">{{c.company.name}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">余额：￥{{c.company.balances}}</p>
                            <p style="color:lightcoral;line-height: 25px">最近：{{c.lastUpdatedAt}}</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>船0002 【张三】</p>
                            <p style="color:lightgray;font-size:12px">广西XXXX有限公司</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">余额：￥20000</p>
                            <p style="color:lightcoral;line-height: 25px">最近：2017-07-07</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>船0002 【张三】</p>
                            <p style="color:lightgray;font-size:12px">个人</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:lightgray">无余额</p>
                            <p style="color:lightcoral;line-height: 25px">最近：2017-07-07</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>船0002 【张三】</p>
                            <p style="color:lightgray;font-size:12px">个人</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">余额：￥156</p>
                            <p style="color:lightcoral;line-height: 25px">最近：2017-07-07</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="公司列表">
                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>广西XXXX有限公司</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">余额：￥20000</p>
                            <p style="color:lightcoral;line-height: 25px">最近：2017-07-07</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>广西XXXX有限公司</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">余额：￥20000</p>
                            <p style="color:lightcoral;line-height: 25px">最近：2017-07-07</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>

        <yd-popup v-model="show2" position="right">
            <yd-cell-group title="单选：计划单" style="margin-top:20px">
                <yd-flexbox style="line-height: 60px">
                    <yd-flexbox-item style="text-align: center"><yd-button type="warning">已计划</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center"><yd-button type="hollow">已完成</yd-button></yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item style="text-align: center; margin-bottom: 20px"><yd-button type="hollow">已审批</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center; margin-bottom: 20px"></yd-flexbox-item>
                </yd-flexbox>
            </yd-cell-group>
            <yd-cell-group title="单选：账户余额">
                <yd-flexbox style="line-height: 60px">
                    <yd-flexbox-item style="text-align: center"><yd-button type="warning">少于1000</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center"><yd-button type="hollow">少于10000</yd-button></yd-flexbox-item>
                </yd-flexbox>
            </yd-cell-group>
            <yd-cell-group title="单选：周期">
                <yd-flexbox style="line-height: 60px">
                    <yd-flexbox-item style="text-align: center"><yd-button type="warning">7天不计划</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center"><yd-button type="hollow">15天不计划</yd-button></yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item style="text-align: center; margin-bottom: 20px"><yd-button type="hollow">30天不计划</yd-button></yd-flexbox-item>
                    <yd-flexbox-item style="text-align: center; margin-bottom: 20px"><yd-button type="hollow">90天不计划</yd-button></yd-flexbox-item>
                </yd-flexbox>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="filterclick()">提交</yd-button>
            </div>
        </yd-popup>
        <!--popup新增公司-->
        <yd-popup v-model="showaddcompany" position="right" width="70%">
            <yd-cell-group title="必填">
                <yd-cell-item>
                    <span slot="left">名称：</span>
                    <yd-input slot="right" v-model="modelCompany.name" regex="" placeholder="请输入"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <yd-cell-group title="选填">
                <yd-cell-item arrow>
                    <span slot="left">票类：</span>
                    <select slot="right">
                        <option value="">请选择票类</option>
                        <option value="1">普通票</option>
                        <option value="2">专用票</option>
                    </select>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">抬头：</span>
                    <yd-input slot="right" v-model="modelCompany.invoiceTitle" regex="" placeholder="开票抬头"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">税号：</span>
                    <yd-input slot="right" v-model="modelCompany.taxFileNumber" regex="" type="number" placeholder="请输入"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">账户：</span>
                    <yd-input slot="right" v-model="modelCompany.businessAccount" type="number" placeholder="对公账户"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">银行：</span>
                    <yd-input slot="right" v-model="modelCompany.bank" regex="" placeholder="开户银行"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">地址：</span>
                    <yd-input slot="right" v-model="modelCompany.address" regex="" placeholder="请输入"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">电话：</span>
                    <yd-input slot="right" v-model="modelCompany.phone" regex="mobile" placeholder="请输入"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="addcompanyclick">提交</yd-button>
            </div>
        </yd-popup>
        <!--popup公司选择列表-->
        <yd-popup v-model="showcompany" position="right" width ="70%">
            <yd-cell-group>
                <div style="text-align: center;"><yd-button type="primary" style="width: 90%" @click.native="switchaddcompany">新增</yd-button></div>
                <weui-search v-model="sv" />
                <yd-cell-item arrow type="radio" v-for="co in companys" :key="co.id" @click.native="selectcompanyclick(co)">
                    <span slot="left">{{co.name}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--popup销售列表-->
        <yd-popup v-model="showsales" position="right">
            <yd-cell-group title="必填">
                <weui-search v-model="sv" />
                <yd-cell-item arrow type="radio" v-for="s in sales" :key="s.id" @click.native="selectsalesclick(s)">
                    <span slot="left">{{s.name}}</span>
                    <span slot="right" style="font-size:12px;color:lightgray">{{s.clientcount}}客户</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
    </div>
</template>

<script src="./client.ts" />