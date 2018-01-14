<style>
    .color_blue{
        background-color:lightcyan
    }
</style>
<template>
    <div id="root">
        <yd-cell-group>
            <yd-search v-model="sv" :on-submit="searchSubmit"></yd-search>
            <div class="align-center">
                <yd-button type="warning" @click.native="showAddCompanyclick" style="width: 90%; height: 38px; margin: .2rem 0">添加新公司</yd-button>
            </div>
            <div class="align-center" style="padding: 10px 0 10px">
                <span v-for="(f, index) in filterCType">
                    <yd-button type="warning" v-if="f.actived" @click.native="switchBtn(f, index, '客户类型')">{{f.name}}</yd-button>
                    <yd-button type="hollow" v-if="!f.actived" @click.native="switchBtn(f, index, '客户类型')">{{f.name}}</yd-button>
                </span>
                <span><yd-button type="hollow" @click.native="show2 = true">筛选</yd-button></span>
            </div>
            <yd-infinitescroll :callback="loadList" ref="infinitescroll">
                <yd-cell-item slot="list" arrow v-for="c in clients" :key="c.id" @click.native="clientclick(c)" :class="classMark(c.isMark)">
                    <div slot="left">
                        <p>{{c.carNo}} - {{c.contact}}</p>
                        <p v-if="c.company != null" class="col-light-gray font12">{{c.company.name}}</p>
                    </div>
                    <div slot="right" class="align-left" style="margin-right: 5px">
                        <p v-if="c.company != null" class="col-gray">余额：￥{{c.company.balances}}</p>
                        <p class="col-coral lineheight24">最近：{{getDiffDate(c.lastUpdatedAt, 'hour')}}</p>
                    </div>
                </yd-cell-item>
                <!-- 数据全部加载完毕显示 -->
                <span slot="doneTip">没有数据啦~~</span>
                <!-- 加载中提示，不指定，将显示默认加载中图标 -->
                <img slot="loadingTip" src="http://static.ydcss.com/uploads/ydui/loading/loading10.svg" />
            </yd-infinitescroll>
        </yd-cell-group>
        <!--筛选条件popup-->
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
        <!--popup新增公司-->
        <yd-popup v-model="showCompanyInput" position="right" width="70%">
            <yd-cell-group title="必填">
                <yd-cell-item>
                    <span slot="left">名称：</span>
                    <yd-input slot="right" v-model="modelCompany.name" required placeholder="请输入"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <yd-cell-group title="选填">
                <yd-cell-item>
                    <span slot="left">电话：</span>
                    <yd-input slot="right" v-model="modelCompany.phone" regex="mobile" placeholder="请输入"></yd-input>
                </yd-cell-item>
                <yd-cell-item arrow>
                    <span slot="left">票类：</span>
                    <select slot="right">
                        <option value="">请选择票类</option>
                        <option value="0">循票</option>
                        <option value="1">柴票</option>
                    </select>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">抬头：</span>
                    <yd-input slot="right" v-model="modelCompany.invoiceTitle" regex="" placeholder="开票抬头"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">税号：</span>
                    <yd-input slot="right" v-model="modelCompany.taxFileNumber" regex="" type="number" placeholder="请输入"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">账户：</span>
                    <yd-input slot="right" v-model="modelCompany.businessAccount" type="number" placeholder="对公账户"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">银行：</span>
                    <yd-input slot="right" v-model="modelCompany.bank" regex="" placeholder="开户银行"></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">地址：</span>
                    <yd-input slot="right" v-model="modelCompany.address" regex="" placeholder="请输入"></yd-input>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button size="large" type="primary" @click.native="addcompanyclick">提交</yd-button>
        </yd-popup>
        <!--备注信息popup-->
        <yd-popup v-model="showRemark" position="right" width="75%">
            <yd-cell-group title="备注信息">
                <yd-cell-item>
                    <yd-textarea slot="right" v-model="remark" placeholder="请输入客户备注信息" maxlength="200"></yd-textarea>
                </yd-cell-item>
            </yd-cell-group>
            <yd-button size="large" type="primary" @click.native="putReMark()">保存</yd-button>
        </yd-popup>
        <yd-popup v-model="showCompanys" position="right" width="75%">
            <yd-search v-model="svCompany" :on-submit="searchCompanySubmit"></yd-search>
            <yd-cell-group title="选择要编入的公司">
                <yd-cell-item arrow v-for="co in companys" :key="co.id" @click.native="companyclick(co.id, co.name)">
                    <span slot="left">{{co.name}}</span>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--actionsheet-->
        <yd-actionsheet :items="actItems" v-model="showAct" cancel="取消"></yd-actionsheet>
    </div>
</template>

<script src="./myclient.ts" />

