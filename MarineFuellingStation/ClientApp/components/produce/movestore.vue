<template>
    <div id="root">
        <yd-cell-group :title="'单号：' + model.name" style="margin-top:20px">
            <!--应客户要求暂时取消生产员的选择-->
            <!--<yd-cell-item arrow @click.native="showManuUsers = true">
                <span slot="left">生产员：</span>
                <span slot="right">
                    {{model.manufacturer}}
                </span>
            </yd-cell-item>-->
        </yd-cell-group>
        <yd-cell-group title="转出仓">

            <yd-cell-item>
                <span slot="left">分类：</span>
                <span slot="right" style="padding: .2rem 0">
                    <span v-for="(f, index) in stypeFrom">
                        <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '转出仓')">{{f.name}}</yd-button>
                        <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '转出仓')">{{f.name}}</yd-button>
                    </span>
                </span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油仓：</span>
                <select slot="right" v-model="selectedOutStore">
                    <option value="">请选择油仓</option>
                    <option v-for="s in outStores" :key="s.id" :value="s.id">{{s.name}}</option>
                </select>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">密度：</span>
                <yd-input slot="right" v-model="model.outDensity" type="number" placeholder="请输入密度" required></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油温：</span>
                <yd-input slot="right" v-model="model.outTemperature" type="number" placeholder="请输入油温" required></yd-input>
                <span slot="right">℃</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">安排转出：</span>
                <yd-input slot="right" v-model="model.outPlan" type="number" placeholder="请输入安排转出升数" required></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>

            <!--<yd-cell-item>
                <span slot="left">实际转出：</span>
                <yd-input slot="right" v-model="model.outFact" type="number" placeholder="请输入实际转出升数"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>-->

        </yd-cell-group>
        <yd-cell-group title="转入仓">

            <yd-cell-item>
                <span slot="left">分类：</span>
                <span slot="right" style="padding: .2rem 0">
                    <span v-for="(ff, index) in stypeTo">
                        <yd-button type="warning" v-if="ff.actived" @click.native="switchBtn(ff, index, '转入仓')">{{ff.name}}</yd-button>
                        <yd-button type="hollow" v-if="!ff.actived" @click.native="switchBtn(ff, index, '转入仓')">{{ff.name}}</yd-button>
                    </span>
                </span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油仓：</span>
                <select slot="right" v-model="selectedInStore">
                    <option value="">请选择油仓</option>
                    <option v-for="s in inStores" :key="s.id" :value="s.id">{{s.name}}</option>
                </select>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">密度：</span>
                <yd-input slot="right" v-model="model.inDensity" type="number" placeholder="请输入密度" required></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油温：</span>
                <yd-input slot="right" v-model="model.inTemperature" type="number" placeholder="请输入油温" required></yd-input>
                <span slot="right">℃</span>
            </yd-cell-item>

            <!--<yd-cell-item>
                <span slot="left">实际转入：</span>
                <yd-input slot="right" v-model="model.inFact" type="number" placeholder="请输入实际转入升数"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>-->
        </yd-cell-group>

        <div>
            <yd-button size="large" type="primary" @click.native="buttonclick" :disabled="isPrevent">提交</yd-button>
        </div>

        <yd-popup v-model="showManuUsers" position="right">
            <yd-cell-group title="请选择生产员">
                <yd-cell-item type="checkbox" v-for="m in manufacturer" :key="m.userid">
                    <span slot="left">{{m.name}}</span>
                    <input slot="right" type="checkbox" :value="m.name" v-model="picked" />
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="selectproducerclick()">确定</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./movestore.ts" />