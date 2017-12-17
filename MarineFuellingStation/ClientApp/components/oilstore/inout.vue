<style>
    .color_green{
        color:forestgreen
    }
    .color_red {
        color: red
    }
</style>
<template>
    <div id="root">
        <yd-tab :callback="change">

            <yd-tab-panel label="所有">
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                        <yd-cell-item slot="list" v-for="i in inAndOutLogs" :key="i.id">
                            <div slot="left" class="lineheight24" style="margin: 10px 0 10px 0">
                                <p><span :class="classState(i.type)">{{strLogType(i.type)}}</span><span> 【{{i.store.storeTypeName}}】 - {{i.store == null? "" : i.store.name}}</span></p>
                                <p class="col-light-gray font12">{{i.operators}}</p>
                                <p class="col-light-gray font12">{{formatDate(i.createdAt)}}</p>
                            </div>
                            <div slot="right" style="text-align: left;margin-right: 5px">
                                <p :class="classState(i.type)" style="font-size: 22px">{{i.value}}{{i.unit}}</p>
                            </div>
                        </yd-cell-item>
                        <!-- 数据全部加载完毕显示 -->
                        <span slot="doneTip">没有数据啦~~</span>
                        <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                        <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                    </yd-infinitescroll>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="出仓">
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll1">
                        <yd-cell-item slot="list" v-for="i in outLogs" :key="i.id">
                            <div slot="left" class="lineheight24" style="margin: 10px 0 10px 0">
                                <p><span :class="classState(i.type)">{{strLogType(i.type)}}</span><span> 【{{i.store.storeTypeName}}】 - {{i.store == null? "" : i.store.name}}</span></p>
                                <p class="col-light-gray font12">{{i.operators}}</p>
                                <p class="col-light-gray font12">{{formatDate(i.createAt)}}</p>
                            </div>
                            <div slot="right" class="align-right" style="margin-right: 5px">
                                <p :class="classState(i.type)" style="font-size: 22px">{{i.value}}{{i.unit}}</p>
                            </div>
                        </yd-cell-item>
                        <!-- 数据全部加载完毕显示 -->
                        <span slot="doneTip">没有数据啦~~</span>
                        <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                        <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                    </yd-infinitescroll>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="入仓">
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll2">
                        <yd-cell-item slot="list" v-for="i in inLogs" :key="i.id">
                            <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                                <p>
                                    <span :class="classState(i.type)">{{strLogType(i.type)}}</span><span> 【{{i.store.storeTypeName}}】 - {{i.store == null? "" : i.store.name}}</span>
                                </p>
                                <p class="col-light-gray font12">{{i.operators}}</p>
                                <p class="col-light-gray font12">{{formatDate(i.createAt)}}</p>
                            </div>
                            <div slot="right" class="align-left" style="margin-right: 5px">
                                <p :class="classState(i.type)" style="font-size: 22px">{{i.value}}{{i.unit}}</p>
                            </div>
                        </yd-cell-item>
                        <!-- 数据全部加载完毕显示 -->
                        <span slot="doneTip">没有数据啦~~</span>
                        <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                        <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
                    </yd-infinitescroll>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<script src="./inout.ts" />

