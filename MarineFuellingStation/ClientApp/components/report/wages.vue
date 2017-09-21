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
        <weui-search v-model="sv" />
        <div style="text-align:center;margin-top:10px;font-size:18px">
            <yd-datetime type="date" v-model="selecteddate"></yd-datetime>
        </div>
        
        <yd-cell-group :title="'总计：￥' + sumwage" style="margin-top: 20px">
            <yd-cell-item v-for="w in list" :key="w.id" arrow @click.native="clickWage(w)">
                <div slot="left">
                    <p>{{w.name}}</p>
                    <p style="color:lightgray;font-size:14px">{{w.职务}} - {{departmentdict[w.departmentId]}}</p>
                </div>
                <div slot="right" style="text-align: right;margin-right: 5px">
                    <p>
                        <span style="color:gray; font-size: 18px">￥{{w.实发}}</span>
                    </p>
                </div>
            </yd-cell-item>
        </yd-cell-group>
        <yd-popup v-model="showwage" position="right">
            <div style="font-size: 16px;line-height:30px;margin:0px 12px 0px 12px">
                <yd-flexbox>
                    <yd-flexbox-item>姓名：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">{{model.name}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>职务：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">{{model.职务}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>基本：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">{{model.基本}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>出勤天数：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">{{model.出勤天数}}</yd-flexbox-item>
                </yd-flexbox>
                <div style="text-align:right;margin-top:20px;color:lightgray">津贴</div>
                <hr />
                <yd-flexbox>
                    <yd-flexbox-item>提成：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">{{model.提成}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>超额：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">{{model.超额}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>交通：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input type="number" v-model="traffic" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <div style="text-align:right;margin-top:20px;color:lightgray">扣款</div>
                <hr />
                <yd-flexbox>
                    <yd-flexbox-item>社保：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">{{model.社保}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>请假：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input type="number" v-model="qingjia" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>餐费：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">{{model.餐费}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>借支：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input type="number" v-model="lend" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item><span style="width:200px">安全保障金：</span></yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input type="number" v-model="security" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <div style="text-align:right;margin-top:20px;color:lightgray">汇总</div>
                <hr />
                <yd-flexbox>
                    <yd-flexbox-item>实发：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">{{model.实发}}</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>转卡金额：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input type="number" v-model="model.转卡金额" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>现金：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input type="number" v-model="model.现金" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
            </div>
            <div style="text-align: center;margin-top:20px">
                <yd-button style="width:100px" type="primary" @click.native="saveWage">保存</yd-button>
            </div>
        </yd-popup>
        <yd-popup v-model="departshow" position="right">
            <yd-cell-group title="请选择部门">
                <yd-cell-item type="checkbox" v-for="d in departments" :key="d.id">
                    <span slot="left">{{d.name}}</span>
                    <input slot="right" type="checkbox" :value="d.id" v-model="picked" />
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="clickDepart">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./wages.ts" />