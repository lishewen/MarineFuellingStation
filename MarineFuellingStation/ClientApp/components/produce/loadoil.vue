<template>
    <div>
        <yd-grids-group :rows="2" title="加油">
            <yd-grids-item @click.native="go('load')">
                <img slot="icon" src="/images/水上加油.png" style="width:36px; height: 36px">
                <div slot="text">
                    水上加油
                </div>
            </yd-grids-item>
            <yd-grids-item @click.native="go('landload')">
                <img slot="icon" src="/images/陆上装车.png" style="width:36px; height: 36px">
                <div slot="text">
                    陆上装车
                </div>
            </yd-grids-item>
            <yd-grids-item @click.native="go('landload')">
                <img slot="icon" src="/images/运输车加油.png" style="width:36px; height: 36px">
                <div slot="text">
                    运输车加油
                </div>
            </yd-grids-item>
        </yd-grids-group>
    </div>
</template>
<script src="./loadoil.ts"></script>

