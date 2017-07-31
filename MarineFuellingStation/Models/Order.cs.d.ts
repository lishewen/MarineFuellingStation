declare module server {
	/** 销售单 Name字段为单号 */
	interface order extends entityBase {
		salesPlanId?: number;
		salesPlan: salesPlan;
		orderType: salesPlanType;
		carNo: string;
		productId: number;
		product: product;
		price: number;
		count: number;
		/** 单位 */
		unit: string;
		totalMoney: number;
		/** 是否开票 */
		isInvoice: boolean;
		/** 开票单位 */
		billingCompany: string;
		/** 开票单价 */
		billingPrice: number;
		/** 开票数量 */
		billingCount: number;
		/** 实际加油数量 */
		oilCount: number;
		/** 生产员 以'|'区分多个 */
		worker: string;
		/** 开始装油时间 */
		startOilDateTime?: Date;
		/** 结束装油时间 */
		endOilDateTime?: Date;
		/** 表1 */
		instrument1: number;
		/** 表2 */
		instrument2: number;
		/** 表3 */
		instrument3: number;
		/** 密度 */
		density: number;
		/** 油温 */
		oilTemperature: number;
		/** 实际与订单差 */
		diffOil: number;
		/** 皮重 陆上 */
		emptyCarWeight: number;
		/** 毛重 陆上 */
		oilCarWeight: number;
		/** 油重 陆上 */
		diffWeight: number;
		/** 销售提成 */
		salesCommission: number;
		transportOrderId?: number;
	}
}
