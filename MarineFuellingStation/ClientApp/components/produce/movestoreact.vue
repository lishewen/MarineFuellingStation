<template>
    <div id="root">
        <yd-tab :change="change">

            <yd-tab-panel label="施工">
                <yd-cell-group>
                    <yd-cell-item arrow @click.native="changeState(m)" v-for="m in movestores" :key="m.id">
                        <div slot="left" style="line-height: 22px;margin: 10px 0 10px 0">
                            <p>
                                <span class="col-green">出：</span><span>【{{m.outStoreTypeName}}】 - {{m.outStoreName}}</span>
                            </p>
                            <p>
                                <span style="color:red">入：</span><span>【{{m.inStoreTypeName}}】 - {{m.inStoreName}}</span>
                            </p>
                            <p class="col-light-gray font12">{{m.name}} - {{formatDate(m.createdAt)}}</p>
                        </div>
                        <div slot="right" style="text-align: left;margin-right: 5px">
                            <p class="col-gray font16">{{m.outPlan}}升</p>
                            <p class="col-red font14">{{m.stateName}}</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-tab-panel label="完工单">
                <yd-cell-group>
                    <yd-cell-item arrow v-for="f in fnmovestores" :key="f.id" @click.native="showMenuclick(f.id)">
                        <div slot="left" class="lineheight24" style="margin: 10px 0 10px 0">
                            <p>
                                <span class="col-green">出：</span><span>【{{f.outStoreTypeName}}】 - {{f.outStoreName}}</span>
                            </p>
                            <p>
                                <span class="col-red">入：</span><span>【{{f.inStoreTypeName}}】 - {{f.inStoreName}}</span>
                            </p>
                            <p class="col-light-gray font12">{{f.name}} - {{formatDate(f.lastUpdateAt)}}</p>
                        </div>
                        <div slot="right" class="align-left" style="margin-right: 5px">
                            <p class="col-gray font16">{{f.outPlan}}升</p>
                        </div>
                    </yd-cell-item>
                </yd-cell-group>
            </yd-tab-panel>
            <yd-popup v-model="show2" position="right" width="70%">
                <yd-cell-group title="施工结束录入">
                    <div style="background-color: yellowgreen;padding: 10px; text-align: center; color: white">本次安排转仓：{{model.outPlan}}升</div>
                    <yd-cell-item>
                        <span slot="left">实际转出：</span>
                        <yd-input slot="right" v-model="model.outFact" required placeholder="" type="number"></yd-input>
                        <span slot="right">升</span>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">实际转入：</span>
                        <yd-input slot="right" v-model="model.inFact" required placeholder="" type="number"></yd-input>
                        <span slot="right">升</span>
                    </yd-cell-item>
                </yd-cell-group>
                <yd-button size="large" type="primary" @click.native="overclick()">施工结束提交</yd-button>
            </yd-popup>
        </yd-tab>
        <!--打印菜单-->
        <yd-actionsheet :items="menus" v-model="showMenus" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./movestoreact.ts" />

