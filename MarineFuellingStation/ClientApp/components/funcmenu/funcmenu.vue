<template>
    <div>
        <yd-grids-group :rows="3" title="销售管理">
            <yd-grids-item link="./sales/plan">
                <p slot="text">销售计划</p>
            </yd-grids-item>
            <yd-grids-item link="./sales/order">
                <p slot="text">销售单</p>
            </yd-grids-item>
            <yd-grids-item link="./sales/myorder">
                <p slot="text">我的订单</p>
            </yd-grids-item>
            <yd-grids-item link="./sales/myclient">
                <p slot="text">我的客户</p>
            </yd-grids-item>
            <yd-grids-item link="./sales/boatclean">
                <p slot="text">船舶清污开单</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="生产管理">
            <yd-grids-item link="./produce/buyboard">
                <p slot="text">采购计划看板</p>
            </yd-grids-item>
            <yd-grids-item link="./produce/planboard">
                <p slot="text">销售计划看板</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">陆上卸油</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">水上装油</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">生产（转仓）</p>
            </yd-grids-item>
            <yd-grids-item link="./produce/assay">
                <p slot="text">化验</p>
            </yd-grids-item>
            <yd-grids-item link="./oilstore">
                <p slot="text">油仓情况</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="油仓管理">
            <yd-grids-item>
                <p slot="text">油仓情况</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">出入仓记录</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">油仓和分类</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">商品和分类</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="客户管理">
            <yd-grids-item>
                <p slot="text">客户</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">公司</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="商品采购">
            <yd-grids-item>
                <p slot="text">采购计划</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="单据打印">
            <yd-grids-item>
                <p slot="text">卸油单</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">卸油过磅单</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">装车过磅单</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">船舶清污单</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">调拨单</p>
                <p slot="text">（销售单）</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">预收款确认单</p>
                <p slot="text">（充值）</p>
            </yd-grids-item>
        </yd-grids-group>
    </div>
</template>