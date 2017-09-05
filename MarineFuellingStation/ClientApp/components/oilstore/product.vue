<template>
    <div id="root">
        <yd-tab :change="change">

            <yd-tab-panel label="所有分类">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="ptClick(pt)" v-for="pt in pts" :key="pt.id">
                        <div slot="left" style="margin: 10px 0 10px 0;line-height:22px">
                            <p>{{pt.name}}</p>
                            <p style="color:lightgray;font-size:14px">XXXXXX</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="添加">
                <yd-cell-group>
                    <yd-cell-item @click.native="ptshow = true">
                        <span slot="left">所属分类：</span>
                        <span slot="right">{{selectptname}}</span>
                        <span slot="right"><yd-button type="primary" @click.native="addpt($event)">添加</yd-button></span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">名称：</span>
                        <yd-input slot="right" v-model="currentproduct.name" required placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">最新单价：</span>
                        <yd-input slot="right" type="number" v-model="currentproduct.minPrice" placeholder="请输入单价"></yd-input>
                        <span slot="right" style="width: 60px">元 / 升</span>
                    </yd-cell-item>
                </yd-cell-group>
                <div style="text-align: center">
                    <yd-button style="width:100px" type="primary" @click.native="">添加</yd-button>
                </div>
            </yd-tab-panel>
        </yd-tab>
        <yd-actionsheet :items="ptoptions" v-model="ptshow" cancel="取消"></yd-actionsheet>
        <yd-popup v-model="show1" position="right">
            <yd-cell-group :title="currentpt.name">
                <yd-cell-item v-for="p in currentpt.products" :key="p.id">
                    <div slot="left">{{p.name}}</div>
                    <div slot="right">
                        <p style="color:forestgreen; font-size: 14px">￥{{p.minPrice}}/升</p>
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
                </yd-cell-item>
                <yd-cell-item>
                    <yd-input slot="left" v-model="ptName" required placeholder="请输入名称"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="postProductType()">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./product.ts" />