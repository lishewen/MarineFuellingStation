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
                <yd-cell-item slot="list" arrow v-for="p in plans" :key="p.id" @click.native="planclick(p)" style="padding: 10px 0">
                    <div slot="left" style="padding-left: .2rem">
                        <p style="font-weight: bold; color: forestgreen">{{p.carNo}} - ￥{{p.price * p.count}}</p>
                        <p style="margin-top: .2rem;color: gray">{{p.oilName}} | ￥{{p.price}} | {{p.count}}{{p.unit}}</p>
                        <p style="color:lightgray">{{p.name}}</p>
                    </div>
                    <div slot="right" style="line-height: .6rem">
                        <p>{{p.createdBy}}</p>
                        <p>{{formatDate(p.oilDate)}}</p>
                    </div>
                </yd-cell-item>
            </yd-infinitescroll>
        </yd-cell-group>

        <!--actionsheet-->
        <yd-actionsheet :items="actItems" v-model="showAct" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./auditing.ts" />