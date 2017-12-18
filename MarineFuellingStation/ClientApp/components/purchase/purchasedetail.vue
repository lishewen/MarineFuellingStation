<template>
    <div id="root">
        <div v-show="model.state == 0" style="background-color: lightcoral;padding: 10px; text-align: center; color: white">已开单，待施工</div>
        <div v-show="model.state > 0 && model.state < 7" style="background-color: lightcoral;padding: 10px; text-align: center; color: white">施工中，未完工</div>
        <div v-show="model.state == 7" style="background-color: lightcoral;padding: 10px; text-align: center; color: white">卸油完工，待审核</div>
        <div v-show="model.state == 8" style="background-color: yellowgreen;padding: 10px; text-align: center; color: white">已审核，已更新油仓</div>
        <yd-cell-group>
            <yd-cell-item>
                <span slot="left">油品：</span>
                <span slot="right">{{model.product == null? "" : model.product.name}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">单价：</span>
                <span slot="right">{{model.price}}</span>
                <span slot="right" style="width: 50px">元 / 吨</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">数量：</span>
                <span slot="right">{{model.count}}</span>
                <span slot="right">吨</span>
            </yd-cell-item>

            <!--<yd-cell-item>
                <span slot="left">始发地：</span>
                <span slot="right">{{model.origin}}</span>
            </yd-cell-item>-->

            <yd-cell-item>
                <span slot="left">出发时间：</span>
                <span slot="right">{{formatDate(model.startTime)}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">预计到达：</span>
                <span slot="right">{{formatDate(model.arrivalTime)}}</span>
            </yd-cell-item>
        </yd-cell-group>

        <yd-cell-group title="运输信息">
            <yd-cell-item>
                <span slot="left">车牌号一：</span>
                <span slot="right">{{model.carNo}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.trailerNo != null">
                <span slot="left">挂车号：</span>
                <span slot="right">{{model.trailerNo}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.driver1 != null">
                <span slot="left">司机一：</span>
                <span slot="right">{{model.driver1}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.idCard1 != null">
                <span slot="left">身份证一：</span>
                <span slot="right">{{model.idCard1}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.phone1 != null">
                <span slot="left">电话号码一：</span>
                <span slot="right">{{model.phone1}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.driver2 != null">
                <span slot="left">司机二：</span>
                <span slot="right">{{model.driver2}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.idCard2 != null">
                <span slot="left">身份证二：</span>
                <span slot="right">{{model.idCard2}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="model.phone2 != null">
                <span slot="left">电话号码二：</span>
                <span slot="right">{{model.phone2}}</span>
            </yd-cell-item>
        </yd-cell-group>
        <div v-show="model.state == 7">
            <yd-grids-group :rows="2" title="与计划误差">
                <yd-grids-item>
                    <div slot="text">
                        <p style="color: lightgray;font-size: .3rem">油表误差</p>
                        <p style="margin-top: .2rem;font-size: .2rem">{{model.diffLitre}}升 | {{model.diffTon}}吨</p>
                    </div>
                </yd-grids-item>
                <yd-grids-item>
                    <div slot="text">
                        <p style="color: lightgray;font-size: .3rem">磅秤误差</p>
                        <p style="margin-top: .2rem;font-size: .2rem">{{round(model.scaleWithCar - model.scale - model.count)}}吨</p>
                    </div>
                </yd-grids-item>
            </yd-grids-group>
            <yd-cell-group title="卸油明细">
                <yd-cell-item>
                    <span slot="left">毛重：</span>
                    <span slot="right">{{model.scaleWithCar}}吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">皮重：</span>
                    <span slot="right">{{model.scale}}吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">净重：</span>
                    <span slot="right">{{model.scaleWithCar - model.scale}}吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">与计划相差：</span>
                    <span slot="right">{{round((model.scaleWithCar - model.scale) - model.count)}}吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">卸入油仓：</span>
                    <div slot="right" class="font16">
                        <p v-for="ts in model.toStoresList" :key="ts.id">
                            {{ts.name}} - {{round(ts.count)}}升
                        </p>
                    </div>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">卸入油仓总数：</span>
                    <span slot="right">{{model.oilCount}}升</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">密度：</span>
                    <span slot="right">{{model.density}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">换算吨数：</span>
                    <span slot="right">{{round(model.oilCount * model.density / 1000)}}吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">施工人：</span>
                    <span slot="right">{{model.worker}}</span>
                </yd-cell-item>
                <yd-cell-item>
                    <div slot="left">毛重图片：</div>
                    <div slot="right"><div class="img-wrap"><img :src="this.model.scaleWithCarPic" /></div></div>
                </yd-cell-item>
                <yd-cell-item>
                    <div slot="left">皮重图片：</div>
                    <div slot="right"><div class="img-wrap"><img :src="this.model.scalePic" /></div></div>
                </yd-cell-item>
            </yd-cell-group>
        </div>
    </div>
</template>

<script src="./purchasedetail.ts" />


