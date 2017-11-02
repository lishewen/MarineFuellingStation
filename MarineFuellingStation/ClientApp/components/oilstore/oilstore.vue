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
                    <yd-progressbar :progress="getPercent(s.sumInValue, s.sumInValue+s.sumOutValue)" trailWidth="4">{{strInOutDiff(s.sumInValue, s.sumOutValue)}}</yd-progressbar>
                </div>
                <div slot="left" style="width: 60px;height: 60px;margin:10px 10px 10px -5px">
                    <yd-progressbar :progress="getPercent(s.value,s.volume)"
                                    trailWidth="2"
                                    strokeWidth="4"
                                    strokeColor="#B2B2B2"
                                    trailColor="#FE5D51"
                                    fillColor="#EAEAEA">{{s.value}}</yd-progressbar>
                </div>
                <div slot="left">
                    <p style="color:gray;font-size:.4rem;">{{s.name}}</p>
                    <p style="color:lightgray;font-size:14px;">{{s.density}}</p>
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
            <yd-cell-group :title="selectStore.name + ' - 测量'">
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
                <yd-cell-item>
                    <span slot="left">对应升数：</span>
                    <yd-input type="number" slot="right" v-model="survey.count" placeholder="请输入" required></yd-input>
                    <span slot="right">升</span>
                </yd-cell-item>
                <yd-cell-item>
                    <span slot="right">{{strToton()}}吨</span>
                </yd-cell-item>
            </yd-cell-group>
            <div style="text-align: center">
                <yd-button style="width:80%" type="primary" @click.native="postSurveyclick">提交测量记录</yd-button>
            </div>
        </yd-popup>
        <yd-popup v-model="show2" position="right" width="70%">
            <yd-cell-group title="最近十五次测量">
                <yd-cell-item v-for="su in surveys" :key="su.id">
                    <div slot="left">
                        <p>{{su.createdBy}}</p>
                        <p>{{formatDate(su.createdAt)}}</p>
                    </div>
                    <div slot="right" style="text-align: left;padding: .2rem 0 .2rem">
                        <p>油温：{{su.temperature}}℃</p>
                        <p>密度：{{su.density}}</p>
                        <p>油高：{{su.height}}cm</p>
                        <p>对应升数：{{su.count}}</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <yd-popup v-model="showAssays" position="right" width="70%">
            <yd-cell-group title="化验记录">
                <yd-cell-item v-for="a in assays" :key="a.id">
                    <div slot="left" style="line-height: 24px">
                        <p>时间：</p>
                        <p>化验员：</p>
                        <p>视密：</p>
                        <p>标密：</p>
                        <p>闭口闪点：</p>
                        <p>初硫：</p>
                        <p>回流：</p>
                    </div>
                    <div slot="right"  style="line-height: 24px">
                        <p>{{formatDate(a.createdAt)}}</p>
                        <p>{{a.createdBy}}</p>
                        <p>{{a.视密}}</p>
                        <p>{{a.标密}}</p>
                        <p>{{a.闭口闪点}}</p>
                        <p>{{a.初硫}}</p>
                        <p>{{a.回流}}</p>
                    </div>
                </yd-cell-item>
            </yd-cell-group>
        </yd-popup>
        <!--actionsheet-->
        <yd-actionsheet :items="actItems" v-model="showAct" cancel="取消"></yd-actionsheet>
    </div>
</template>
<script src="./oilstore.ts"></script>