<template>
    <div id="root">
        <yd-tab :change="change">

            <yd-tab-panel label="所有">
                <yd-cell-group>
                    <yd-cell-item arrow>
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span style="color:forestgreen">出仓</span><span>【地仓】 - 1#仓</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">张三 李四 王五</p>
                            <p style="color:lightgray;font-size:12px">2017-07-07 10:00</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray; font-size: 22px">1000升</p>
                        </div>
                    </yd-cell-item>
                    <yd-cell-item arrow>
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span style="color:red">入仓</span><span>【船759仓】 - 2#仓</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">张三 李四 王五</p>
                            <p style="color:lightgray;font-size:12px">2017-07-07 10:00</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray; font-size: 22px">1000升</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="出仓">
                <yd-cell-group>
                    <yd-cell-item arrow>
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span style="color:forestgreen">出仓</span><span>【地仓】 - 1#仓</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">张三 李四 王五</p>
                            <p style="color:lightgray;font-size:12px">2017-07-07 10:00</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray; font-size: 22px">1000升</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="入仓">
                <yd-cell-group>
                    <yd-cell-item arrow>
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span style="color:red">入仓</span><span>【船759仓】 - 2#仓</span>
                            </p>
                            <p style="color:lightgray;font-size:12px">张三 李四 王五</p>
                            <p style="color:lightgray;font-size:12px">2017-07-07 10:00</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p style="color:gray; font-size: 22px">1000升</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
        </yd-tab>
    </div>
</template>

<script src="./inout.ts" />