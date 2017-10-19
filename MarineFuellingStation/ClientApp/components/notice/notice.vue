<style>
    .color_blue {
        background-color: lightcyan
    }
</style>
<template>
    <div id="root">
        <div style="text-align: center">
            <yd-button type="primary" @click.native="showAdd = true" style="width: 90%;margin: 10px 0">新增通知</yd-button>
        </div>
        <yd-cell-group>
            <div style="text-align: center;padding: 10px 0 10px">
                <span v-for="(f, index) in filterCType">
                    <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index)">{{f.name}}</yd-button>
                    <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index)">{{f.name}}</yd-button>
                </span>
            </div>
            <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                <yd-cell-item slot="list" v-for="n in notices" :key="n.id" @click.native="" style="padding: 10px 0">
                    <div slot="left" style="padding-left: .2rem">
                        <p>{{n.name}}</p>
                        <p style="margin-top: .2rem;color: gray">{{n.content}}</p>
                        <p>{{n.createdBy}} {{formatDate(n.createAt)}}</p>
                    </div>
                    <div slot="right">
                        <yd-switch v-model="n.isUse" @click.native="changeIsUse(n)"></yd-switch>
                    </div>
                </yd-cell-item>
            </yd-infinitescroll>
        </yd-cell-group>
        <yd-popup position="right" width="70%" v-model="showAdd">
            <yd-cell-group title="标题">
                <yd-cell-item>
                    <span slot="left">
                        <yd-input placeholder="请输入标题" v-model="notice.name" max="20"></yd-input>
                    </span>
                </yd-cell-item>
            </yd-cell-group>
            <yd-cell-group title="通知内容">
                <yd-cell-item>
                    <yd-textarea slot="right" placeholder="请输入通知内容" maxlength="100" v-model="notice.content"></yd-textarea>
                </yd-cell-item>
            </yd-cell-group>
            <yd-cell-group title="通知应用">
                <yd-checkbox-group v-model="toApps">
                    <yd-checkbox val="水上计划" style="padding: 10px 10px"></yd-checkbox>
                    <yd-checkbox val="陆上计划" style="padding: 10px 10px"></yd-checkbox>
                    <yd-checkbox val="销售单" style="padding: 10px 10px"></yd-checkbox>
                    <yd-checkbox val="陆上卸油" style="padding: 10px 10px"></yd-checkbox>
                    <yd-checkbox val="陆上装车" style="padding: 10px 10px"></yd-checkbox>
                    <yd-checkbox val="水上加油" style="padding: 10px 10px"></yd-checkbox>
                </yd-checkbox-group>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button type="primary" @click.native="postNotice()" style="width: 80%;margin: 10px 0" :disabled="notice.name == null || notice.name == ''">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./notice.ts" />