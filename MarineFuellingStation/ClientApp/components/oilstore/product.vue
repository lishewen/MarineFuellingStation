<template>
    <div id="root">
        <yd-tab :change="change">

            <yd-tab-panel label="所有分类">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="show1 = true">
                        <div slot="left" style="margin: 10px 0 10px 0;line-height:22px">
                            <p>柴油</p>
                            <p style="color:lightgray;font-size:14px">XXXXXX</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show1 = true">
                        <div slot="left" style="margin: 10px 0 10px 0;line-height:22px">
                            <p>循环油</p>
                            <p style="color:lightgray;font-size:14px">XXXXXX</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show1 = true">
                        <div slot="left" style="margin: 10px 0 10px 0;line-height:22px">
                            <p>机油</p>
                            <p style="color:lightgray;font-size:14px">XXXXXX</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="添加">
                <yd-cell-group>
                    <yd-cell-item>
                        <span slot="left">所属分类：</span>
                        <select slot="right">
                            <option value="">请选择分类</option>
                            <option value="1">柴油</option>
                            <option value="2">循环油</option>
                            <option value="3">机油</option>
                        </select>
                        <span slot="right"><yd-button type="primary" @click.native="show2 = true">添加</yd-button></span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">名称：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">最新单价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入单价"></yd-input>
                        <span slot="right" style="width: 60px">元 / 升</span>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-cell-group title="不显示">
                    <yd-cell-item>
                        <span slot="left">创建/更新时间：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">上次单价：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="上次更新前的单价"></yd-input>
                        <span slot="right" style="width: 60px">元 / 升</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">是否使用：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="布尔值"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
        <yd-popup v-model="show1" position="right">
            <yd-cell-group title="【柴油】">
                <yd-cell-item>
                    <div slot="left">国三</div>
                    <div slot="right">
                        <p style="color:forestgreen; font-size: 14px">￥4.25/升</p>
                    </div>
                </yd-cell-item>
                <yd-cell-item>
                    <div slot="left">国四</div>
                    <div slot="right">
                        <p style="color:forestgreen; font-size: 14px">￥4.25/升</p>
                    </div>
                </yd-cell-item>
                <yd-cell-item>
                    <div slot="left">国五</div>
                    <div slot="right">
                        <p style="color:forestgreen; font-size: 14px">￥4.25/升</p>
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

<script src="./product.ts" />