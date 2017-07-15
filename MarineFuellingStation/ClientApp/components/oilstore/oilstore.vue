<template>
    <div>
        <yd-grids-group :rows="2" title="2017-07-07 汇总">
            <yd-grids-item>
                <p slot="text">93#油</p>
                <p slot="text" style="color: lightgray">总：xxxL</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">95#油</p>
                <p slot="text" style="color: lightgray">总：xxxL</p>
            </yd-grids-item>
            <yd-grids-item>
                <p slot="text">97#油</p>
                <p slot="text" style="color: lightgray">总：xxxL</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-cell-group title="船一" style="margin-top:20px">
            <yd-cell-item arrow type="link" href="#">
                <div slot="left" style="width: 80px;height: 80px;padding:10px">
                    <yd-progressbar :progress="progress2"
                                    trailWidth="2"
                                    strokeWidth="4"
                                    strokeColor="#B2B2B2"
                                    trailColor="#FE5D51"
                                    fillColor="#EAEAEA">{{progress1 * 100}}%</yd-progressbar>
                </div>
                <div slot="left" style="font-size:18px;padding-left:10px">
                    <p>1#仓</p>
                    <p>300.99升</p>
                </div>
                <div slot="left" style="padding-left: 20px">
                    <p style="color:gray">进： 今 <span style="color:red">+1000</span> 升</p>
                    <p style="color:lightgray">出： 今 <span style="color:green">-800</span> 升</p>
                </div>
            </yd-cell-item>
        </yd-cell-group>
        <yd-cell-group title="船二">
            <yd-cell-item arrow type="link" href="#">
                <div slot="left" style="width: 80px;height: 80px;padding:10px">
                    <yd-progressbar :progress="progress2"
                                    trailWidth="2"
                                    strokeWidth="4"
                                    strokeColor="#B2B2B2"
                                    trailColor="#FE5D51"
                                    fillColor="#EAEAEA">{{progress2 * 100}}%</yd-progressbar>
                </div>
                <div slot="left" style="font-size:18px;padding-left:10px">
                    <p>1#仓</p>
                    <p>300.99升</p>
                </div>
                <div slot="left" style="padding-left: 20px">
                    <p style="color:gray">进： 今 <span style="color:red">+1000</span> 升</p>
                    <p style="color:lightgray">出： 今 <span style="color:green">-800</span> 升</p>
                </div>
            </yd-cell-item>
            <yd-cell-item arrow type="link" href="#">
                <div slot="left" style="width: 80px;height: 80px;padding:10px">
                    <yd-progressbar :progress="progress2"
                                    trailWidth="2"
                                    strokeWidth="4"
                                    strokeColor="#B2B2B2"
                                    trailColor="#FE5D51"
                                    fillColor="#EAEAEA">{{progress1 * 100}}%</yd-progressbar>
                </div>
                <div slot="left" style="font-size:18px;padding-left:10px">
                    <p>1#仓</p>
                    <p>300.99升</p>
                </div>
                <div slot="left" style="padding-left: 20px">
                    <p style="color:gray">进： 今 <span style="color:red">+1000</span> 升</p>
                    <p style="color:lightgray">出： 今 <span style="color:green">-800</span> 升</p>
                </div>
            </yd-cell-item>
            <yd-cell-item arrow type="link" href="#">
                <div slot="left" style="width: 80px;height: 80px;padding:10px">
                    <yd-progressbar :progress="progress2"
                                    trailWidth="2"
                                    strokeWidth="4"
                                    strokeColor="#B2B2B2"
                                    trailColor="#FE5D51"
                                    fillColor="#EAEAEA">{{progress2 * 100}}%</yd-progressbar>
                </div>
                <div slot="left" style="font-size:18px;padding-left:10px">
                    <p>1#仓</p>
                    <p>300.99升</p>
                </div>
                <div slot="left" style="padding-left: 20px">
                    <p style="color:gray">进： 今 <span style="color:red">+1000</span> 升</p>
                    <p style="color:lightgray">出： 今 <span style="color:green">-800</span> 升</p>
                </div>
            </yd-cell-item>
        </yd-cell-group>
    </div>
</template>
<script src="./oilstore.ts"></script>