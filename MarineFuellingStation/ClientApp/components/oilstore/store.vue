<template>
    <div id="root">
        <yd-tab :change="change">

            <yd-tab-panel label="所有分类">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="show1 = true">
                        <div slot="left" style="margin: 10px 0 10px 0;line-height:22px">
                            <p>【地仓】- 6个仓</p>
                            <p style="color:lightgray;font-size:14px">总容量：20000升</p>
                        </div>
                        <div slot="right" style="text-align: right;margin: 10px 0 10px 0">
                            <p style="color:forestgreen; font-size: 16px">1000升</p>
                            <p style="color:lightcoral; font-size: 16px">￥180000</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show1 = true">
                        <div slot="left" style="margin: 10px 0 10px 0">
                            <p>【船758】- 6个仓</p>
                            <p style="color:lightgray;font-size:14px">总容量：20000升</p>
                        </div>
                        <div slot="right" style="text-align: right;margin: 10px 0 10px 0">
                            <p style="color:forestgreen; font-size: 16px">80升</p>
                            <p style="color:lightcoral; font-size: 16px">￥180000</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show1 = true">
                        <div slot="left" style="margin: 10px 0 10px 0">
                            <p>【船229】- 6个仓</p>
                            <p style="color:lightgray;font-size:14px">总容量：20000升</p>
                        </div>
                        <div slot="right" style="text-align: right;margin: 10px 0 10px 0">
                            <p style="color:forestgreen; font-size: 16px">400升</p>
                            <p style="color:lightcoral; font-size: 16px">￥180000</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="添加">
                <yd-cell-group>
                    <yd-cell-item>
                        <yd-radio-group v-model="radio1" slot="left">
                            <yd-radio val="1">销售仓</yd-radio>
                            <yd-radio val="2">存储仓</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">所属分类：</span>
                        <select slot="right">
                            <option value="">请选择分类</option>
                            <option value="1">地仓</option>
                            <option value="2">船758</option>
                            <option value="3">船229</option>
                        </select>
                        <span slot="right"><yd-button type="primary" @click.native="show2 = true">添加</yd-button></span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">名称：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">容量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入容量"></yd-input>
                        <span slot="right">升</span>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-cell-group title="不显示">
                    <yd-cell-item>
                        <span slot="left">创建时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">平均单价成本：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="每次出入库后自动计算"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">上次数量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="变化前的数量">数量</yd-input>
                        <span slot="right">升</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="初始为0">数量</yd-input>
                        <span slot="right">升</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">当前价值：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="平均单价成本 * 数量"></yd-input>
                        <span slot="right">元</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">是否使用：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="布尔值"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-popup v-model="show1" position="right">
            <yd-cell-group title="【地仓】">
                <yd-cell-item>
                    <div slot="left">1#仓</div>
                    <div slot="right">
                        <p style="color:forestgreen; font-size: 14px">80升</p>
                        <p style="color:lightcoral; font-size: 14px">￥180000</p>
                    </div>
                </yd-cell-item>
                <yd-cell-item>
                    <div slot="left">2#仓</div>
                    <div slot="right">
                        <p style="color:forestgreen; font-size: 14px">80升</p>
                        <p style="color:lightcoral; font-size: 14px">￥180000</p>
                    </div>
                </yd-cell-item>
                <yd-cell-item>
                    <div slot="left">3#仓</div>
                    <div slot="right">
                        <p style="color:forestgreen; font-size: 14px">80升</p>
                        <p style="color:lightcoral; font-size: 14px">￥180000</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="">添加</yd-button>
            </div>
        </yd-popup>
        <yd-popup v-model="show2" position="right">
            <yd-cell-group title="添加分类">
                <yd-cell-item>
                    <span slot="left">分类名称：</span>
                    <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入名称"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./store.ts" />