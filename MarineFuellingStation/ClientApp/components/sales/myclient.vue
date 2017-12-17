<style>
    .color_blue{
        background-color:lightcyan
    }
</style>
<template>
    <div id="root">
        <yd-cell-group>
            <div class="align-center" style="padding: 10px 0 10px">
                <span v-for="(f, index) in filterCType">
                    <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '客户类型')">{{f.name}}</yd-button>
                    <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '客户类型')">{{f.name}}</yd-button>
                </span>
                <span><yd-button type="hollow" @click.native="show2 = true">筛选</yd-button></span>
            </div>
            <yd-cell-item arrow v-for="c in clients" :key="c.id" @click.native="clientclick(c)" :class="classMark(c.isMark)" style="padding: .1rem 0">
                <div slot="left">
                    <p>{{c.carNo}} - {{c.contact}}</p>
                    <p v-if="c.company != null" class="col-light-gray font12">{{c.company.name}}</p>
                </div>
                <div slot="right" class="align-left" style="margin-right: 5px">
                    <p v-if="c.company != null" class="col-gray">余额：￥{{c.company.balances}}</p>
                    <p class="col-coral lineheight24">最近：{{getDiffDate(c.lastUpdatedAt, 'day')}}</p>
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
                <yd-grids-item v-for="(f, index) in filterCycle" :key="f.id" style="position: static; padding: .2rem">
                    <span slot="text">
                        <yd-button style="box-sizing: inherit" type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '周期')">{{f.name}}</yd-button>
                        <yd-button style="box-sizing: inherit" type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '周期')">{{f.name}}</yd-button>
                    </span>
                </yd-grids-item>
            </yd-grids-group>
            <yd-button size="large" type="primary" @click.native="filterclick()">提交</yd-button>
        </yd-popup>
        <yd-popup v-model="showRemark" position="right" width="75%">
            <yd-cell-group title="备注信息">
                <yd-cell-item>
                    <yd-textarea slot="right" v-model="remark" placeholder="请输入客户备注信息" maxlength="200"></yd-textarea>
                </yd-cell-item>
            </yd-cell-group>
                <yd-button size="large" type="primary" @click.native="putReMark()">保存</yd-button>
        </yd-popup>
        <!--actionsheet-->
        <yd-actionsheet :items="actItems" v-model="showAct" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./myclient.ts" />

