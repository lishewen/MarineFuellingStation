declare module server {
	const enum unloadState {
		已开单,
        已到达,
        选择油仓,
		油车过磅,
		化验,
		卸油中,
		空车过磅,
        完工
	}
	/** 采购计划 */
	interface purchase extends entityBase {
        productId: number;
        product: product;
		price: number;
		count: number;
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
        /** 卸油表表数1 */
        instrument1: number;
        /** 卸油表表数2 */
        instrument2: number;
        /** 卸油表表数3 */
        instrument3: number;
        /** 卸油用油仓id */
        storeId?: number;
        /** 卸油油仓 */
        store: store;
        /** 卸车时需要测量的密度 */
        density: number;
		/** 油车磅秤数 */
		scaleWithCar: number;
		/** 油车磅秤图片地址 */
		scaleWithCarPic: string;
		/** 空车磅秤数 */
		scale: number;
		/** 油车磅秤图片地址 */
		scalePic: string;
        /** 化验单 */
        assay: assay;
        /** 状态 */
        state: unloadState;
		totalMoney: number;
	}
}
