<template>
    <div id="root">
        <weui-search v-model="sv" />
        <yd-button type="primary" @click.native="handleClick">Click Me!</yd-button>
        <chart :options="polar"></chart>
    </div>
</template>

<script src="./ydui.ts"></script>