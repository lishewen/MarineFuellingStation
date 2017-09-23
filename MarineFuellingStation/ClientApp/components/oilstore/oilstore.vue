<template>
    <div>
        <yd-grids-group rows="2" :title="datestr + ' 出仓汇总'">
            <yd-grids-item v-for="s in salesSts" :key="s.id">
                <p slot="text">{{s.name}}</p>
                <p slot="text" style="color: lightgray">总：{{s.sumOutValue}}L</p>
            </yd-grids-item>
        </yd-grids-group>
        <yd-cell-group :title="st.name" style="margin-top:20px" v-for="st in sts" :key="st.id">
            <yd-cell-item arrow @click.native="storeclick(s)" v-for="s in st.stores" :key="s.id">
                <div slot="left" style="width: 60px;height: 60px;margin:15px 15px 15px -5px">
                    <yd-progressbar :progress="getPercent(s.sumInValue, s.sumInValue+s.sumOutValue)" trailWidth="4">{{s.sumInValue}}</yd-progressbar>
                </div>
                <div slot="left" style="width: 60px;height: 60px;margin:10px 10px 10px -5px">
                    <yd-progressbar :progress="getPercent(s.value,s.volume)"
                                    trailWidth="2"
                                    strokeWidth="4"
                                    strokeColor="#B2B2B2"
                                    trailColor="#FE5D51"
                                    fillColor="#EAEAEA">{{s.volume - s.value}}</yd-progressbar>
                </div>
                <div slot="left">
                    <p style="color:gray;font-size:.4rem;">{{s.name}}</p>
                    <p style="color:lightgray;font-size:14px;">{{formatDate(s.lastSurveyAt)}}</p>
                </div>
                <div slot="right">
                    <div style="text-align: left">
                        <p style="color:gray">今 <span style="color:red">+{{s.sumInValue}}</span> 升</p>
                        <p style="color:lightgray">今 <span style="color:green">-{{s.sumOutValue}}</span> 升</p>
                    </div>
                </div>
            </yd-cell-item>
        </yd-cell-group>
        <yd-popup v-model="show1" position="right" width="70%">
            <yd-cell-group :title="selectStore.name">
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
                <yd-button style="width:80%" type="primary" @click.native="postSurveyclick">提交新数据</yd-button>
            </div>
            <yd-cell-group title="最近十次测量" style="margin-top: 20px">
                <yd-cell-item v-for="su in surveys" :key="su.id">
                    <div slot="left">{{formatDate(su.createdAt)}}</div>
                    <div slot="right" style="text-align: left;padding: .2rem 0 .2rem">
                        <p>油温：{{su.temperature}}℃</p>
                        <p>密度：{{su.density}}</p>
                        <p>油高：{{su.height}}cm</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
    </div>
</template>
<script src="./oilstore.ts"></script>