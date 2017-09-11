declare module server {
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
		totalMoney: number;
	}
}
