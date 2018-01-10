<template>
    <div id="root">
        <template>
            <yd-grids-group :rows="2" title="考勤记录">
                <yd-grids-item v-for="c,idx in checkinData" :key="idx">
                    <div slot="text">
                        <p>{{c.checkin_type}}</p>
                        <p>{{formatDate(new Date(c.checkin_time) * 1000)}}</p>
                    </div>
                </yd-grids-item>
            </yd-grids-group>
        </template>
    </div>
</template>

<script src="./checkin.ts" />

