<template>
    <div id="root">
        <yd-tab :change="change">

            <yd-tab-panel label="施工">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="show2 = true">
                        <div slot="left">
                            <p>XS07070001</p>
                            <p style="color:lightgray;font-size:12px">桂平货0001</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">{{selectedstore}} 35吨 / 柴油</p>
                            <p style="color:blue;text-align:right">已开单</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow @click.native="show2 = true">
                        <div slot="left">
                            <p>XS07070001</p>
                            <p style="color:lightgray;font-size:12px">桂平货0001</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">{{selectedstore}} 35吨 / 柴油</p>
                            <p style="color:blue;text-align:right">已开单</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="完工">
                <yd-cell-group>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>XS07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:red;text-align:right">已完成</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>XS07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:red;text-align:right">已完成</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left">
                            <p>XS07070001</p>
                            <p style="color:lightgray;font-size:12px">桂DCT200 司机张三</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray">1#地仓 / 35吨 / 柴油</p>
                            <p style="color:red;text-align:right">已完成</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-popup v-model="show2" position="right">
                <yd-cell-group title="请选择转出油仓" style="margin-top: 20px">
                    <yd-cell-item type="radio">
                        <div slot="left">
                            <p>1#加油船759</p>
                        </div>
                        <div slot="right">
                            <p style="color:lightgray">500升</p>
                        </div>
                        <input slot="right" type="radio" value="Lili" v-model="picked" />
                    </yd-cell-item>
                    <yd-cell-item type="radio">
                        <div slot="left">
                            <p>2#加油船759</p>
                        </div>
                        <div slot="right">
                            <p style="color:lightgray">500升</p>
                        </div>
                        <input slot="right" type="radio" value="Lucy" v-model="picked" />
                    </yd-cell-item>
                    <yd-cell-item type="radio">
                        <div slot="left">
                            <p>10#加油船669</p>
                        </div>
                        <div slot="right">
                            <p style="color:lightgray">500升</p>
                        </div>
                        <input slot="right" type="radio" value="Jack" v-model="picked" />
                    </yd-cell-item>
                </yd-cell-group>
                <div style="text-align: center">
                    <yd-button style="width:100px" type="primary" @click.native="saveclick2()">施工结束</yd-button>
                </div>
            </yd-popup>
        </yd-tab>
    </div>
</template>

<script src="./load.ts" />