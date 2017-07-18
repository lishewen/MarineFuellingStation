<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="添加客户">
                <yd-cell-group title="必填" style="margin-top:20px">

                    <yd-cell-item>
                        <yd-radio-group slot="left" v-model="radio2">
                            <yd-radio val="1">个人</yd-radio>
                            <yd-radio val="2">公司</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>

                    <yd-cell-item v-show="show1">
                        <span slot="left">所属公司：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入所属公司，填写关键字自动检索数据库"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">船号/车牌号：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入名称船号或车牌号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">默认商品/油品：</span>
                        <select slot="right">
                            <option value="">请选择油品</option>
                            <option value="1">93#</option>
                            <option value="2">95#</option>
                            <option value="3">97#</option>
                        </select>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">联系人：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入联系人"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">联系电话：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入联系电话"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>

                <yd-cell-group title="选填" style="margin-top:10px">

                    <yd-cell-item>
                        <span slot="left">身份证号：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入身份证号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">地址：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="收银流程充值自动计算"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">固定电话：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入固定电话"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>

                <yd-cell-group title="不显示" style="margin-top:10px">
                    <yd-cell-item>
                        <span slot="left">●账户金额：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="收银流程充值自动计算"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●登记时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●最近消费时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●总消费金额：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary">提交</yd-button>
                </div>
            </yd-tab-panel>
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
    </div>
</template>

<script src="./myclient.ts" />