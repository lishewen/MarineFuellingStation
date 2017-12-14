<template>
    <div id="root">
        <yd-tab :callback="change">
            <yd-tab-panel label="所有分类">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="ptClick(pt)" v-for="pt in pts" :key="pt.id">
                        <div slot="left" style="margin: 10px 0 10px 0" class="lineheight24">
                            <p>{{pt.name}}</p>
                            <p class="col-light-gray font14">{{pt.products.length}}个商品</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel :label="isAddProduct?'添加':'编辑'" :active="isAddProduct?false:true">
                <yd-cell-group>
                    <yd-cell-item @click.native="ptshow = true">
                        <span slot="left">所属分类：</span>
                        <span slot="left">{{selectptname}}</span>
                        <span slot="right"><yd-button type="warning" @click.native="addpt($event)">添加</yd-button></span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <yd-radio-group v-model="currentproduct.isForLand" slot="left">
                            <yd-radio val="false">水上</yd-radio>
                            <yd-radio val="true">陆上</yd-radio>
                        </yd-radio-group>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">名称：</span>
                        <yd-input slot="right" v-model="currentproduct.name" required placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">最近一次单价：</span>
                        <yd-input slot="right" type="number" v-model="currentproduct.lastPrice" placeholder="请输入最新售价"></yd-input>
                        <span slot="right" style="width: 60px">元 / 升</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">最低单价：</span>
                        <yd-input slot="right" type="number" v-model="currentproduct.minPrice" placeholder="请输入最低单价" required></yd-input>
                        <span slot="right" style="width: 60px">元 / 升</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">最低开票单价：</span>
                        <yd-input slot="right" type="number" v-model="currentproduct.minInvoicePrice" placeholder="请输入最低开票单价" required></yd-input>
                        <span slot="right" style="width: 60px">元 / 升</span>
                    </yd-cell-item>
                </yd-cell-group>
                <div class="align-center">
                    <yd-button v-show="isAddProduct" size="large" type="primary" @click.native="postProductclick">提交</yd-button>
                    <yd-button v-show="!isAddProduct" size="large" type="primary" @click.native="saveProductclick">保存</yd-button>
                </div>
            </yd-tab-panel>
        </yd-tab>
        <yd-actionsheet :items="ptoptions" v-model="ptshow" cancel="取消"></yd-actionsheet>
        <yd-popup v-model="show1" position="right" width="70%">
            <yd-cell-group :title="currentpt.name">
                <yd-cell-item arrow v-for="p in currentpt.products" :key="p.id" @click.native="editProductclick(p)">
                    <div slot="left">{{p.name}}</div>
                    <div slot="right" style="font-size: 14px">
                        <p class="col-green">￥{{p.minPrice}} / 升</p>
                        <p class="col-coral"> 开票￥{{p.minInvoicePrice}} / 升</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
            <div class="align-center">
                <yd-button size="large" type="primary" @click.native="editProductTypeclick">编辑分类</yd-button>
            </div>
        </yd-popup>
        <!--popup添加编辑分类-->
        <yd-popup v-model="show2" position="right" width="70%">
            <yd-cell-group :title="isAddType?'添加分类':'编辑分类'">
                <yd-cell-item>
                    <span slot="left">分类名称：</span>
                    <yd-input slot="left" v-model="ptName" required placeholder="请输入名称"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
                <yd-button v-show="isAddType" type="primary" size="large" @click.native="postProductTypeclick()">提交</yd-button>
                <yd-button v-show="!isAddType" type="primary" size="large" @click.native="saveProductTypeclick">保存</yd-button>
        </yd-popup>
    </div>
</template>

<script src="./product.ts" />

