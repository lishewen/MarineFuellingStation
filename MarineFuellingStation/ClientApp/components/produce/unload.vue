<style>
    .center{
        text-align:center;
    }
</style>
<template>
    <div id="root">
        <div style="text-align: center; margin-top: .4rem">
            <yd-button style="width:90%" type="primary" @click.native="showPurchases = true">采购单{{purchase.name? '：' + purchase.name : ''}}</yd-button>
        </div>
        <yd-step :current="currStep" style="margin: .4rem 0 .4rem">
            <yd-step-item>
                <span slot="bottom">选择油仓</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">油车过磅</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">化验</span>
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
        <div class="center" v-show="currStep == 1">
            <yd-button style="width:90%" type="primary" @click.native="showStores = true">选择油仓</yd-button>
        </div>
        <div v-show="currStep == 2">
            <yd-cell-group title="油车过磅">
                <yd-cell-item>
                    <span slot="left">测量密度：</span>
                    <yd-input slot="right" v-model="purchase.density" type="number" required placeholder="请输入测量密度"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">磅秤数：</span>
                    <yd-input slot="right" v-model="purchase.scaleWithCar" type="number" required placeholder="请输入磅秤数"></yd-input>
                    <span slot="right">吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">图片上传：</span>
                    <input slot="left" type="file" value="选择图片" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />
                </yd-cell-item>
            </yd-cell-group>
            <div class="center">
                <yd-button style="width:90%" type="primary" @click.native="goNext" :disabled="isPrevent">前往化验</yd-button>
            </div>
        </div>
        <div class="center" v-show="currStep == 3">
            <yd-button style="width:90%" type="primary" @click.native="goNext">已化验，前往施工</yd-button>
        </div>
        <div class="center" v-show="currStep == 4">
            <yd-cell-item>
                <span slot="left">卸油后表数1：</span>
                <yd-input slot="right" v-model="purchase.instrument1" type="number" required placeholder="请输入卸油表数1"></yd-input>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">卸油后表数2：</span>
                <yd-input slot="right" v-model="purchase.instrument2" type="number" required placeholder="请输入卸油表数2"></yd-input>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">卸油后表数3：</span>
                <yd-input slot="right" v-model="purchase.instrument3" type="number" required placeholder="请输入卸油表数3"></yd-input>
            </yd-cell-item>
            <yd-button style="width:90%" type="primary" @click.native="goNext">卸油结束，前往过磅</yd-button>
        </div>
        <div v-show="currStep == 5">
            <yd-cell-group title="空车过磅">
                <yd-cell-item>
                    <span slot="left">磅秤数：</span>
                    <yd-input slot="right" v-model="purchase.scale" type="number" required placeholder="请输入磅秤数"></yd-input>
                    <span slot="right">吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">图片上传：</span>
                    <input slot="left" type="file" value="选择图片" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />
                </yd-cell-item>
            </yd-cell-group>
            <div class="center">
                <yd-button style="width:90%" type="primary" @click.native="goNext" :disabled="isPrevent1">完工确认</yd-button>
            </div>
        </div>
        <yd-popup v-model="showPurchases" position="right" width="70%">
            <yd-cell-group>
                <yd-cell-item v-for="p in purchases" v-show="p.state != 7" :key="p.id" @click.native="purchaseclick(p)" arrow>
                    <div slot="left" style="padding:.2rem 0 .2rem">
                        <p>{{p.name}}</p>
                        <p style="color:lightgray;font-size:12px">{{p.carNo}} - {{p.trailerNo}}</p>
                        <p style="color:lightgray;font-size:12px">{{p.driver1}} {{p.driver2}}</p>
                    </div>
                    <div slot="right" style="text-align: left;margin-right: 5px">
                        <p style="color:gray">{{p.product.name}}</p>
                        <p style="color:gray">{{p.count}}吨</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--popup油仓选择-->
        <yd-popup v-model="showStores" position="right">
            <yd-cell-group title="请选择销售仓">
                <yd-cell-item v-for="s in stores" :key="s.id" @click.native="storeclick(s)">
                    <div slot="left">
                        <p>{{s.name}}</p>
                    </div>
                    <div slot="right">
                        <p style="color:lightgray">{{s.value}}</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
    </div>
</template>

<script src="./unload.ts" />