declare module server {
    const enum unloadState {
        已开单,
        已到达,
        选择油仓,
        油车过磅,
        化验,
        卸油中,
        空车过磅,
        完工,
        已审核
    }
    /** 进油计划 */
    interface purchase extends entityBase {
        productId: number;
        product: product;
        price: number;
        /** 计划订单数量 单位吨 */
        count: number;
        /** 实际卸油数量 单位升 */
        oilCount: number;
        diffLitre: number;
        diffTon: number;
        /** 始发地 */
        origin: string;
        startTime?: string;
        /** 预计到达时间 */
        arrivalTime?: string;
        carNo: string;
        /** 挂车号 */
        trailerNo: string;
        driver1: string;
        idCard1: string;
        phone1: string;
        driver2: string;
        idCard2: string;
        phone2: string;
        /** 卸油后表数1 */
        instrument1: number;
        /** 卸油后表数2 */
        instrument2: number;
        /** 卸油后表数3 */
        instrument3: number;
        /** 审核人 */
        auditor: string;
        /** 审核时间 */
        audiTime: string;
        /** 卸油选择的多个油仓 */
        toStoresList: toStore[];
        /** 卸车时需要测量的密度 */
        density: number;
        /** 油车磅秤数 毛重 */
        scaleWithCar: number;
        /** 油车磅秤图片地址 */
        scaleWithCarPic: string;
        /** 空车磅秤数 皮重 */
        scale: number;
        /** 油车磅秤图片地址 */
        scalePic: string;
        /** 油重 净重 */
        diffWeight: number;
        /** 化验单 */
        assay: assay;
        /** 状态 */
        state: unloadState;
        totalMoney: number;
        /** 施工人员 */
        worker: string;
        /** 单据是否删除标识 */
        isDel: boolean;

    }
    /** 选择多个油仓卸油用到的Model **/
    interface toStore {
        id: number;
        count?: number;
        name: string;
        //卸油前表数
        instrumentBf?: number;
        //卸油后表数
        instrumentAf?: number;
    }
}
