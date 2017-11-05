<template>
    <div id="root">
        <yd-cell-group :title="s.name + ' - ' + s.volume + '升'" v-for="s in stores" :key="s.id">
            <yd-cell-item>
                <span slot="left">当前油量：</span>
                <yd-input slot="right" required v-model="s.value"></yd-input>
                <span slot="right">升</span>
            </yd-cell-item>
            <yd-cell-item>
                <span slot="left">密度：</span>
                <yd-input slot="right" required v-model="s.density"></yd-input>
            </yd-cell-item>
        </yd-cell-group>
        <yd-button type="primary" size="large" @click.native="saveclick" :disabled="isPrevent">保存</yd-button>
    </div>
</template>

<script src="./setting.ts" />
<style src="./../website.css" />