<template>
    <div id="root">
        <yd-grids-group :rows="3">
            <yd-grids-item style="padding:10px">
                <div slot="text">
                    当月
                </div>
            </yd-grids-item>
            <yd-grids-item style="padding:10px">
                <div slot="text">
                    上月
                </div>
            </yd-grids-item>
            <yd-grids-item style="padding:10px" @click.native="departshow = true">
                <p slot="text">筛选</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-search v-model="sv" />
        <div class="align-center first-group font16">
            <yd-datetime type="date" v-model="selecteddate"></yd-datetime>
        </div>
        
        <yd-cell-group :title="'总计：￥' + sumwage">
            <yd-cell-item v-for="w in list" :key="w.id" arrow @click.native="clickWage(w)">
                <div slot="left">
                    <p>{{w.name}}</p>
                    <p class="col-light-gray font14">{{w.职务}} - {{departmentdict[w.departmentId]}}</p>
                </div>
                <div slot="right" class="align-right" style="margin-right: 5px">
                    <p>
                        <span class="col-gray font16">￥{{w.实发}}</span>
                    </p>
                </div>
            </yd-cell-item>
        </yd-cell-group>
        <yd-popup v-model="showwage" position="right" width="70%">
            <div class="font14 lineheight28" style="margin:0px 12px 0px 12px">
                <yd-flexbox>
                    <yd-flexbox-item>姓名：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">{{model.name}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>职务：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">{{model.职务}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>基本：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">{{model.基本}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>出勤天数：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">{{model.出勤天数}}</yd-flexbox-item>
                </yd-flexbox>
                <div class="align-right first-group col-light-gray">津贴</div>
                <hr />
                <yd-flexbox>
                    <yd-flexbox-item>绩效工资：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">
                        <yd-input type="number" v-model="jixiao" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>提成：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">{{model.提成}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>超额：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">{{model.超额}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>交通：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">
                        <yd-input type="number" v-model="traffic" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <div class="align-right first-group col-light-gray">扣款</div>
                <hr />
                <yd-flexbox>
                    <yd-flexbox-item>社保：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">{{model.社保}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>请假：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">
                        <yd-input type="number" v-model="qingjia" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>餐费：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">{{model.餐费}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>借支：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">
                        <yd-input type="number" v-model="lend" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item><span style="width:200px">安全保障金：</span></yd-flexbox-item>
                    <yd-flexbox-item class="align-right">
                        <yd-input type="number" v-model="security" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <div class="align-right first-group col-light-gray">汇总</div>
                <hr />
                <yd-flexbox>
                    <yd-flexbox-item>实发：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">{{model.实发}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>转卡金额：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">
                        <yd-input type="number" v-model="model.转卡金额" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>现金：</yd-flexbox-item>
                    <yd-flexbox-item class="align-right">
                        <yd-input type="number" v-model="model.现金" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
            </div>
            <div class="align-center first-group">
                <yd-button style="width:80%" type="primary" @click.native="saveWage">保存</yd-button>
            </div>
        </yd-popup>
        <yd-popup v-model="departshow" position="right" width="70%">
            <yd-cell-group title="请选择部门">
                <yd-cell-item type="checkbox" v-for="d in departments" :key="d.id">
                    <span slot="left">{{d.name}}</span>
                    <input slot="right" type="checkbox" :value="d.id" v-model="picked" />
                </yd-cell-item>
            </yd-cell-group>
            <div class="align-center">
                <yd-button style="width:80%" type="primary" @click.native="clickDepart">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./wages.ts" />

