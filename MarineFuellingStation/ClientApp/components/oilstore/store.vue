<template>
    <div id="root">
        <yd-tab :change="change">
    
            <yd-tab-panel label="所有分类">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="stClick(st)" v-for="st in sts">
                        <div slot="left" style="margin: 10px 0 10px 0;line-height:22px">
                            <p>【{{st.name}}】- {{st.stores.length}}个仓</p>
                            <p style="color:lightgray;font-size:14px">总容量：{{sumVolume(st)}}升</p>
                        </div>
                        <div slot="right" style="text-align: right;margin: 10px 0 10px 0">
                            <p style="color:forestgreen; font-size: 16px">{{sumValue(st)}}升</p>
                            <p style="color:lightcoral; font-size: 16px">￥{{sumCost(st)}}</p>
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
                        <select slot="right" v-model="model.storeTypeId">
                            <option value="-1">请选择分类</option>
                            <option v-for="st in sts" :key="st.id" :value="st.id">{{st.name}}</option>
                        </select>
                        <span slot="right">
                            <yd-button type="primary" @click.native="newstShowClick">添加</yd-button>
                        </span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">名称：</span>
                        <yd-input slot="right" v-model="model.name" required placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">容量：</span>
                        <yd-input type="number" slot="right" v-model="model.volume" required placeholder="请输入容量"></yd-input>
                        <span slot="right">升</span>
                    </yd-cell-item>
                </yd-cell-group>
                <div style="text-align: center">
                    <yd-button style="width:100px" type="primary" @click.native="">添加</yd-button>
                </div>
            </yd-tab-panel>
        </yd-tab>
        <yd-popup v-model="stshow" position="right">
            <yd-cell-group :title="'【' + currentst.name + '】'">
                <yd-cell-item v-for="s in currentst.stores">
                    <div slot="left">{{s.name}}</div>
                    <div slot="right">
                        <p style="color:forestgreen; font-size: 14px">{{s.value}}升</p>
                        <p style="color:lightcoral; font-size: 14px">￥{{s.cost}}</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <yd-popup v-model="newstshow" position="right">
            <yd-cell-group title="添加分类">
                <yd-cell-item>
                    <span slot="left">分类名称：</span>
                    <yd-input slot="right" v-model="stName" required placeholder="请输入名称"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="postStoreType">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./store.ts" />