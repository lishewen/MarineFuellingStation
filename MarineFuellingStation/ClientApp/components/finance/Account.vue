<template>
    <div>
        <yd-grids-group :rows="2" title="账户">
            <yd-grids-item @click.native="go('/finance/charge')">
                <img slot="icon" src="/images/水上加油.png" style="width:36px; height: 36px">
                <div slot="text">
                    充值
                </div>
            </yd-grids-item>
            <yd-grids-item @click.native="go('/finance/chargelog')">
                <img slot="icon" src="/images/陆上装车.png" style="width:36px; height: 36px">
                <div slot="text">
                    变动记录
                </div>
            </yd-grids-item>
        </yd-grids-group>
    </div>
</template>
<script src="./Account.ts"></script>