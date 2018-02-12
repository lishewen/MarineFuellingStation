<template>
    <div>
        <yd-tab :callback="change">
            <yd-tab-panel label="计划开单">
                <yd-cell-group :title="'单号：' + model.name">
                    <yd-cell-item arrow @click.native="oilshow = true">
                        <span slot="left">油品：</span>
                        <span slot="right">{{oilName}}</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">单价：</span>
                        <yd-input slot="right" v-model="model.price" type="number" required placeholder="请输入单价"></yd-input>
                        <span slot="right" style="width: 50px">元 / 吨</span>
                    </yd-cell-item>

                    <yd-cell-item>
                        <span slot="left">数量：</span>
                        <yd-input slot="right" v-model="model.count" type="number" required placeholder="请输入数量"></yd-input>
                        <span slot="right">吨</span>
                    </yd-cell-item>

                    <yd-cell-item arrow>
                        <span slot="left">始发地：</span>
                        <input slot="right" type="text" @click.stop="originshow = true" v-model="model.origin" readonly placeholder="请选择始发地" />
                    </yd-cell-item>

                    <yd-cell-item arrow>
                        <span slot="left">出发时间：</span>
                        <yd-datetime v-model="model.startTime" type="datetime" slot="right"></yd-datetime>
                    </yd-cell-item>

                    <yd-cell-item arrow>
                        <span slot="left">预计到达：</span>
                        <yd-datetime v-model="model.arrivalTime" type="datetime" slot="right"></yd-datetime>
                    </yd-cell-item>

                </yd-cell-group>

                <yd-cell-group title="运输">
                    <yd-cell-item>
                        <span slot="left">车牌号一：</span>
                        <yd-input slot="right" v-model="model.carNo" required placeholder="请输入车牌号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">挂车号：</span>
                        <yd-input slot="right" v-model="model.trailerNo" regex="" placeholder="请输入挂车号，选填"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">司机一：</span>
                        <yd-input slot="right" v-model="model.driver1" regex="" placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">身份证一：</span>
                        <yd-input slot="right" v-model="model.idCard1" type="number" placeholder="请输入身份证号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">电话号码一：</span>
                        <yd-input slot="right" v-model="model.phone1" type="number" placeholder="请输入电话号码"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">司机二：</span>
                        <yd-input slot="right" v-model="model.driver2" regex="" placeholder="请输入名称"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">身份证二：</span>
                        <yd-input slot="right" v-model="model.idCard2" type="number" placeholder="请输入身份证号"></yd-input>
                    </yd-cell-item>
                    <yd-cell-item>
                        <span slot="left">电话号码二：</span>
                        <yd-input slot="right" v-model="model.phone2" type="number" placeholder="请输入电话号码"></yd-input>
                    </yd-cell-item>
                </yd-cell-group>

                <div>
                    <yd-button size="large" type="primary" @click.native="buttonclick" :disabled="isPrevent">提交</yd-button>
                </div>
            </yd-tab-panel>
            <yd-tab-panel label="单据记录">

                <yd-cell-group>
                    <yd-search v-model="sv" />
                    <div class="align-center cell-padding">
                        <span v-for="(f, index) in filterCType">
                            <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '筛选')">{{f.name}}</yd-button>
                            <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '筛选')">{{f.name}}</yd-button>
                        </span>
                    </div>
                    <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                        <yd-cell-item slot="list" arrow v-for="s in purchases" :key="s.id" @click.native="showMenuclick(s)">
                            <div slot="left">
                                <p>{{s.name}}</p>
                                <p class="font14">{{s.origin}}</p>
                                <p class="col-light-gray font12">{{s.carNo}} {{s.trailerNo}}</p>
                            </div>
                            <div slot="right" class="align-right" style="margin-right: .2rem;padding: .2rem 0 .2rem">
                                <p>
                                    <span class="col-green">{{s.product == null ? "" : s.product.name}}</span>
                                    <span class="col-gray font16">{{s.count}}吨</span>
                                </p>
                                <p class="col-coral">预计到达：{{getDiffDate(s.arrivalTime, 'hour')}}</p>
                                <p class="col-gray" v-show="s.state != 0">实际到达：{{formatDate(s.updateAt, 'MM-DD hh:mm')}}</p>
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
        <!--popup作废原因输入-->
        <yd-popup v-model="showAddDelReason" position="right">
            <yd-cell-group title="请输入作废单据原因">
                <yd-cell-item>
                    <yd-textarea slot="right" v-model="selectPurchase.delReason" placeholder="请输入本次作废单据原因" maxlength="100"></yd-textarea>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button size="large" type="primary" @click.native="delPurchaseclick" :disabled="selectPurchase.delReason == null || selectPurchase.delReason == ''">提交</yd-button>
        </yd-popup>
        <yd-actionsheet :items="oiloptions" v-model="oilshow" cancel="取消"></yd-actionsheet>
        <yd-actionsheet :items="menus" v-model="showMenus" cancel="取消"></yd-actionsheet>
        <yd-cityselect v-model="originshow" :callback="origincallback" :items="district"></yd-cityselect>
    </div>
</template>

<script src="./purchase.ts" />

