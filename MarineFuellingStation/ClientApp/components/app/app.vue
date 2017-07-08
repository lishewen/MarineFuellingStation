<template>
    <div id='app-root' style="height:100%">
        <yd-layout>
            <yd-navbar slot="navbar" :title="title">
                <router-link to="#" slot="left">
                    <yd-navbar-back-icon></yd-navbar-back-icon>
                </router-link>
            </yd-navbar>
            <router-view @setTitle="setTitle" />
        </yd-layout>
    </div>
</template>

<script src="./app.ts"></script>
