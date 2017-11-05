<template>
    <div id="root">
        <yd-cell-group title="必填" class="first-group">

            <yd-cell-item>
                <yd-radio-group slot="left" v-model="model.placeType">
                    <yd-radio val="0">陆上</yd-radio>
                    <yd-radio val="1">水上</yd-radio>
                </yd-radio-group>
            </yd-cell-item>

            <yd-cell-item>
                <yd-radio-group slot="left" v-model="model.clientType">
                    <yd-radio val="0">个人</yd-radio>
                    <yd-radio val="1">公司</yd-radio>
                </yd-radio-group>
            </yd-cell-item>

            <yd-cell-item v-show="show1" arrow @click.native="showcompany = true">
                <span slot="left">所属公司：</span>
                <span slot="right">{{model.company != null? model.company.name : ""}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">{{labelBoatOrCar}}：</span>
                <yd-input slot="right" placeholder="请输入" v-model="model.carNo" required></yd-input>
            </yd-cell-item>

            <yd-cell-item arrow @click.native="showsales = true">
                <span slot="left">跟进销售：</span>
                <span slot="right">{{model.followSalesman}}</span>
            </yd-cell-item>

            <yd-cell-item arrow @click.native="oilshow = true">
                <span slot="left">默认商品：</span>
                <span slot="right">{{model.product != null ? model.product.name : ""}}</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">联系人：</span>
                <yd-input slot="right" v-model="model.contact" regex="" placeholder="请输入联系人" required></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">联系电话：</span>
                <yd-input slot="right" v-model="model.mobile" type="number" placeholder="请输入联系电话" required></yd-input>
            </yd-cell-item>

        </yd-cell-group>
        <yd-cell-group title="选填" class="first-group">

            <yd-cell-item>
                <span slot="left">身份证号：</span>
                <yd-input slot="right" v-model="model.idCard" regex="" placeholder="请输入身份证号"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">地址：</span>
                <yd-input slot="right" v-model="model.address" regex="" placeholder="请输入地址"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">固定电话：</span>
                <yd-input slot="right" v-model="model.phone" regex="" type="number" placeholder="请输入固定电话"></yd-input>
            </yd-cell-item>

        </yd-cell-group>

        <yd-cell-group title="设置（选填）" class="first-group">

            <yd-cell-item>
                <span slot="left">最高挂账金额：</span>
                <yd-input slot="right" v-model="model.maxOnAccount" regex="" placeholder="0 - 不指定" type="number"></yd-input>
            </yd-cell-item>

        </yd-cell-group>

        <div>
            <yd-button size="large" type="primary" @click.native="saveclientclick">保存</yd-button>
        </div>
        <yd-actionsheet :items="oiloptions" v-model="oilshow" cancel="取消"></yd-actionsheet>
        <!--popup公司选择列表-->
        <yd-popup v-model="showcompany" position="right" width="70%">
            <yd-cell-group>
                <yd-search v-model="svCompany1" />
                <yd-cell-item arrow type="radio" v-for="co in companys" :key="co.id" @click.native="selectcompanyclick(co)">
                    <span slot="left">{{co.name}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--popup销售列表-->
        <yd-popup v-model="showsales" position="right">
            <yd-cell-group title="必填">
                <yd-cell-item arrow type="radio" v-for="s in sales" :key="s.userid" @click.native="selectsalesclick(s)">
                    <span slot="left">{{s.name}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
    </div>
</template>

<script src="./clientdetail.ts" />
