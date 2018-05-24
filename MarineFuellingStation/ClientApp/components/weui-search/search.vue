<template>
    <div>
        <div class="weui-search-bar">
            <div class="weui-search-bar__form">
                <div class="weui-search-bar__box">
                    <i class="weui-icon-search"></i>
                    <input type="text" class="weui-search-bar__input" :placeholder="placeholder" v-model="currentValue" ref="searchInput">
                    <a href="javascript:" class="weui-icon-clear" @click="searchClear"></a>
                </div>
                <label class="weui-search-bar__label" @click="textClick" v-show="!isActive">
                    <i class="weui-icon-search"></i>
                    <span v-text="placeholder"></span>
                </label>
            </div>
            <a href="javascript:" class="weui-search-bar__cancel-btn" @click="searchCancel" v-show="isActive" v-text="cancelText"></a>
        </div>
    </div>
</template>

<style src="./search.css" />
<script src="./search.ts"/>