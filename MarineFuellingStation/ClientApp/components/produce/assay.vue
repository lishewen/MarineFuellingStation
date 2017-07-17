<template>
    <div id="root">
        <yd-tab :change="change">
            <yd-tab-panel label="化验录入">
                <yd-cell-group title="单号：HY07070001" style="margin-top:20px">

                    <yd-cell-item>
                        <span slot="left">化验员：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="默认开单员"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">化验油仓：</span>
                        <select slot="right">
                            <option value="">请选择油仓</option>
                            <option value="1">1#船759</option>
                            <option value="2">2#地仓</option>
                            <option value="3">3#船759</option>
                        </select>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">采购来源：</span>
                        <select slot="right">
                            <option value="">请选择采购来源计划单</option>
                            <option value="1">CG07070001#35吨#柴油</option>
                            <option value="2">CG07070001#35吨#柴油</option>
                            <option value="3">CG07070001#35吨#柴油</option>
                        </select>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">视密：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="参考值：0.835"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">标密：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入标密"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">闭口闪点：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="参考值：>=61℃"></yd-input>
                        <span slot="right">℃</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">油温：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入油温"></yd-input>
                        <span slot="right">℃</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">味道：</span>
                        <select slot="right">
                            <option value="">请选择</option>
                            <option value="1">一般刺鼻</option>
                            <option value="2">刺鼻</option>
                            <option value="3">不刺鼻</option>
                        </select>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">混水反应：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="参考值：油水迅速分离，不出现絮状物"></yd-input>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">十六烷值：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入标密"></yd-input>
                    </yd-cell-item>

                </yd-cell-group>
                <yd-cell-group title="化验二">

                    <yd-cell-item>
                        <span slot="left">初硫：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="参考值：160-180℃"></yd-input>
                        <span slot="right">℃</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">10%：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="参考值："></yd-input>
                        <span slot="right">℃</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">50%：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="参考值：220-280℃"></yd-input>
                        <span slot="right">℃</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">90%：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="参考值：300-345℃"></yd-input>
                        <span slot="right">℃</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">回流：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                        <span slot="right">%</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">干点：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="参考值："></yd-input>
                        <span slot="right">℃</span>
                    </yd-cell-item>

                </yd-cell-group>

                <yd-cell-group title="不显示" style="margin-top:10px">
                    <yd-cell-item>
                        <span slot="left">●开单员：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">●开单时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
                <div>
                    <yd-button size="large" type="primary">提交</yd-button>
                </div>

            </yd-tab-panel>
            <yd-tab-panel label="记录">

                <yd-cell-group>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>HY07070001</p>
                            <p style="color:lightgray;font-size:12px">张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>HY07070001</p>
                            <p style="color:lightgray;font-size:12px">张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>HY07070001</p>
                            <p style="color:lightgray;font-size:12px">张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<script src="./assay.ts" />