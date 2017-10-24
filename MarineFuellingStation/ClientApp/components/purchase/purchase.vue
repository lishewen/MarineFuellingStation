<template>
    <div>
        <yd-tab :callback="change">
            <yd-tab-panel label="计划开单">
                <yd-cell-group :title="'单号：' + model.name">
                    <yd-cell-item arrow @click.native="oilshow = true">
                        <span slot="left">油品：</span>
                        <span slot="right">{{oilName}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="model.price" required placeholder="请输入单价"></yd-input>
                        <span slot="right" style="width: 50px">元 / 吨</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="model.count" required placeholder="请输入数量"></yd-input>
                        <span slot="right">吨</span>
                    </yd-cell-item>

                    <yd-cell-item arrow>
                        <span slot="left">始发地：</span>
                        <input slot="right" type="text" @click.stop="originshow = true" v-model="model.origin" readonly placeholder="请选择始发地"/>
                    </yd-cell-item>

                    <yd-cell-item arrow>
                        <span slot="left">出发时间：</span>
                        <yd-datetime v-model="model.startTime" type="datetime" slot="right"></yd-datetime>
                    </yd-cell-item>

                    <yd-cell-item arrow>
                        <span slot="left">预计到达：</span>
                        <yd-datetime v-model="model.arrivalTime" type="datetime" slot="right"></yd-datetime>
                    </yd-cell-item>

                </yd-cell-group>

                <yd-cell-group title="运输">
                    <yd-cell-item>
                        <span slot="left">车牌号一：</span>
                        <yd-input slot="right" v-model="model.carNo" required placeholder="请输入车牌号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">挂车号：</span>
                        <yd-input slot="right" v-model="model.trailerNo" regex="" placeholder="请输入挂车号，选填"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">司机一：</span>
                        <yd-input slot="right" v-model="model.driver1" regex="" placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">身份证一：</span>
                        <yd-input slot="right" v-model="model.idCard1" regex="" placeholder="请输入身份证号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">电话号码一：</span>
                        <yd-input slot="right" v-model="model.phone1" regex="" placeholder="请输入电话号码"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">司机二：</span>
                        <yd-input slot="right" v-model="model.driver2" regex="" placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">身份证二：</span>
                        <yd-input slot="right" v-model="model.idCard2" regex="" placeholder="请输入身份证号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">电话号码二：</span>
                        <yd-input slot="right" v-model="model.phone2" regex="" placeholder="请输入电话号码"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>

                <div>
                    <yd-button size="large" type="primary" @click.native="buttonclick" :disabled="isPrevent">提交</yd-button>
                </div>
            </yd-tab-panel>
            <yd-tab-panel label="列表">

                <yd-cell-group>
                    <yd-search v-model="sv" />
                    <yd-cell-item arrow v-for="s in list" :key="s.id" @click.native="godetail(s.id)">
                        <div slot="left">
                            <p>{{s.name}}</p>
                            <p>{{s.origin}}</p>
                            <p style="color:lightgray;font-size:12px">{{s.carNo}} {{s.trailerNo}}</p>
                        </div>
                        <div slot="right" style="text-align: right;margin-right: .2rem;padding: .2rem 0 .2rem">
                            <p>
                                <span style="color:forestgreen;">{{s.product == null ? "" : s.product.name}}</span>
                                <span style="color:gray; font-size: 22px" s>{{s.count}}吨</span>
                            </p>
                            <p style="color:lightcoral;">预计到达：{{formatDate(s.arrivalTime, 'MM-DD hh:mm')}}</p>
                            <p style="color:gray;" v-show="s.state != 0">实际到达：{{formatDate(s.updateAt)}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-actionsheet :items="oiloptions" v-model="oilshow" cancel="取消"></yd-actionsheet>
        <yd-cityselect v-model="originshow" :callback="origincallback" :items="district"></yd-cityselect>
    </div>
</template>

<script src="./purchase.ts" />