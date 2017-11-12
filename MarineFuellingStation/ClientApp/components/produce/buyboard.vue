<template>
    <div>
        <yd-grids-group :rows="2" title="已计划">
            <yd-grids-item v-for="p in list" :key="p.id" @click.native="godetail(p.id)">
                <div slot="text" style="line-height:25px">
                    <p class="col-coral font14">【{{p.name}}】</p>
                    <p class="col-green">{{formatDate(p.arrivalTime)}} 到</p>
                    <p class="col-gray font16" style="font-weight:bold;">{{p.product.name}} {{p.count}} 吨</p>
                    <p class="col-light-gray">{{p.carNo}} {{p.driver1}}</p>
                </div>
            </yd-grids-item>
        </yd-grids-group>
    </div>
</template>
<script src="./buyboard.ts"></script>

