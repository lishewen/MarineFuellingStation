<template>
    <div id="root">
        <yd-grids-group :rows="2" title="汇总" class="font14">
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">未完成</p>
                <p slot="text" class="mtop10 font24 col-green">11111 L</p>
                <p slot="text" class="font14 col-green">111吨</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">已完成</p>
                <p slot="text" class="mtop10">今日：222 L / 22吨</p>
                <p slot="text">昨日：333 L / 33吨</p>
                <p slot="text">本月：444 L / 44吨</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="2" title="当月完成比对（差额：￥99999）" class="font14">
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">计划</p>
                <p slot="text" class="mtop10">222 L / 22吨 / ￥9999</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">实际</p>
                <p slot="text" class="mtop10">222 L / 22吨 / ￥9999</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="2" title="当月任务完成率" class="font14">
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">完成</p>
                <p slot="text" class="mtop10 font24 col-red">99.99%</p>
                <p slot="text" class="mtop10 col-green">当前：￥9999</p>
                <p slot="text" class="col-green">剩余：￥9999</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">当月任务</p>
                <p slot="text" class="mtop10">设置</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="2" title="按商品" class="font14">
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">【国四】柴油</p>
                <p slot="text" class="mtop10">222 L / 22吨 / ￥9999</p>
                <p slot="text" class="mtop10 col-gray1">最高单价：3.33 / 升</p>
                <p slot="text" class="col-gray2">最低单价：3.33 / 升</p>
                <p slot="text" class="col-gray3">平均单价：3.33 / 升</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">【国五】柴油</p>
                <p slot="text" class="mtop10">222 L / 22吨 / ￥9999</p>
                <p slot="text" class="mtop10 col-gray1">最高单价：3.33 / 升</p>
                <p slot="text" class="col-gray2">最低单价：3.33 / 升</p>
                <p slot="text" class="col-gray3">平均单价：3.33 / 升</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="2" title="按水陆" class="font14">
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">水上</p>
                <p slot="text" class="mtop10">222 L / 22吨 / ￥9999</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text" class="font16 col-light-gray">陆上</p>
                <p slot="text" class="mtop10">222 L / 22吨 / ￥9999</p>
            </yd-grids-item>
        </yd-grids-group>
    </div>
</template>
<script src="./report.ts"></script>
<style src="./../website.css" />