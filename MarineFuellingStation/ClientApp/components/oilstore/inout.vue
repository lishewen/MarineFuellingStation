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
        <yd-tab :change="change">

            <yd-tab-panel label="所有">
                <yd-cell-group>
                    <yd-cell-item arrow v-for="i in inAndOutLogs" :key="i.id">
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span :class="classState(i.type)">{{getType(i.type)}}</span><span> 【{{getSttName(i.store)}}】 - {{i.store == null? "" : i.store.name}}</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">{{i.operators}}</p>
                            <p style="color:lightgray;font-size:12px">{{formatDate(i.createdAt)}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p :class="classState(i.type)" style="font-size: 22px">{{i.value}}{{i.unit}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="出仓">
                <yd-cell-group>
                    <yd-cell-item arrow v-for="i in inAndOutLogs" :key="i.id">
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span :class="classState(i.type)">{{getType(i.type)}}</span><span> 【{{getSttName(i.store)}}】 - {{i.store == null? "" : i.store.name}}</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">{{i.operators}}</p>
                            <p style="color:lightgray;font-size:12px">{{formatDate(i.createAt)}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p :class="classState(i.type)" style="font-size: 22px">{{i.value}}{{i.unit}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="入仓">
                <yd-cell-group>
                    <yd-cell-item arrow v-for="i in inAndOutLogs" :key="i.id">
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span :class="classState(i.type)">{{getType(i.type)}}</span><span> 【{{getSttName(i.store)}}】 - {{i.store == null? "" : i.store.name}}</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">{{i.operators}}</p>
                            <p style="color:lightgray;font-size:12px">{{formatDate(i.createAt)}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p :class="classState(i.type)" style="font-size: 22px">{{i.value}}{{i.unit}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<script src="./inout.ts" />