declare module server {
	const enum unloadState {
		已开单,
		已到达,
		已油车过磅,
		已化验,
		卸油中,
		卸油结束,
		已空车过磅,
		完工,
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
