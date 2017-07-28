<template>
    <div id="root">
        <weui-search v-model="sv" />
        <yd-button type="primary" @click.native="handleClick">Click Me!</yd-button>
        <chart :options="polar"></chart>
        <yd-datetime :yearFormat="yearFormat" :monthFormat="monthFormat" :dayFormat="dayFormat" type="date" v-model="datetime4"></yd-datetime>
    </div>
</template>

<script src="./ydui.ts"></script>