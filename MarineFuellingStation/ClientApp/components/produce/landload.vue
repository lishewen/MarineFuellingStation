<template>
    <div id="root">
        <yd-tab :change="change">

            <yd-tab-panel label="1-空车过磅">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="show1 = true">
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:blue;text-align:right">已开单</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show1 = true">
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:blue;text-align:right">已开单</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show1 = true">
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:blue;text-align:right">已开单</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="2-施工">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="endConfrim()">
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:darkseagreen;text-align:right">卸油中</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="beginConfrim()">
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:lightskyblue;text-align:right">已化验</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="beginConfrim()">
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:lightskyblue;text-align:right">已化验</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="3-油车过磅">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="show3 = true">
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:forestgreen;text-align:right">卸油结束</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show3 = true">
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:forestgreen;text-align:right">卸油结束</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show3 = true">
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:forestgreen;text-align:right">卸油结束</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="完工">
                <yd-cell-group>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:red;text-align:right">已完成</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:red;text-align:right">已完成</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:red;text-align:right">已完成</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>CG07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:red;text-align:right">已完成</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-popup v-model="show1" position="right">
                <yd-cell-group title="请输入">
                    <yd-cell-item>
                        <span slot="left">磅秤数：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入磅秤数"></yd-input>
                        <span slot="right">吨</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">图片上传：</span>
                        <input slot="left" type="file" value="选择图片" />
                    </yd-cell-item>
                </yd-cell-group>
                <div style="text-align: center">
                    <yd-button style="width:100px" type="primary" @click.native="saveclick1()">保存</yd-button>
                </div>
            </yd-popup>
            <yd-popup v-model="show3" position="right">
                <yd-cell-group title="请输入">
                    <yd-cell-item>
                        <span slot="left">磅秤数：</span>
                        <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入磅秤数"></yd-input>
                        <span slot="right">吨</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">图片上传：</span>
                        <input slot="left" type="file" value="选择图片" />
                    </yd-cell-item>
                </yd-cell-group>
                <div style="text-align: center">
                    <yd-button style="width:100px" type="primary" @click.native="saveclick3()">保存</yd-button>
                </div>
            </yd-popup>
        </yd-tab>
    </div>
</template>

<script src="./landload.ts" />