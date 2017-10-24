<style>
    .center {
        text-align: center;
    }

    .img-wrap {
        display: inline-block;
    }
    .img-wrap img {
        width: 100%;
    }
</style>
<template>
    <div id="root">
        <div style="text-align: center; margin-top: .4rem">
            <yd-button style="width:90%" type="primary" @click.native="showPurchases = true">进油单{{purchase.name? '：' + purchase.name : ''}}</yd-button>
        </div>
        <yd-step :current="currStep" style="margin: .4rem 0 .4rem">
            <yd-step-item>
                <span slot="bottom">油车过磅</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">化验</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">选择油仓</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">卸油</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">空车过磅</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">完工</span>
            </yd-step-item>
        </yd-step>
        <div v-show="currStep == 1">
            <yd-cell-group title="油车过磅">
                <yd-cell-item>
                    <span slot="left">测量密度：</span>
                    <yd-input slot="right" v-model="purchase.density" type="number" required placeholder="请输入测量密度"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">磅秤数（毛重）：</span>
                    <yd-input slot="right" v-model="purchase.scaleWithCar" type="number" required placeholder="请输入磅秤数"></yd-input>
                    <span slot="right">吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">图片上传：</span>
                    <input slot="left" type="file" value="选择图片" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />
                </yd-cell-item>
            </yd-cell-group>
            <div class="center">
                <yd-button style="width:90%" type="primary" @click.native="goNext" :disabled="purchase.density <= 0 || purchase.scaleWithCar <= 0 || !isScaleWithCarUpload">前往化验</yd-button>
            </div>
            <div style="text-align: center;margin-top: .2rem">
                <div class="img-wrap">
                    <img :src="purchase.scaleWithCarPic" />
                </div>
                
            </div>
        </div>
        <div class="center" v-show="currStep == 2">
            <yd-button style="width:90%" type="primary" @click.native="goNext">已化验</yd-button>
        </div>
        <div class="center" v-show="currStep == 3">
            <yd-cell-group title="选择卸油油仓对应油表">
                <yd-cell-item arrow type="label" v-for="(ts,idx) in toStores" :key="ts.id">
                    <span slot="left">{{ts.name}}：</span>
                    <!--<yd-input slot="right" required type="number" v-model="toStoreCounts[idx]"></yd-input>-->
                    <select slot="right" v-model="instruments[idx]">
                        <option value="">请选择</option>
                        <option value="表1">表1 - {{lastPurchase.instrument1}}</option>
                        <option value="表2">表2 - {{lastPurchase.instrument2}}</option>
                        <option value="表3">表3 - {{lastPurchase.instrument3}}</option>
                    </select>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button style="width:90%" type="primary" @click.native="showStores = true">选择油仓</yd-button>
            <yd-button style="width:90%; margin-top:10px;" type="primary" @click.native="toStoresOKclick" :disabled="isPrevent2">提交，下一步</yd-button>
        </div>
        <div class="center" v-show="currStep == 4">
            <yd-cell-group title="卸油前表数（上次卸油）">
                <yd-cell-item v-show="isHas('表1')">
                    <span slot="left">表数1：</span>
                    <yd-input slot="right" v-model="lastPurchase.instrument1" type="number" required></yd-input>
                </yd-cell-item>
                <yd-cell-item v-show="isHas('表2')">
                    <span slot="left">表数2：</span>
                    <yd-input slot="right" v-model="lastPurchase.instrument2" type="number" required></yd-input>
                </yd-cell-item>
                <yd-cell-item v-show="isHas('表3')">
                    <span slot="left">表数3：</span>
                    <yd-input slot="right" v-model="lastPurchase.instrument3" type="number" required></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <yd-cell-group title="卸油后表数（本次卸油）">
                <yd-cell-item v-show="isHas('表1')">
                    <span slot="left">表数1：</span>
                    <yd-input slot="right" v-model="purchase.instrument1" type="number" required placeholder="请输入卸油表数1"></yd-input>
                    <span slot="right" style="width: 150px">油量：{{diff1}}升</span>
                </yd-cell-item>
                <yd-cell-item v-show="isHas('表2')">
                    <span slot="left">表数2：</span>
                    <yd-input slot="right" v-model="purchase.instrument2" type="number" required placeholder="请输入卸油表数2"></yd-input>
                    <span slot="right" style="width: 150px">油量：{{diff2}}升</span>
                </yd-cell-item>
                <yd-cell-item v-show="isHas('表3')">
                    <span slot="left">表数3：</span>
                    <yd-input slot="right" v-model="purchase.instrument3" type="number" required placeholder="请输入卸油表数3"></yd-input>
                    <span slot="right" style="width: 150px">油量：{{diff3}}升</span>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button style="width:90%" type="primary" @click.native="goNext">卸油结束，前往过磅</yd-button>
        </div>
        <div v-show="currStep == 5">
            <yd-cell-group title="空车过磅">
                <yd-cell-item>
                    <span slot="left">磅秤数（皮重）：</span>
                    <yd-input slot="right" v-model="purchase.scale" type="number" required placeholder="请输入磅秤数"></yd-input>
                    <span slot="right">吨</span>
                </yd-cell-item>
                
                <yd-cell-item>
                    <span slot="left">图片上传：</span>
                    <input slot="left" type="file" value="选择图片" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />
                </yd-cell-item>
            </yd-cell-group>
            <div class="center">
                <yd-button style="width:90%" type="primary" @click.native="goNext" :disabled="purchase.scale <= 0 || !isScaleUpload">完工确认</yd-button>
            </div>
            <div style="text-align: center; margin-top: .2rem">
                <div class="img-wrap">
                    <img :src="purchase.scalePic" />
                </div>
            </div>
        </div>

        <!--施工明细-->
        <yd-cell-group title="施工明细" v-show="currStep == 6">
            <yd-cell-item>
                <span slot="right" style="font-weight: bold">{{purchase.product? purchase.product.name : ""}} - {{purchase.count}}吨</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">毛重（磅秤）：</span>
                <span slot="right">{{purchase.scaleWithCar}}吨</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">皮重（磅秤）：</span>
                <span slot="right">{{purchase.scale}}吨</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">净重（计算）：</span>
                <span slot="right">{{purchase.scaleWithCar - purchase.scale}}吨</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">相差：</span>
                <span slot="right" style="color: red; font-weight: bold">{{purchase.count - (purchase.scaleWithCar - purchase.scale)}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">卸入油仓：</span>
                <div slot="right">
                    <p v-for="ts in purchase.toStoresList" :key="ts.id">
                        {{ts.name}} - {{ts.count}}升
                    </p>
                </div>
            </yd-cell-item>
            <yd-cell-item v-show="isHas('表数1')">
                <span slot="left">表1：</span>
                <span slot="right">{{purchase.instrument1}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="isHas('表数2')">
                <span slot="left">表2：</span>
                <span slot="right">{{purchase.instrument2}}</span>
            </yd-cell-item>
            <yd-cell-item v-show="isHas('表数3')">
                <span slot="left">表3：</span>
                <span slot="right">{{purchase.instrument3}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">施工人：</span>
                <span slot="right">{{purchase.lastUpdatedBy}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <div slot="left">毛重图片：</div>
                <div slot="right"><div class="img-wrap"><img :src="this.purchase.scaleWithCarPic" /></div></div>
            </yd-cell-item>
            <yd-cell-item>
                <div slot="left">皮重图片：</div>
                <div slot="right"><div class="img-wrap"><img :src="this.purchase.scalePic" /></div></div>
            </yd-cell-item>
        </yd-cell-group>
        <div class="center" v-show="currStep == 6">
            <yd-button style="width:90%" type="primary" @click.native="putRestart">重新施工</yd-button>
        </div>
        <yd-popup v-model="showPurchases" position="right" width="70%">
            <yd-cell-group>
                <yd-cell-item v-for="p in purchases" :key="p.id" @click.native="purchaseclick(p)" arrow style="padding: 10px 0 10px">
                    <div slot="left" style="padding:.2rem 0 .2rem">
                        <p>{{p.name}}</p>
                        <p style="color:lightgray;font-size:12px">{{p.carNo}} - {{p.trailerNo}}</p>
                        <p style="color:lightgray;font-size:12px">{{p.driver1}} {{p.driver2}}</p>
                    </div>
                    <div slot="right" style="text-align: left;margin-right: 5px">
                        <p style="color: forestgreen">{{strState(p.state)}}</p>
                        <p style="color:gray">{{p.product.name}}</p>
                        <p style="color:gray">{{p.count}}吨</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--popup油仓选择-->
        <!--<yd-popup v-model="showStores" position="right" width="70%">
            <yd-cell-group title="请选择油仓">
                <yd-cell-item v-for="s in stores" :key="s.id" @click.native="storeclick(s)">
                    <div slot="left">
                        <p>{{s.name}}</p>
                        <p style="color:lightcoral">{{strClass(s.storeClass)}}</p>
                    </div>
                    <div slot="right">
                        <p style="color:lightgray">当前：{{s.value}}升</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>-->
        <yd-popup v-model="showStores" position="right" width="70%">
            <div style="text-align: center;margin: 10px 0">
                <yd-button type="primary" style="width: 80%" @click.native="storeOKclick()" :disabled="selectedStIds.length < 1">选好了</yd-button>
            </div>
            <yd-checklist align="right" v-model="selectedStIds">
                <yd-checklist-item v-for="s in stores" :key="s.id" :val="s.id">
                    <div style="height: 50px;line-height: 50px;">{{s.name}}</div>
                </yd-checklist-item>
            </yd-checklist>
        </yd-popup>
    </div>
</template>

<script src="./unload.ts" />