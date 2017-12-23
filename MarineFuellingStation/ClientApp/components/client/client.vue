<template>
    <div id="root">
        <yd-tab :callback="change">
            <yd-tab-panel label="添加客户">
                <yd-cell-group title="必填">
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

                    <yd-cell-item v-show="show1" arrow @click.native="showCompanys = true">
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
                        <yd-input slot="right" v-model="model.mobile" type="number" placeholder="请输入联系电话" required></yd-input>
                    </yd-cell-item>

                </yd-cell-group>
                <yd-cell-group title="选填">

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

                <yd-cell-group title="设置（选填）">

                    <yd-cell-item>
                        <span slot="left">最高挂账金额：</span>
                        <yd-input slot="right" v-model="model.maxOnAccount" regex="" placeholder="0 - 不指定" type="number"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>

                <yd-button size="large" type="primary" @click.native="addclientclick">提交</yd-button>
            </yd-tab-panel>
            <yd-actionsheet :items="oiloptions" v-model="oilshow" cancel="取消"></yd-actionsheet>
            <yd-tab-panel label="客户列表">
                <yd-cell-group>
                    <yd-search v-model="svClient" />
                    <div class="align-center cell-padding">
                        <span v-for="(f, index) in filterCType">
                            <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '客户类型')">{{f.name}}</yd-button>
                            <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '客户类型')">{{f.name}}</yd-button>
                        </span>
                        <span><yd-button type="hollow" @click.native="show2 = true">筛选</yd-button></span>
                    </div>
                    <yd-cell-item arrow v-for="c in clients" :key="c.id" @click.native="godetail(c)">
                        <div slot="left">
                            <p>{{c.carNo}} - {{c.contact}}</p>
                            <p v-if="c.company != null" class="col-light-gray font12">{{c.company.name}}</p>
                        </div>
                        <div slot="right" class="align-left" style="margin-right: 5px">
                            <p v-if="c.company != null" class="col-gray">余额：￥{{c.company.balances}}</p>
                            <p class="col-coral lineheight24">最近：{{formatDate(c.lastUpdatedAt)}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="公司列表">
                <yd-cell-group>
                    <div class="align-center">
                        <yd-button type="warning" @click.native="switchaddcompany" style="width: 90%; height: 38px; margin: .2rem 0">添加新公司</yd-button>
                    </div>
                    <yd-search v-model="svCompany" />
                    <yd-cell-item arrow v-for="co in companys" :key="co.id" @click.native="companyclick(co)">
                        <div slot="left">
                            <p>{{co.name}}</p>
                        </div>
                        <div slot="right" class="align-left" style="margin-right: 5px">
                            <p class="col-gray">余额：￥{{co.balances}}</p>
                            <p class="col-coral lineheight24">最近：{{formatDate(co.lastUpdatedAt)}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>

        <yd-popup v-model="show2" position="right" width="75%">
            <yd-grids-group :rows="3" title="单选：计划单">
                <yd-grids-item v-for="(f, index) in filterPType" :key="f.id" style="position: static; padding: .2rem">
                    <span slot="text">
                        <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '计划单')">{{f.name}}</yd-button>
                        <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '计划单')">{{f.name}}</yd-button>
                    </span>
                </yd-grids-item>
            </yd-grids-group>
            <yd-grids-group :rows="2" title="单选：账户余额">
                <yd-grids-item v-for="(f, index) in filterBalances" :key="f.id" style="position: static; padding: .2rem">
                    <span slot="text">
                        <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '账户余额')">{{f.name}}</yd-button>
                        <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '账户余额')">{{f.name}}</yd-button>
                    </span>
                </yd-grids-item>
            </yd-grids-group>
            <yd-grids-group :rows="2" title="单选：周期">
                <yd-grids-item v-for="(f, index) in filterCycle" :key="f.id" style="position: static; padding: .2rem">
                    <span slot="text">
                        <yd-button style="box-sizing: inherit" type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '周期')">{{f.name}}</yd-button>
                        <yd-button style="box-sizing: inherit" type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '周期')">{{f.name}}</yd-button>
                    </span>
                </yd-grids-item>
            </yd-grids-group>
            <div style="text-align: center;margin-top: .2rem">
                <yd-button size="large" type="primary" @click.native="filterclick()">提交</yd-button>
            </div>
        </yd-popup>
        <!--popup新增公司-->
        <yd-popup v-model="showCompanyInput" position="right" width="70%">
            <yd-cell-group title="必填">
                <yd-cell-item>
                    <span slot="left">名称：</span>
                    <yd-input slot="right" v-model="modelCompany.name" required placeholder="请输入"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <yd-cell-group title="选填">
                <yd-cell-item>
                    <span slot="left">电话：</span>
                    <yd-input slot="right" v-model="modelCompany.phone" regex="mobile" placeholder="请输入"></yd-input>
                </yd-cell-item>
                <yd-cell-item arrow>
                    <span slot="left">票类：</span>
                    <select slot="right">
                        <option value="">请选择票类</option>
                        <option value="0">循票</option>
                        <option value="1">柴票</option>
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
            </yd-cell-group>
            <yd-button v-show="isAddAction" size="large" type="primary" @click.native="addcompanyclick">提交</yd-button>
            <yd-button v-show="!isAddAction" size="large" type="primary" @click.native="savecompanyclick">保存</yd-button>
        </yd-popup>
        <!--popup公司选择列表-->
        <yd-popup v-model="showCompanys" position="right" width="70%">
            <yd-cell-group>
                <div class="align-center"><yd-button type="primary" style="width: 90%" @click.native="switchaddcompany">新增</yd-button></div>
                <yd-search v-model="svCompany1" />
                <yd-cell-item arrow type="radio" v-for="co in companys" :key="co.id" @click.native="selectcompanyclick(co)">
                    <span slot="left">{{co.name}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--popup公司客户成员列表-->
        <yd-popup v-model="showMyClients" position="right" width="70%">
            <yd-cell-group :title="modelCompany.name">
                <yd-cell-item type="checkbox" v-for="c in modelCompany.clients" :key="c.id">
                    <span slot="left">{{c.carNo}}</span>
                    <input slot="right" type="checkbox" :value="c.id" v-model="selectClientIds" />
                </yd-cell-item>
            </yd-cell-group>
            <yd-button size="large" :disabled="selectClientIds.length < 1" v-show="modelCompany.clients != null && modelCompany.clients.length > 0" @click.native="putRemoveCompanyClients">移除所选成员</yd-button>
            <yd-button size="large" @click.native="showSearchInput = true">添加成员</yd-button>
        </yd-popup>
        <!--popup添加成员时客户搜索输入-->
        <yd-popup v-model="showSearchInput" position="right" width="70%">
            <yd-cell-group title="查询添加">
                <yd-cell-item>
                    <yd-input slot="right" placeholder="请输入船号/车号/手机号/联系人" v-model="svClient1"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button size="large" @click.native="getClientsByKw">查询</yd-button>
        </yd-popup>
        <!--popup搜索客户列表-->
        <yd-popup v-model="showSearchResult" position="right" width="70%">
            <yd-cell-group title="查询结果如下：">
                <yd-cell-item type="checkbox" v-for="c in searchClients" :key="c.id">
                    <span slot="left">{{c.carNo}}</span>
                    <input slot="right" type="checkbox" :value="c.id" v-model="selectClientIds1" />
                </yd-cell-item>
            </yd-cell-group>
            <yd-button size="large":disabled="selectClientIds1 == null || selectClientIds1.length == 0" @click.native="putAddCompanyClients">确认添加</yd-button>
        </yd-popup>
        <!--popup销售列表-->
        <yd-popup v-model="showsales" position="right">
            <yd-cell-group title="必填">
                <yd-cell-item arrow type="radio" v-for="s in sales" :key="s.userid" @click.native="selectsalesclick(s)">
                    <span slot="left">{{s.name}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--点击公司action sheet -->
        <yd-actionsheet :items="menuoptions" v-model="menushow" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./client.ts" />