<template>
    <div id="root">
        <yd-tab :change="change">

            <yd-tab-panel label="施工">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="show2 = true" v-for="m in movestores" :key="m.id">
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span style="color:forestgreen">出：</span><span>【{{m.outStoreTypeName}}】 - {{m.outStoreName}}</span>
                            </p>
                            <p>
                                <span style="color:red">入：</span><span>{{m.inStoreTypeName}} - {{m.inStoreName}}</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">{{m.name}} - {{formatDate(m.lastUpdateAt)}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray; font-size: 22px">{{m.outPlan}}升</p>
                            <p style="color:red; font-size: 14px">{{m.stateName}}</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="actConfrim()">
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span style="color:forestgreen">出：</span><span>【地仓】 - 1#仓</span>
                            </p>
                            <p>
                                <span style="color:red">入：</span><span>【船785仓】 - 2#仓</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">ZC07070001</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray; font-size: 22px">1000升</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="完工单">
                <yd-cell-group>
                    <yd-cell-item arrow v-for="f in fnmovestores" :key="f.id">
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span style="color:forestgreen">出：</span><span>【{{f.outStoreTypeName}}】 - {{f.outStoreName}}</span>
                            </p>
                            <p>
                                <span style="color:red">入：</span><span>{{f.inStoreTypeName}} - {{f.inStoreName}}</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">{{f.name}} - {{formatDate(f.lastUpdateAt)}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray; font-size: 22px">{{f.outPlan}}升</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-popup v-model="show2" position="right">
                <yd-cell-group title="请录入" style="margin-top: 20px">
                    <yd-cell-item>
                        <span slot="left">实际转出：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                        <span slot="right">升</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">实际转入：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder=""></yd-input>
                        <span slot="right">升</span>
                    </yd-cell-item>
                </yd-cell-group>
                <div style="text-align: center">
                    <yd-button style="width:100px" type="primary" @click.native="saveclick2()">施工结束</yd-button>
                </div>
            </yd-popup>
        </yd-tab>
    </div>
</template>

<script src="./movestoreact.ts" />