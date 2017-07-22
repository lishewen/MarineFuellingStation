<template>
    <div id="root">
        <yd-cell-group title="单号：ZC07070001" style="margin-top:20px">
            <yd-cell-item arrow @click.native="show1 = true">
                <span slot="left">生产员：</span>
                <span slot="right">
                    {{picked}}
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
                <select slot="right">
                    <option value="">请选择油仓</option>
                    <option value="1">1#仓</option>
                    <option value="2">2#仓</option>
                    <option value="3">3#仓</option>
                </select>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">密度：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入密度"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油温：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入油温"></yd-input>
                <span slot="right">℃</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">安排转出：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入安排转出升数"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">实际转出：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入实际转出升数"></yd-input>
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
                <select slot="right">
                    <option value="">请选择油仓</option>
                    <option value="1">1#仓</option>
                    <option value="2">2#仓</option>
                    <option value="3">3#仓</option>
                </select>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">密度：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入密度"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">油温：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入油温"></yd-input>
                <span slot="right">℃</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">实际转入：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="请输入实际转入升数"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>

        </yd-cell-group>

        <yd-cell-group title="不显示" style="margin-top:10px">
            <yd-cell-item>
                <span slot="left">●开单员：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">●开单时间：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">●开始转仓时间：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">●结束转仓时间：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="自动录入"></yd-input>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">●耗时：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="结束转仓时间-开始转仓时间"></yd-input>
                <span slot="right">分钟</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">●安排转入：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="默认安排转出升数"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>

            <yd-cell-item>
                <span slot="left">●误差：</span>
                <yd-input slot="right" v-model="carNo" regex="" placeholder="安排转出-实际转入"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>

        </yd-cell-group>
        <div>
            <yd-button size="large" type="primary">提交</yd-button>
        </div>

        <yd-popup v-model="show1" position="right">
            <yd-cell-group title="请选择生产员">
                <yd-cell-item type="checkbox">
                    <span slot="left">生产员1</span>
                    <input slot="right" type="checkbox" value="生产员1" v-model="picked" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">生产员2</span>
                    <input slot="right" type="checkbox" value="生产员2" v-model="picked" />
                </yd-cell-item>

                <yd-cell-item type="checkbox">
                    <span slot="left">生产员3</span>
                    <input slot="right" type="checkbox" value="生产员3" v-model="picked" />
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:100px" type="primary" @click.native="selectproducerclick()">保存</yd-button>
            </div>
        </yd-popup>
    </div>
</template>

<script src="./movestore.ts" />