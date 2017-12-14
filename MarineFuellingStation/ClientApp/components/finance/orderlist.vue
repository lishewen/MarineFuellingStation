<style>
    .navBtn {
        width: 1rem;
        height: .8rem;
    }
</style>
<template>
    <div id="root">
            <div class="align-center first-group font16">
                <div class="align-center" style="margin-top: .2rem">
                    <yd-button style="width:90%;height: 38px" type="primary" @click.native="showSalesmans = true">销售员{{selectedsales == '' ? selectedsales : '：' + selectedsales}}</yd-button>
                </div>
                <div style="display: flex; line-height: .6rem; margin-top: .2rem">
                    <yd-datetime type="date" v-model="startDate" slot="right"></yd-datetime>
                    <div>至</div>
                    <yd-datetime type="date" v-model="endDate" slot="right"></yd-datetime>
                    <yd-button type="primary" style="width: 2rem; height: .6rem; margin-right: .2rem" @click.native="query">查询</yd-button>
                </div>
            </div>
            <div style="text-align: center;padding: 10px 0 10px">
                <div v-for="(f,idx) in filterBtns" style="display: inline-block" :key="f.id">
                    <yd-button class="navBtn" type="warning" v-if="f.actived" @click.native="switchBtn(f,idx)">{{f.name}}</yd-button>
                    <yd-button class="navBtn" type="hollow" v-if="!f.actived" @click.native="switchBtn(f,idx)">{{f.name}}</yd-button>
                </div>
            </div>
            <yd-cell-group :title="getTotalSalesComm()">
                <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                    <yd-cell-item slot="list" arrow v-for="o in orders" :key="o.id" @click.native="godetail(o.id)" style="padding: .2rem 0 .2rem">
                        <div slot="left">
                            <p>{{o.carNo}}</p>
                            <p class="col-gray" style="margin-top: .2rem">{{formatDate(o.createAt)}}</p>
                            <p class="col-light-gray font12">{{o.name}}</p>
                        </div>
                        <div class="col-coral align-left" slot="right" style="width:80px;">提：￥{{o.salesCommission}}</div>
                        <div slot="right" style="padding-right: .1rem">
                            <p class="col-green" style="padding-left:10px">{{strState(o)}}</p>
                            <p class="col-gray" style="margin-top:0.1rem">{{strPayState(o)}}</p>
                        </div>
                    </yd-cell-item>
                    <!-- 数据全部加载完毕显示 -->
                    <span slot="doneTip">没有数据啦~~</span>
                    <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                    <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                </yd-infinitescroll>
            </yd-cell-group>
        <!--popup销售列表-->
        <yd-popup v-model="showSalesmans" position="right">
            <yd-cell-group title="必填">
                <yd-cell-item arrow type="radio" v-for="s in sales" :key="s.userid" @click.native="selectsalesclick(s)">
                    <span slot="left">{{s.name}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
</div>
</template>

<script src="./orderlist.ts" />

