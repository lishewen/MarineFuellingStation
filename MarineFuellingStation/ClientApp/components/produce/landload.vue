<template>
    <div id="root">
        <div v-show="showSelectWorker">
            <div class="font16 align-center" style="font-weight:bold;margin:10px 0">请选择当前施工员：{{worker}}</div>
            <yd-cell-group>
                <yd-cell-item type="radio" v-for="w,idx in workers" :key="idx">
                    <span slot="left">{{w.name}}</span>
                    <input slot="right" type="radio" :value="w.name" v-model="worker" />
                </yd-cell-item>
            </yd-cell-group>
            <div class="align-center">
                <yd-button style="width:90%;height:38px;" @click.native="workerSelectedClick" class="mtop20" :disabled="worker == null || worker == ''">下一步</yd-button>
            </div>
        </div>
        <div v-show="!showSelectWorker">
            <div class="align-center first-group">
                <yd-button style="width:90%;height:38px" type="primary" @click.native="showOrdersclick" :disabled="oid != null && oid != ''">销售单{{order.name? '：' + order.name + ' / ' + order.count + order.unit : ''}}</yd-button>
            </div>
            <yd-step :current="currStep" style="margin: .4rem 0 .4rem">
                <yd-step-item>
                    <span slot="bottom">选择油仓</span>
                </yd-step-item>
                <yd-step-item>
                    <span slot="bottom">空车过磅</span>
                </yd-step-item>
                <yd-step-item>
                    <span slot="bottom">加油</span>
                </yd-step-item>
                <yd-step-item>
                    <span slot="bottom">油车过磅</span>
                </yd-step-item>
                <yd-step-item>
                    <span slot="bottom">完工</span>
                </yd-step-item>
            </yd-step>
            <!--1-选择油仓-->
            <div class="align-center" v-show="currStep == 1">
                <yd-button style="width:90%;height: 38px" type="primary" @click.native="showStores = true">第一步：选择销售仓</yd-button>
            </div>
            <!--2-空车过磅-->
            <yd-cell-group title="第二步：空车过磅 称皮重" v-show="currStep == 2">
                <yd-cell-item>
                    <span slot="left">测量密度：</span>
                    <yd-input slot="right" v-model="order.density" type="number" required placeholder="请输入测量密度"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">油温：</span>
                    <yd-input slot="right" v-model="order.oilTemperature" type="number" required placeholder="请输入油温"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">皮重：</span>
                    <yd-input slot="right" v-model="order.emptyCarWeight" type="number" required placeholder="请输入磅秤数"></yd-input>
                    <span slot="right">KG</span>
                </yd-cell-item>
                <!--<yd-cell-item>
                    <span slot="left">图片上传：</span>
                    <label slot="right" class="input-file"><input title="浏览文件" type="file" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />选择图片…</label>
                </yd-cell-item>-->
            </yd-cell-group>
            <div class="align-center" v-show="currStep == 2">
                <yd-button type="warning" class="mtop20" slot="right" style="width: 90%;height:38px" @click.native="uploadByWeixin">磅秤拍照上传</yd-button>
                <yd-button style="width:90%;height:38px;margin-top: 20px;" type="primary" @click.native="currStep -= 1">← 上一步：选择销售仓</yd-button>
                <yd-button style="width:90%;height:38px;margin-top: 20px;" type="primary" @click.native="changeState(3)" :disabled="order.emptyCarWeightPic == '' || order.emptyCarWeightPic == null || order.density <= 0 || order.emptyCarWeight <= 0">下一步：加油 →</yd-button>
            </div>
            <div class="align-center first-group" v-show="currStep == 2">
                <yd-lightbox class="img-wrap">
                    <yd-lightbox-img :src="order.emptyCarWeightPic"></yd-lightbox-img>
                </yd-lightbox>
            </div>
            <!--3-加油-->
            <div class="align-center" v-show="currStep == 3">
                <yd-cell-group title="第三步：加油">
                    <yd-cell-item>
                        <span slot="left">表数（加油前）：</span>
                        <yd-input slot="right" v-model="lastorder.instrument1" type="number" required placeholder="请输入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">表数（加油后）：</span>
                        <yd-input slot="right" v-model="order.instrument1" type="number" required placeholder="请输入"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="right">油量：{{order.oilCountLitre}}升</span>
                    </yd-cell-item>
                    <!--应客户要求，暂时只有一个加油表-->
                    <!--<yd-cell-item>
                        <span slot="left">加油后表数2：</span>
                        <yd-input slot="right" v-model="order.instrument2" type="number" required placeholder="请输入装油表数2"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">加油后表数3：</span>
                        <yd-input slot="right" v-model="order.instrument3" type="number" required placeholder="请输入装油表数3"></yd-input>
                    </yd-cell-item>-->
                </yd-cell-group>
                <yd-button style="width:90%;height:38px;margin-top: 20px;" type="primary" @click.native="currStep -= 1">← 上一步：空车过磅</yd-button>
                <yd-button style="width:90%;height:38px;margin-top: 20px;" type="primary" @click.native="changeState(4)">下一步：油车过磅 →</yd-button>
            </div>
            <!--4-油车过磅-->
            <yd-cell-group title="第四步：油车过磅" v-show="currStep == 4">
                <yd-cell-item>
                    <span slot="right">皮重：{{order.emptyCarWeight}} KG</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">毛重：</span>
                    <yd-input slot="right" v-model="order.oilCarWeight" type="number" required placeholder="请输入磅秤数"></yd-input>
                    <span slot="right"> KG</span>
                </yd-cell-item>
                <!--<yd-cell-item>
                    <span slot="left">图片上传：</span>
                    <label slot="right" class="input-file"><input title="浏览文件" type="file" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />选择图片…</label>
                </yd-cell-item>-->
            </yd-cell-group>
            <div class="align-center" v-show="currStep == 4">
                <yd-button type="warning" class="mtop20" slot="right" style="width: 90%;height:38px;margin-top: 20px;" @click.native="uploadByWeixin">磅秤拍照上传</yd-button>
                <yd-button style="width:90%;height:38px;margin-top: 20px;" type="primary" @click.native="currStep -= 1">← 上一步：加油</yd-button>
                <yd-button style="width:90%;height:38px;margin-top: 20px;" type="primary" @click.native="changeState(5)" :disabled="order.oilCarWeight <= 0 || order.oilCarWeightPic == null || order.oilCarWeightPic == ''">下一步：完工 →</yd-button>
            </div>
            <div class="align-center first-group" v-show="currStep == 4">
                <yd-lightbox class="img-wrap">
                    <yd-lightbox-img :src="order.oilCarWeightPic"></yd-lightbox-img>
                </yd-lightbox>
            </div>
            <!--打印-->
            <div class="align-center" v-show="currStep == 5">
                <yd-button style="width:90%;height:38px;" type="hollow" @click.native="getPrintPonderation(order.id, '地磅室')">打印【过磅单】到【地磅室】</yd-button>
                <yd-button style="width:90%;height:38px;margin-top:20px;" type="hollow" @click.native="getPrintDeliver(order.id, '地磅室')">打印【陆上送货单】到【地磅室】</yd-button>
                <yd-button style="width:90%;height:38px;margin-top:20px;" type="hollow" @click.native="getPrintLandload(order.id, '地磅室');">打印【陆上装车单】到【地磅室】</yd-button>
            </div>
            <!--施工明细-->
            <div v-show="currStep == 5" class="mtop20">
                <div style="background-color: yellowgreen;padding: 10px; text-align: center; color: white; font-size: .3rem">实际加：{{order.oilCount}}{{order.unit}} | 应加：{{order.count}}{{order.unit}} | 相差{{order.diffOil}}{{order.unit}}</div>
                <yd-cell-group title="过磅明细" class="mtop20">
                    <yd-cell-item>
                        <span slot="right" style="font-weight: bold">{{order.product? order.product.name : ""}} - {{order.count}}{{order.unit}}</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">毛重：</span>
                        <span slot="right">{{order.oilCarWeight}} KG</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">皮重：</span>
                        <span slot="right">{{order.emptyCarWeight}} KG</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">净重：</span>
                        <span slot="right">{{order.diffWeight}} KG</span>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-cell-group title="油表明细">
                    <yd-cell-item>
                        <span slot="left">加油前表数：</span>
                        <span slot="right">{{lastorder.instrument1}}</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">加油后表数：</span>
                        <span slot="right">{{order.instrument1}}</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">实际加油数量：</span>
                        <span slot="right">{{order.instrument1 - lastorder.instrument1}}升</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">测量密度：</span>
                        <span slot="right">{{order.density}}</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">实际加油数量 × 密度 ÷ 1000 = </span>
                        <span slot="right">{{order.oilCount}}{{order.unit}}</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">施工人：</span>
                        <span slot="right">{{order.worker}}</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <div slot="left">毛重图片：</div>
                        <div slot="right"><div class="img-wrap"><img :src="this.order.oilCarWeightPic" /></div></div>
                    </yd-cell-item>
                    <yd-cell-item>
                        <div slot="left">皮重图片：</div>
                        <div slot="right"><div class="img-wrap"><img :src="this.order.emptyCarWeightPic" /></div></div>
                    </yd-cell-item>
                </yd-cell-group>
                <div class="align-center" v-show="currStep == 5">
                    <yd-button class="mtop20" style="width:90%;height:38px" type="primary" @click.native="putRestart">重新施工</yd-button>
                </div>
            </div>
        </div>
        <!--popup订单选择-->
        <yd-popup v-model="showOrders" position="right" width="70%">
            <yd-pullrefresh :callback="getOrders">
                <yd-cell-group>
                    <yd-cell-item v-for="o in orders" :key="o.id" @click.native="orderclick(o)" arrow>
                        <div slot="left" style="padding:.2rem 0 .2rem">
                            <p>{{o.name}}</p>
                            <p class="col-gray">车牌：{{o.carNo}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p class="col-gray">{{o.product.name}}</p>
                            <p class="col-gray">{{o.count}}{{o.unit}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-pullrefresh>
        </yd-popup>
        <!--popup销售仓选择-->
        <yd-popup v-model="showStores" position="right">
            <yd-cell-group title="请选择销售仓">
                <yd-cell-item v-for="s in stores" :key="s.id" @click.native="storeclick(s)">
                    <div slot="left">
                        <p>{{s.name}}</p>
                    </div>
                    <div slot="right">
                        <p class="col-light-gray">{{s.value}}</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
    </div>
</template>

<script src="./landload.ts" />

