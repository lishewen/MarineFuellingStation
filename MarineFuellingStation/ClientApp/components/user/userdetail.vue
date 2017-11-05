<template>
    <div id="root">
        <yd-cell-group title="基本资料（必填）" class="first-group">
            <yd-cell-item>
                <span slot="left">性别：</span>
                <yd-radio-group slot="left" v-model="model.workInfo.gender">
                    <yd-radio val="1" disabled>男</yd-radio>
                    <yd-radio val="2" disabled>女</yd-radio>
                </yd-radio-group>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">部门：</span>
                <span slot="right">{{departmentdict[model.workInfo.department[0]]}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">职位：</span>
                <yd-radio-group slot="left" v-model="model.workInfo.isleader">
                    <yd-radio val="0" disabled>普通职员</yd-radio>
                    <yd-radio val="1" disabled>上级</yd-radio>
                </yd-radio-group>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">姓名：</span>
                <yd-input slot="right" disabled v-model="model.workInfo.name" regex="" placeholder="请输入姓名"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">电话：</span>
                <yd-input slot="right" disabled v-model="model.workInfo.mobile" regex="" placeholder="请输入电话"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">基本工资：</span>
                <yd-input slot="right" type="number" v-model="model.localInfo.baseWage" placeholder="请输入基本工资"></yd-input>
                <span slot="right">元</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">社保自付：</span>
                <yd-input slot="right" type="number" v-model="model.localInfo.socialSecurity" regex="" placeholder="请输入社保自付金额"></yd-input>
                <span slot="right" style="width:50px">元 / 月</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">安全保障：</span>
                <yd-input slot="right" type="number" v-model="model.localInfo.security" regex="" placeholder="请输入安全保障金"></yd-input>
                <span slot="right" style="width:50px">元 / 月</span>
            </yd-cell-item>

        </yd-cell-group>

        <yd-cell-group title="其他资料（选填）">
            <yd-cell-item>
                <span slot="left">身份证号：</span>
                <yd-input slot="right" v-model="model.localInfo.iDCard" regex="" placeholder="请输入身份证号"></yd-input>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">住址：</span>
                <yd-input slot="right" v-model="model.localInfo.address" regex="" placeholder="请输入住址"></yd-input>
            </yd-cell-item>
        </yd-cell-group>

        <yd-cell-group title="银行卡资料（选填）">
            <yd-cell-item>
                <span slot="left">开户银行：</span>
                <yd-input slot="right" v-model="model.localInfo.bank" regex="" placeholder="例：农业银行梧州分行"></yd-input>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">银行账户：</span>
                <yd-input slot="right" v-model="model.localInfo.account" regex="" placeholder="请输入车牌号"></yd-input>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">开户人：</span>
                <yd-input slot="right" v-model="model.localInfo.accountName" regex="" placeholder="默认姓名"></yd-input>
            </yd-cell-item>
        </yd-cell-group>

        <div>
            <yd-button size="large" type="primary" @click.native="buttonclick">提交</yd-button>
        </div>
    </div>
</template>

<script src="./userdetail.ts" />
<style src="./../website.css" />