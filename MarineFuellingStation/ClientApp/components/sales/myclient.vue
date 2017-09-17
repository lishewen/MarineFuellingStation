<template>
    <div id="root">
        <yd-cell-group>
            <div style="text-align: center;padding: 10px 0 10px">
                <span v-for="(f, index) in filterCType">
                    <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '客户类型')">{{f.name}}</yd-button>
                    <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '客户类型')">{{f.name}}</yd-button>
                </span>
                <span><yd-button type="hollow" @click.native="show2 = true">筛选</yd-button></span>
            </div>
            <yd-cell-item arrow v-for="c in clients" :key="c.id">
                <div slot="left">
                    <p>{{c.carNo}} - {{c.contact}}</p>
                    <p v-if="c.company != null" style="color:lightgray;font-size:12px">{{c.company.name}}</p>
                </div>
                <div slot="right" style="text-align: left;margin-right: 5px">
                    <p v-if="c.company != null" style="color:gray">余额：￥{{c.company.balances}}</p>
                    <p style="color:lightcoral;line-height: 25px">最近：{{formatDate(c.lastUpdatedAt)}}</p>
                </div>
            </yd-cell-item>
        </yd-cell-group>

        <yd-popup v-model="show2" position="right" width="75%">
            <yd-grids-group :rows="3" title="单选：计划单">
                <yd-grids-item v-for="(f, index) in filterPType" :key="f.id" style="position: static; padding: .2rem">
                    <span slot="text">
                        <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '计划单')">{{f.name}}</yd-button>
                        <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '计划单')">{{f.name}}</yd-button>
                    </span>
                </yd-grids-item>
            </yd-grids-group>
            <yd-grids-group :rows="2" title="单选：账户余额">
                <yd-grids-item v-for="(f, index) in filterBalances" :key="f.id" style="position: static; padding: .2rem">
                    <span slot="text">
                        <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '账户余额')">{{f.name}}</yd-button>
                        <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '账户余额')">{{f.name}}</yd-button>
                    </span>
                </yd-grids-item>
            </yd-grids-group>
            <yd-grids-group :rows="2" title="单选：周期">
                <yd-grids-item  v-for="(f, index) in filterCycle" :key="f.id" style="position: static; padding: .2rem">
                    <span slot="text">
                        <yd-button style="box-sizing: inherit" type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '周期')">{{f.name}}</yd-button>
                        <yd-button style="box-sizing: inherit" type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '周期')">{{f.name}}</yd-button>
                    </span>
                </yd-grids-item>
            </yd-grids-group>
            <div style="text-align: center;margin-top: .2rem">
                <yd-button style="width:90%" type="primary" @click.native="filterclick()">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./myclient.ts" />