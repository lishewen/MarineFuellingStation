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
        <div class="align-center first-group">
            <yd-button style="width:90%;height:38px" type="primary" @click.native="showPurchases = true">进油单{{purchase.name? '：' + purchase.name + ' / ' + purchase.count + '吨' : ''}}</yd-button>
        </div>
        <yd-step :current="currStep" style="margin: .4rem 0 .4rem">
            <yd-step-item>
                <span slot="bottom">油车过磅</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">化验</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">空车过磅</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">卸油</span>
            </yd-step-item>
            <yd-step-item>
                <span slot="bottom">完工</span>
            </yd-step-item>
        </yd-step>
        <div v-show="currStep == 1">
            <yd-cell-group title="第一步：油车过磅 称毛重">
                <yd-cell-item>
                    <span slot="left">毛重：</span>
                    <yd-input slot="right" v-model="purchase.scaleWithCar" type="number" required placeholder="请输入磅秤数"></yd-input>
                    <span slot="right">吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">图片上传：</span>
                    <label slot="right" class="input-file"><input title="浏览文件" type="file" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />选择图片…</label>
                </yd-cell-item>
            </yd-cell-group>
            <div class="center">
                <yd-button class="mtop20" style="width:90%;height:38px;" type="primary" @click.native="goNext" :disabled="purchase.scaleWithCar <= 0 || !isScaleWithCarUpload">下一步：化验 →</yd-button>
            </div>
            <div class="align-center first-group">
                <yd-lightbox class="img-wrap">
                    <yd-lightbox-img :src="purchase.scaleWithCarPic"></yd-lightbox-img>
                </yd-lightbox>
            </div>
        </div>
        <div class="center" v-show="currStep == 2">
            <yd-cell-group title="第二步：化验">
                <yd-cell-item>
                    <span slot="left">测量密度：</span>
                    <yd-input slot="right" v-model="purchase.density" type="number" required placeholder="请输入测量密度"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button style="width:90%;height:38px; margin-top:30px;" type="primary" @click.native="currStep -= 1">← 上一步：油车过磅</yd-button>
            <yd-button style="width:90%;height:38px; margin-top:20px;" type="primary" @click.native="goNext" :disabled="purchase.density <= 0">下一步：空车过磅 →</yd-button>
        </div>
        <div v-show="currStep == 3">
            <yd-cell-group title="第三步：空车过磅 称皮重">
                <yd-cell-item>
                    <span slot="right">毛重：{{purchase.scaleWithCar}}吨</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">皮重：</span>
                    <yd-input slot="right" v-model="purchase.scale" type="number" required placeholder="请输入磅秤数"></yd-input>
                    <span slot="right">吨</span>
                </yd-cell-item>

                <yd-cell-item>
                    <span slot="left">图片上传：</span>
                    <label slot="right" class="input-file"><input title="浏览文件" type="file" accept="image/png,image/gif,image/jpeg" @change="uploadfile" />选择图片…</label>
                </yd-cell-item>
            </yd-cell-group>
            <div class="center">
                <yd-button style="width:90%;height:38px;" type="primary" @click.native="currStep -= 1" class="mtop20">← 上一步：化验</yd-button>
                <yd-button style="width:90%;height:38px; margin-top:20px;" type="primary" @click.native="goNext" :disabled="purchase.scale <= 0 || !isScaleUpload">下一步：卸油 →</yd-button>
            </div>
            <div class="align-center first-group">
                <yd-lightbox class="img-wrap">
                    <yd-lightbox-img :src="purchase.scalePic"></yd-lightbox-img>
                </yd-lightbox>
            </div>
        </div>
        <div class="center" v-show="currStep == 4">
            <yd-button style="width:90%;height:38px;" type="hollow" @click.native="getPrintUnloadPond(purchase.id, '地磅室')">打印【过磅单】到【地磅室】</yd-button>
            <yd-button style="width:90%;height:38px; margin-top:10px;" type="hollow" @click.native="getPrintUnloadPond(purchase.id, '收银台')">打印【过磅单】到【收银台】</yd-button>
            <yd-button style="width:90%;height:38px; margin: 20px 0 20px" type="warning" @click.native="showStoresclick()">>>> 请选择油仓 <<<</yd-button>
            <yd-cell-group :title="ts.name" v-for="ts in purchase.toStoresList" :key="ts.id">
                <yd-cell-item>
                    <span slot="left">表数（卸油前）：</span>
                    <yd-input slot="right" required type="number" v-model="ts.instrumentBf"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">表数（卸油后）：</span>
                    <yd-input slot="right" required type="number" v-model="ts.instrumentAf"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="right">油量：{{ts.instrumentAf - ts.instrumentBf}}升</span>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button style="width:90%;height:38px; margin-top:20px;" type="primary" @click.native="currStep -= 1">上一步：空车过磅</yd-button>
            <yd-button style="width:90%;height:38px; margin-top:20px;" type="primary" @click.native="toStoresOKclick" :disabled="!isFinish()">下一步：完工</yd-button>
        </div>
        <!--打印-->
        <div class="center" v-show="currStep == 5">
            <yd-button style="width:90%;height:38px" type="hollow" @click.native="getPrintUnload(purchase.id, '收银台');">打印到【收银台】</yd-button>
            <yd-button class="mtop10" style="width:90%;height:38px" type="hollow" @click.native="getPrintUnload(purchase.id, '地磅室')">打印到【地磅室】</yd-button>
        </div>
        <!--施工明细-->
        <yd-cell-group title="施工明细" v-show="currStep == 5" class="mtop20">
            <yd-cell-item>
                <span slot="right" style="font-weight: bold">{{purchase.product? purchase.product.name : ""}} - {{purchase.count}}吨</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">密度：</span>
                <span slot="right">{{purchase.density}}</span>
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
                <span slot="right" class="col-red" style="font-weight: bold">{{(purchase.scaleWithCar - purchase.scale) - purchase.count}}</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">卸入油仓：</span>
                <div slot="right">
                    <p v-for="ts in purchase.toStoresList" :key="ts.id">
                        {{ts.name}} - {{ts.count}}升
                    </p>
                </div>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">施工人：</span>
                <span slot="right">{{purchase.worker}}</span>
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
        <div class="center" v-show="currStep == 5">
            <yd-button style="width:90%;height:38px" type="primary" @click.native="putRestart">重新施工</yd-button>
        </div>
        <yd-popup v-model="showPurchases" position="right" width="70%">
            <yd-cell-group>
                <yd-cell-item v-for="p in purchases" :key="p.id" @click.native="purchaseclick(p)" arrow style="padding: 10px 0 10px">
                    <div slot="left" style="padding:.2rem 0 .2rem">
                        <p>{{p.name}}</p>
                        <p class="col-light-gray font12">{{p.carNo}} - {{p.trailerNo}}</p>
                        <p class="col-light-gray font12">{{p.driver1}} {{p.driver2}}</p>
                    </div>
                    <div slot="right" style="text-align: left;margin-right: 5px">
                        <p style="color: forestgreen">{{strState(p.state)}}</p>
                        <p class="col-gray">{{p.product.name}}</p>
                        <p class="col-gray">{{p.count}}吨</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--popup油仓选择-->
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

