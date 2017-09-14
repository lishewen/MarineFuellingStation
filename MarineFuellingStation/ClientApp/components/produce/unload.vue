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
        <yd-step :current="purchase.state" style="margin: .4rem 0 .4rem">
            <yd-step-item>
                <span slot="bottom">已到达</span>
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
        <div class="center" v-show="currStep == 0">
            <yd-button style="width:90%" type="primary" @click.native="changeState(2)">到达确认</yd-button>
        </div>
        <yd-cell-group title="油车过磅" v-show="currStep == 2">
            <yd-cell-item>
                <span slot="left">磅秤数：</span>
                <yd-input slot="right" v-model="purchase.scaleWithCar" type="number" required placeholder="请输入磅秤数"></yd-input>
                <span slot="right">吨</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">图片上传：</span>
                <input slot="left" type="file" value="选择图片" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />
            </yd-cell-item>
            <div class="center">
                <yd-button style="width:90%" type="primary" @click.native="changeState(3)">前往化验</yd-button>
            </div>
        </yd-cell-group>
        <div class="center" v-show="currStep == 3">
            <yd-button style="width:90%" type="primary" @click.native="changeState(4)">已化验，前往施工</yd-button>
        </div>
        <div class="center" v-show="currStep == 4">
            <yd-button style="width:90%" type="primary" @click.native="changeState(5)">卸油结束，前往过磅</yd-button>
        </div>
        <yd-cell-group title="空车过磅" v-show="currStep == 5">
            <yd-cell-item>
                <span slot="left">磅秤数：</span>
                <yd-input slot="right" v-model="purchase.scale" type="number" required placeholder="请输入磅秤数"></yd-input>
                <span slot="right">吨</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">图片上传：</span>
                <input slot="left" type="file" value="选择图片" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />
            </yd-cell-item>
            <div class="center">
                <yd-button style="width:90%" type="primary" @click.native="changeState(6)">完工确认</yd-button>
            </div>
        </yd-cell-group>
        <yd-popup v-model="showPurchases" position="right" width="70%">
            <yd-cell-group>
                <yd-cell-item v-for="p in purchases" v-show="p.state != 6" :key="p.id" @click.native="purchaseclick(p)" arrow>
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
    </div>
</template>

<script src="./unload.ts" />