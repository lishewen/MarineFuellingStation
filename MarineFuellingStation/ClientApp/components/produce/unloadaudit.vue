<style>
    .color_blue {
        background-color: lightcyan
    }
</style>
<template>
    <div id="root">
        <yd-cell-group>
            <div style="text-align: center;padding: 10px 0 10px">
                <span v-for="(f, index) in filterCType">
                    <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index)">{{f.name}}</yd-button>
                    <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index)">{{f.name}}</yd-button>
                </span>
            </div>
            <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                <yd-cell-item slot="list" arrow v-for="p in purchases" :key="p.id" @click.native="purchaseclick(p)" style="padding: 10px 0">
                    <div slot="left" style="padding-left: .2rem">
                        <p style="font-weight: bold; color: forestgreen">{{p.carNo}} - {{p.trailerNo}} <span style="color: lightcoral">误差：{{strDiff(p)}}升</span></p>
                        <div style="margin-top: .2rem;">
                            <p style="color: gray" v-for="ts in p.toStoresList">卸入：{{ts.name}} - {{ts.count}}升</p>
                        </div>
                    </div>
                    <div slot="right" style="line-height: .6rem">
                        <p></p>
                        <p>{{p.updateBy}}</p>
                        <p>{{formatDate(p.lastUpdatedAt)}}</p>
                    </div>
                </yd-cell-item>
            </yd-infinitescroll>
        </yd-cell-group>

        <!--actionsheet-->
        <yd-actionsheet :items="actItems" v-model="showAct" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./unloadaudit.ts" />