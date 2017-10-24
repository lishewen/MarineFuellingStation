<template>
    <div>
        <yd-grids-group :rows="3" title="销售管理">
            <yd-grids-item link="./sales/plan" style="background-color:lightcyan">
                <p slot="text">销售计划</p>
            </yd-grids-item>
            <yd-grids-item link="./sales/order" style="background-color:lightcyan">
                <p slot="text">销售单</p>
            </yd-grids-item>
            <yd-grids-item link="./sales/myorder" style="background-color:lightcyan">
                <p slot="text">我的订单</p>
            </yd-grids-item>
            <yd-grids-item link="./sales/myclient" style="background-color:lightcyan">
                <p slot="text">我的客户</p>
            </yd-grids-item>
            <yd-grids-item link="./sales/boatclean" style="background-color:lightcyan">
                <p slot="text">船舶清污开单</p>
            </yd-grids-item>
            <yd-grids-item link="./sales/auditing" style="background-color:lightcyan">
                <p slot="text">计划审核</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="生产管理">
            <yd-grids-item link="./produce/buyboard" style="background-color:lightcyan">
                <p slot="text">进油计划看板</p>
            </yd-grids-item>
            <yd-grids-item link="./produce/planboard" style="background-color:lightcyan">
                <p slot="text">销售计划看板</p>
            </yd-grids-item>
            <yd-grids-item link="./produce/unload" style="background-color:lightcyan">
                <p slot="text">陆上卸油</p>
            </yd-grids-item>
            <yd-grids-item link="./produce/unloadaudit" style="background-color:lightcyan">
                <p slot="text">卸油审核</p>
            </yd-grids-item>
            <yd-grids-item link="./produce/loadoil" style="background-color:lightcyan">
                <p slot="text">加油</p>
            </yd-grids-item>
            <yd-grids-item link="./produce/movestore" style="background-color:lightcyan">
                <p slot="text">生产(转仓)开单</p>
            </yd-grids-item>
            <yd-grids-item link="./produce/movestoreact" style="background-color:lightcyan">
                <p slot="text">生产施工</p>
            </yd-grids-item>
            <yd-grids-item link="./produce/assay" style="background-color:lightcyan">
                <p slot="text">化验</p>
            </yd-grids-item>
            <yd-grids-item link="./oilstore" style="background-color:lightcyan">
                <p slot="text">油仓情况</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="油仓管理">
            <yd-grids-item link="./oilstore" style="background-color:lightcyan">
                <p slot="text">油仓情况</p>
            </yd-grids-item>
            <yd-grids-item link="./oilstore/inout" style="background-color:lightcyan">
                <p slot="text">出入仓记录</p>
            </yd-grids-item>
            <yd-grids-item link="./oilstore/store" style="background-color:lightcyan">
                <p slot="text">油仓和分类</p>
            </yd-grids-item>
            <yd-grids-item link="./oilstore/product" style="background-color:lightcyan">
                <p slot="text">商品和分类</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="客户管理">
            <yd-grids-item link="./client/client" style="background-color:lightcyan">
                <p slot="text">客户</p>
            </yd-grids-item>
            <yd-grids-item link="./finance/chargelog" style="background-color:lightcyan">
                <p slot="text">账户变动</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="商品采购">
            <yd-grids-item link="./purchase/purchase" style="background-color:lightcyan">
                <p slot="text">进油计划</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="人员管理">
            <yd-grids-item link="./user/user" style="background-color:lightcyan">
                <p slot="text">员工</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="财务">
            <yd-grids-item link="./finance/cashier" style="background-color:lightcyan">
                <p slot="text">收银</p>
            </yd-grids-item>
            <yd-grids-item link="./finance/orderlist" style="background-color:lightcyan">
                <p slot="text">订单查询</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="报表">
            <yd-grids-item link="./report" style="background-color:lightcyan">
                <p slot="text">经营统计</p>
            </yd-grids-item>
            <yd-grids-item link="./wages" style="background-color:lightcyan">
                <p slot="text">工资</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="管理员">
            <yd-grids-item link="./notice" style="background-color:lightcyan">
                <p slot="text">通知</p>
            </yd-grids-item>
            <yd-grids-item link="./wages" style="background-color:lightcyan">
                <p slot="text">工资</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-grids-group :rows="3" title="运输管理">
            <yd-grids-item>
                <p slot="text">客户</p>
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