<template>
    <div>
        <yd-grids-group :rows="2" title="2017-07-07 出仓汇总">
            <yd-grids-item v-for="s in salesSts" :key="s.id">
                <p slot="text">{{s.name}}</p>
                <p slot="text" style="color: lightgray">总：xxxL</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-cell-group :title="st.name" style="margin-top:20px" v-for="st in sts" :key="st.id">
            <yd-cell-item arrow @click.native="show1 = true" v-for="s in st.stores" :key="s.id">
                <div slot="left" style="width: 60px;height: 60px;margin:15px 15px 15px -5px">
                    <yd-progressbar :progress="getPercent(s.value, s.volume)" trailWidth="4">{{s.value}}</yd-progressbar>
                </div>
                <div slot="left" style="width: 60px;height: 60px;margin:10px 10px 10px -5px">
                    <yd-progressbar :progress="progress2"
                                    trailWidth="2"
                                    strokeWidth="4"
                                    strokeColor="#B2B2B2"
                                    trailColor="#FE5D51"
                                    fillColor="#EAEAEA">{{s.volume - s.value}}</yd-progressbar>
                </div>
                <div slot="left" style="font-size:16px;width: 90px">
                    <p style="color:gray;font-size:16px;">{{s.name}}</p>
                    <p style="color:lightgray;font-size:14px;">07-07 08:30 已测</p>
                </div>
                <div slot="right">
                    <div style="text-align: left">
                        <p style="color:gray">今 <span style="color:red">+1000</span> 升</p>
                        <p style="color:lightgray">今 <span style="color:green">-800</span> 升</p>
                    </div>
                </div>
            </yd-cell-item>
        </yd-cell-group>
        <yd-popup v-model="show1" position="right">
            <yd-cell-group title="测量">
                <yd-cell-item>
                    <span slot="left">油温：</span>
                    <yd-input type="number" slot="right" v-model="survey.temperature" placeholder="请输入" required></yd-input>
                    <span slot="right">℃</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">密度：</span>
                    <yd-input type="number" slot="right" v-model="survey.density" placeholder="请输入" required></yd-input>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="left">油高：</span>
                    <yd-input type="number" slot="right" v-model="survey.height" placeholder="请输入" required></yd-input>
                    <span slot="right">cm</span>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:80%" type="primary" @click.native="postSurveyclick">提交</yd-button>
            </div>
        </yd-popup>
    </div>
</template>
<script src="./oilstore.ts"></script>