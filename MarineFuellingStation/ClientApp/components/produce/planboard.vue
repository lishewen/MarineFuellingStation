<template>
    <div>
        <yd-cell-group>
            <yd-cell-item arrow v-for="p in salesPlans" :key="p.id" @click.native="godetail(p)">
                <div slot="left">
                    <p style="font-size:18px">{{p.carNo}}</p>
                    <p style="color:lightgray;font-size:12px">{{p.name}}</p>
                </div>
                <div slot="right" style="text-align: left;margin-right: 5px;padding: 0.2rem 0 .2rem">
                    <p style="color:gray"><span style="font-size:20px;color:darkgreen">{{p.count}}</span> {{p.unit}} / 柴油</p>
                    <p style="color:lightsalmon;text-align:right">{{formatDate(p.createdAt)}}</p>
                    <p style="color:forestgreen;text-align:right">{{getState(p.state)}}</p>
                </div>
            </yd-cell-item>
        </yd-cell-group>
    </div>
</template>
<script src="./planboard.ts"></script>