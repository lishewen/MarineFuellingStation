<template>
    <div id="root">
        <yd-cell-group :title="'单号：' + model.name" style="margin-top:20px">
            <yd-cell-item arrow @click.native="show1 = true">
                <span slot="left">生产员：</span>
                <span slot="right">
                    {{model.manufacturer}}
                </span>
            </yd-cell-item>
        </yd-cell-group>
        <yd-cell-group title="转出仓">

            <yd-cell-item>
                <span slot="left">分类：</span>
                <span slot="left">
                    <yd-button type="warning">地仓</yd-button>
                    <yd-button type="hollow">船759仓</yd-button>
                    <yd-button type="hollow">船228仓</yd-button>
                    <yd-button type="hollow">大船888仓</yd-button>
                </span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油仓：</span>
                <select slot="right" v-model="selectedOutStore">
                    <option value="">请选择油仓</option>
                    <option v-for="s in store" :key="s.id" :value="s.id">{{s.name}}</option>
                </select>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">密度：</span>
                <yd-input slot="right" v-model="model.outDensity" regex="" placeholder="请输入密度"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油温：</span>
                <yd-input slot="right" v-model="model.outTemperature" regex="" placeholder="请输入油温"></yd-input>
                <span slot="right">℃</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">安排转出：</span>
                <yd-input slot="right" v-model="model.outPlan" regex="" placeholder="请输入安排转出升数"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">实际转出：</span>
                <yd-input slot="right" v-model="model.outFact" regex="" placeholder="请输入实际转出升数"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>

        </yd-cell-group>
        <yd-cell-group title="转入仓">

            <yd-cell-item>
                <span slot="left">分类：</span>
                <span slot="left">
                    <yd-button type="hollow">地仓</yd-button>
                    <yd-button type="hollow">船759仓</yd-button>
                    <yd-button type="hollow">船228仓</yd-button>
                    <yd-button type="warning">大船888仓</yd-button>
                </span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油仓：</span>
                <select slot="right" v-model="selectedInStore">
                    <option value="">请选择油仓</option>
                    <option v-for="s in store" :key="s.id" :value="s.id">{{s.name}}</option>
                </select>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">密度：</span>
                <yd-input slot="right" v-model="model.inDensity" regex="" placeholder="请输入密度"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油温：</span>
                <yd-input slot="right" v-model="model.inTemperature" regex="" placeholder="请输入油温"></yd-input>
                <span slot="right">℃</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">实际转入：</span>
                <yd-input slot="right" v-model="model.inFact" regex="" placeholder="请输入实际转入升数"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>
        </yd-cell-group>

        <div>
            <yd-button size="large" type="primary" @click.native="buttonclick">提交</yd-button>
        </div>

        <yd-popup v-model="show1" position="right">
            <yd-cell-group title="请选择生产员">
                <yd-cell-item type="checkbox" v-for="m in manufacturer" :key="m.userid">
                    <span slot="left">{{m.name}}</span>
                    <input slot="right" type="checkbox" :value="m.name" v-model="picked" />
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="selectproducerclick()">保存</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./movestore.ts" />