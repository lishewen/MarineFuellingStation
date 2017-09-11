<template>
    <div>
        <yd-grids-group :rows="2" title="已计划">
            <yd-grids-item v-for="p in list" :key="p.id">
                <div slot="text" style="line-height:25px">
                    <p style="color:lightcoral;font-size: 14px">【{{p.name}}】</p>
                    <p style="color: forestgreen">{{formatDate(p.arrivalTime)}} 到</p>
                    <p style="color: gray;font-weight:bold;font-size:18px">{{p.product.name}} {{p.count}} 吨</p>
                    <p style="color: lightgray">{{p.carNo}} {{p.driver1}}</p>
                </div>
            </yd-grids-item>
        </yd-grids-group>
    </div>
</template>
<script src="./buyboard.ts"></script>