<template>
    <div>
        <yd-tab :callback="change">
            <yd-tab-panel label="水上">
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="waterinfinitescroll">
                        <yd-cell-item slot="list" arrow v-for="p in waterPlans" :key="p.id" @click.native="godetail(p)">
                            <div slot="left">
                                <p class="font16">{{p.carNo}}</p>
                                <p class="col-light-gray font12">{{p.name}}</p>
                            </div>
                            <div slot="right" class="align-left" style="margin-right: 5px;padding: 0.2rem 0 .2rem">
                                <p class="col-gray"><span class="col-green font16">{{p.count}}</span> {{p.unit}} / {{p.oilName}}</p>
                                <p :class="stateClass(p.state)" style="text-align: right">{{strState(p.state)}}</p>
                                <p class="align-right col-salmon">{{formatDate(p.createdAt)}}</p>
                            </div>
                        </yd-cell-item>
                        <!-- 数据全部加载完毕显示 -->
                        <span slot="doneTip">没有数据啦~~</span>
                        <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                        <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                    </yd-infinitescroll>
                </yd-cell-group>
           </yd-tab-panel>
            <yd-tab-panel label="陆上">
                <yd-infinitescroll :callback="loadList" ref="landinfinitescroll">
                    <yd-cell-item slot="list" arrow v-for="p in landPlans" :key="p.id" @click.native="godetail(p)">
                        <div slot="left">
                            <p class="font16">{{p.carNo}}</p>
                            <p class="col-light-gray font12">{{p.name}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px;padding: 0.2rem 0 .2rem">
                            <p class="col-gray"><span class="font16 col-green">{{p.count}}</span> {{p.unit}} / {{p.oilName}}</p>
                            <p :class="stateClass(p.state)" style="text-align: right">{{strState(p.state)}}</p>
                            <p class="col-salmon align-right">{{formatDate(p.createdAt)}}</p>
                        </div>
                    </yd-cell-item>
                    <!-- 数据全部加载完毕显示 -->
                    <span slot="doneTip">没有数据啦~~</span>
                    <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                    <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                </yd-infinitescroll>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>
<script src="./planboard.ts"></script>
<style src="./planboard.css" />
<style src="./../website.css" />