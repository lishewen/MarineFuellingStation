<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="开单">
                <yd-cell-group title="单号：QW2017070700001" style="margin-top: 20px">

                    <yd-cell-item>
                        <span slot="left">船号：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入您的船号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">金额：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入金额"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">销售员：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="默认是开单员"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>

                <yd-cell-group title="选填">

                    <yd-cell-item>
                        <span slot="left">航次：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入航次"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">船舶总吨：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入船舶总吨"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">批准书文号：</span>
                        <yd-input slot="right" v-model="allowNo" regex="" placeholder="请输入批准书文号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">作业地点：</span>
                        <yd-input slot="right" v-model="workplace" regex="" placeholder="请输入作业地点"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">作业单位：</span>
                        <yd-input slot="right" v-model="workcompany" regex="" placeholder="请输入批准书文号"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">联系电话：</span>
                        <yd-input slot="right" v-model="workphone" regex="" placeholder="请输入联系电话"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">是否开票</span>
                        <span slot="right">
                            <yd-switch v-model="isinvoice"></yd-switch>
                        </span>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">开票单位：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入开票单位"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入开票单价，默认同上"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item v-show="isinvoice">
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入开票，默认同上"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>
                <yd-cell-group title="不显示">
                    <yd-cell-item>
                        <span slot="left">●开单时间：</span>
                        <input slot="right" class="cell-input" type="date" value="2017-07-19" placeholder="">
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●总价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="单价 x 数量"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">●作业时间：</span>
                        <input slot="right" class="cell-input" type="date" placeholder="生产流程中录入">
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●完工时间：</span>
                        <input slot="right" class="cell-input" type="date" placeholder="生产流程中录入">
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●开单员：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary">提交</yd-button>
                </div>
            </yd-tab-panel>

            <yd-tab-panel label="单据记录">
                <yd-cell-group>
                    <yd-cell-item arrow>
                        <span slot="left">船0001</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">QW2017070700001</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:red; padding-left:10px">施工中</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">QW2017070700001</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:green; padding-left:10px">已开单</span>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <span slot="left">船0002</span>
                        <span slot="left" style="color:lightgray;margin-left:10px">QW2017070700001</span>
                        <span slot="right">2017-07-07</span>
                        <span slot="right" style="color:blue; padding-left:10px">已完成</span>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<style src="./plan.css" />
<script src="./boatclean.ts"/>