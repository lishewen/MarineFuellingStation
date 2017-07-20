declare module server {
	/** 销售计划 Name字段为单号 */
	interface salesPlan extends entityBase {
		salesPlanType: salesPlanType;
		carNo: string;
		productId: number;
		price: number;
		count: number;
		/** 单位 */
		unit: string;
		/** 当前余油 */
		remainder: number;
		oilDate: Date;
		isInvoice: boolean;
		totalMoney: number;
	}
	const enum salesPlanType {
		水上,
		陆上,
		机油,
	}
}
