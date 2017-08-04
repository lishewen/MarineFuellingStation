<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="添加">
                <yd-cell-group title="基本资料（必填）" style="margin-top:20px">
                    <yd-cell-item>
                        <span slot="left">性别：</span>
                        <yd-radio-group slot="left" v-model="radio1">
                            <yd-radio val="1">男</yd-radio>
                            <yd-radio val="2">女</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>

                    <yd-cell-item arrow @click.native="departmentshow = true">
                        <span slot="left">部门：</span>
                        <span slot="right">{{selectdepartmentname}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">职位：</span>
                        <yd-radio-group slot="left" v-model="radio2">
                            <yd-radio val="1">普通职员</yd-radio>
                            <yd-radio val="2">上级</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">姓名：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入姓名"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">电话：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入电话"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">入职时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="默认当天"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">基本工资：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入基本工资"></yd-input>
                        <span slot="right">元</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">社保自付：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入社保自付金额"></yd-input>
                        <span slot="right" style="width:50px">元 / 月</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">安全保障：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入安全保障金"></yd-input>
                        <span slot="right" style="width:50px">元 / 月</span>
                    </yd-cell-item>

                </yd-cell-group>
                
                <yd-cell-group title="其他资料（选填）">
                    <yd-cell-item>
                        <span slot="left">身份证号：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入身份证号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">住址：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入住址"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="银行卡资料（选填）">
                    <yd-cell-item>
                        <span slot="left">开户银行：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="例：农业银行梧州分行"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">银行账户：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入车牌号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">开户人：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="默认姓名"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="不显示" style="margin-top:10px">
                    <yd-cell-item>
                        <span slot="left">●离职时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●是否离职：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary">提交</yd-button>
                </div>
            </yd-tab-panel>
            <yd-tab-panel label="列表">
                <yd-cell-group>
                    <weui-search v-model="sv" />
                    <yd-cell-item arrow @click.native="userClick(user)" v-for="user in users" :key="user.id">
                        <div slot="left">
                            <p>{{user.name}}</p>
                            <p style="color:lightgray;font-size:12px" v-show="user.isleader==0">普通职员</p>
                            <p style="color:lightgray;font-size:12px" v-show="user.isleader==1">上级</p>
                        </div>
                        <div slot="right" style="text-align: right;margin-right: 5px">
                            <p>
                                <span style="color:gray; font-size: 18px">{{departmentdict[user.department[0]]}}</span>
                            </p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-actionsheet :items="departmentoptions" v-model="departmentshow" cancel="取消"></yd-actionsheet>
        <yd-actionsheet :items="userItems" v-model="usershow" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./user.ts" />