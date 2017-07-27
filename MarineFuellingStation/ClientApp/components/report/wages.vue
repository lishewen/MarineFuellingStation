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
            <yd-grids-item style="padding:10px" @click.native="show2 = true">
                <p slot="text">筛选</p>
            </yd-grids-item>
        </yd-grids-group>
        <weui-search v-model="sv" />
        <div style="text-align:center;margin-top:10px;font-size:18px">
            <yd-datetime type="date" v-model="selecteddate"></yd-datetime>
        </div>
        
        <yd-cell-group title="总计：￥20000.6" style="margin-top: 20px">
            <yd-cell-item arrow @click.native="show1 = true">
                <div slot="left">
                    <p>张三</p>
                    <p style="color:lightgray;font-size:14px">普通职员 - 销售部</p>
                </div>
                <div slot="right" style="text-align: right;margin-right: 5px">
                    <p>
                        <span style="color:gray; font-size: 18px">￥1000.6</span>
                    </p>
                </div>
            </yd-cell-item>
            <yd-cell-item arrow @click.native="">
                <div slot="left">
                    <p>李四</p>
                    <p style="color:lightgray;font-size:14px">上级 - 生产部</p>
                </div>
                <div slot="right" style="text-align: right;margin-right: 5px">
                    <p>
                        <span style="color:gray; font-size: 18px">￥1000.6</span>
                    </p>
                </div>
            </yd-cell-item>
        </yd-cell-group>
        <yd-popup v-model="show1" position="right">
            <div style="font-size: 16px;line-height:30px;margin:0px 12px 0px 12px">
                <yd-flexbox>
                    <yd-flexbox-item>姓名：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">张三</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>职务：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">￥1000</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>基本：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">￥1000</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>出勤天数：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">￥1000</yd-flexbox-item>
                </yd-flexbox>
                <div style="text-align:right;margin-top:20px;color:lightgray">津贴</div>
                <hr />
                <yd-flexbox>
                    <yd-flexbox-item>提成：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">￥1000</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>超额：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">￥1000</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>交通：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input v-model="carNo" regex="" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>应付：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">￥1000</yd-flexbox-item>
                </yd-flexbox>
                <div style="text-align:right;margin-top:20px;color:lightgray">扣款</div>
                <hr />
                <yd-flexbox>
                    <yd-flexbox-item>社保：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">￥1000</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>请假：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input v-model="carNo" regex="" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>餐费：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">￥1000</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>借支：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input v-model="carNo" regex="" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item><span style="width:200px">安全保障金：</span></yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input v-model="carNo" regex="" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <div style="text-align:right;margin-top:20px;color:lightgray">汇总</div>
                <hr />
                <yd-flexbox>
                    <yd-flexbox-item>实发：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">￥1000</yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>转卡金额：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input v-model="carNo" regex="" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
                <yd-flexbox>
                    <yd-flexbox-item>现金：</yd-flexbox-item>
                    <yd-flexbox-item style="text-align:right">
                        <yd-input v-model="carNo" regex="" placeholder="请输入" style="background-color:lightcyan"></yd-input>
                    </yd-flexbox-item>
                </yd-flexbox>
            </div>
            <div style="text-align: center;margin-top:20px">
                <yd-button style="width:100px" type="primary" @click.native="">保存</yd-button>
            </div>
        </yd-popup>
        <yd-popup v-model="show2" position="right">
            <yd-cell-group title="请选择生产员">
                <yd-cell-item type="checkbox">
                    <span slot="left">生产部</span>
                    <input slot="right" type="checkbox" value="生产部" v-model="picked" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">销售部</span>
                    <input slot="right" type="checkbox" value="销售部" v-model="picked" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">后勤部</span>
                    <input slot="right" type="checkbox" value="后勤部" v-model="picked" />
                </yd-cell-item>
                <yd-cell-item type="checkbox">
                    <span slot="left">财务部</span>
                    <input slot="right" type="checkbox" value="财务部" v-model="picked" />
                </yd-cell-item>
                <yd-cell-item type="checkbox">
                    <span slot="left">总经办</span>
                    <input slot="right" type="checkbox" value="总经办" v-model="picked" />
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./wages.ts" />