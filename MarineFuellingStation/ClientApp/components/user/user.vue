<template>
    <div id="root">
        <yd-cell-group>
            <yd-cell-item arrow @click.native="userClick(user)" v-for="user in users" :key="user.id">
                <div slot="left">
                    <p>{{user.name}}</p>
                    <p style="color:lightgray;font-size:12px" v-show="user.isleader==0">普通职员</p>
                    <p style="color:lightgray;font-size:12px" v-show="user.isleader==1">上级</p>
                </div>
                <div slot="right" style="text-align: right;margin-right: 5px">
                    <p>
                        <span style="color:gray; font-size: 18px">{{departmentdict[user.department[0]]}}</span>
                    </p>
                </div>
            </yd-cell-item>
        </yd-cell-group>
        <yd-actionsheet :items="departmentoptions" v-model="departmentshow" cancel="取消"></yd-actionsheet>
        <yd-actionsheet :items="userItems" v-model="usershow" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./user.ts" />