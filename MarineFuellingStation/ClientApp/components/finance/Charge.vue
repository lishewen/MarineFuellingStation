<template>
    <div>
        <yd-grids-group :rows="2" :title="accName">
            <yd-grids-item>
                <div slot="text">
                    <p style="color: lightgray;font-size: .4rem">当前余额</p>
                    <p style="margin-top: .2rem;font-size: .4rem">￥{{isCompany? company.balances : client.balances}}</p>
                </div>
            </yd-grids-item>
            <yd-grids-item>
                <div slot="text">
                    <p style="color: lightgray;font-size: .4rem">挂账金额</p>
                    <p style="margin-top: .2rem;font-size: .4rem">￥[待完成]</p>
                </div>
            </yd-grids-item>
        </yd-grids-group>
        <yd-cell-group title="第一步：查询" style="margin-top: 10px">
            <yd-cell-item>
                <yd-radio-group slot="left" v-model="isCompany">
                    <yd-radio val="0">个人</yd-radio>
                    <yd-radio val="1">公司</yd-radio>
                </yd-radio-group>
            </yd-cell-item>
            <yd-cell-item>
                <yd-input slot="right" v-model="keyword" required placeholder="请输入船号/车号/联系人/手机号码"></yd-input>
                <yd-button slot="right" type="warning" @click.native="queryclick" style="width: 50px">查询</yd-button>
            </yd-cell-item>
        </yd-cell-group>
        <yd-cell-group title="第二步：支付方式与金额" v-show="showStep2">
            <yd-cell-item arrow type="label">
                <span slot="left">支付方式：</span>
                <select slot="right" v-model="chargeLog.payType">
                    <option value="-1">请选择</option>
                    <option value="0">现金</option>
                    <option value="1">微信</option>
                    <option value="2">支付宝</option>
                    <option value="3">桂行刷卡</option>
                    <option value="4">工行刷卡</option>
                </select>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">充值金额：</span>
                <yd-input slot="right" required placeholder="请输入" v-model="chargeLog.money" type="number"></yd-input>
                <span slot="right">元</span>
            </yd-cell-item>
        </yd-cell-group>
        <div v-show="showStep2">
            <yd-button size="large" type="primary" @click.native="chargeclick" :disabled="isPrevent">提交</yd-button>
        </div>
        <!--搜索返回的客户列表-->
        <yd-popup v-model="showClients" position="right" width="70%">
            <yd-cell-group title="搜索结果如下">
                <yd-cell-item arrow @click.native="clientclick(c)" v-for="c in clients" :key="c.id">
                    <div slot="left">
                        <p>{{c.carNo}}</p>
                        <p style="color: gray">{{c.createdBy}}</p>
                    </div>
                    <div slot="right">
                        <p></p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--搜索返回的公司列表-->
        <yd-popup v-model="showCompanys" position="right" width="70%">
            <yd-cell-group title="搜索结果如下">
                <yd-cell-item arrow @click.native="companyclick(co)" v-for="co in companys" :key="co.id">
                    <div slot="left">
                        <p>{{co.name}}</p>
                        <p style="color: gray">{{co.createdBy}}</p>
                    </div>
                    <div slot="right">
                        <p></p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
    </div>
</template>
<script src="./Charge.ts"></script>