<template>
    <div>
        <yd-cell-group>
            <yd-cell-item arrow @click.native="show2 = true">
                <div slot="left">
                    <p style="font-size:18px">桂平货00001</p>
                    <p style="color:lightgray;font-size:12px">JH107070001</p>
                </div>
                <div slot="right" style="text-align: left;margin-right: 5px">
                    <p style="color:gray"><span style="font-size:20px;color:darkgreen">300</span> 升 / 柴油</p>
                    <p style="color:lightsalmon;text-align:right">今天</p>
                </div>
            </yd-cell-item>
            <yd-cell-item arrow @click.native="show2 = true">
                <div slot="left">
                    <p style="font-size:18px">桂平货00001</p>
                    <p style="color:lightgray;font-size:12px">JH107070001</p>
                </div>
                <div slot="right" style="text-align: left;margin-right: 5px">
                    <p style="color:gray"><span style="font-size:20px;color:darkgreen">50</span> 升 / 柴油</p>
                    <p style="color:lightsalmon;text-align:right">明天</p>
                </div>
            </yd-cell-item>
            <yd-cell-item arrow @click.native="show2 = true">
                <div slot="left">
                    <p style="font-size:18px">桂平货00001</p>
                    <p style="color:lightgray;font-size:12px">JH107070001</p>
                </div>
                <div slot="right" style="text-align: left;margin-right: 5px">
                    <p style="color:gray"><span style="font-size:20px;color:darkgreen">288</span> 升 / 柴油</p>
                    <p style="color:lightsalmon;text-align:right">后天</p>
                </div>
            </yd-cell-item>
        </yd-cell-group>
    </div>
</template>
<!--<script src="./planboard.ts"></script>-->