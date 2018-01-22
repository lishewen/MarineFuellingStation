<template>
    <div id="root">
        <yd-tab :callback="change">
            <yd-tab-panel label="开单">
                <yd-cell-group :title="'单号：' + model.name">
                    <!--应客户要求暂时取消生产员的选择
                    <yd-cell-item arrow @click.native="showManuUsers = true">
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
                </yd-cell-group>

                <div>
                    <yd-button size="large" type="primary" @click.native="buttonclick" :disabled="isPrevent || isPrevent1">提交</yd-button>
                </div>
            </yd-tab-panel>
            <yd-tab-panel label="记录">
                <yd-search v-model="sv" />
                <yd-cell-group>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                        <yd-cell-item slot="list" arrow v-for="m in movestores" :key="m.id">
                            <div slot="left" class="lineheight24" style="margin: 10px 0 10px 0">
                                <p class="">{{m.name}}</p>
                                <p class="col-green mtop10"> 出：{{m.outStoreName}} | {{m.outTemperature}}℃ | 密{{m.outDensity}}</p>
                                <p class="col-red">入：{{m.inStoreName}} | {{m.inTemperature}}℃ | 密{{m.inDensity}}</p>
                            </div>
                            <div slot="right" style="margin-right: 5px">
                                <p class="col-gray font16">{{m.outPlan}}升</p>
                                <p class="col-light-gray">{{formatDate(m.lastUpdatedAt, 'MM-DD HH:mm')}}</p>
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

        <yd-popup v-model="showManuUsers" position="right">
            <yd-cell-group title="请选择生产员">
                <yd-cell-item type="checkbox" v-for="m in manufacturer" :key="m.userid">
                    <span slot="left">{{m.name}}</span>
                    <input slot="right" type="checkbox" :value="m.name" v-model="picked" />
                </yd-cell-item>
            </yd-cell-group>
            <div class="align-center">
                <yd-button style="width:100px" type="primary" @click.native="selectproducerclick()">确定</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./movestore.ts" />

