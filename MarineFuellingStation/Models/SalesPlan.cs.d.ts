declare module server {
	/** 销售计划 Name字段为单号 */
	interface salesPlan extends entityBase {
		salesPlanType: salesPlanType;
		carNo: string;
		productId: number;
		/** 油品名 */
		oilName: string;
		price: number | string;
		count: number;
		/** 单位 */
		unit: string;
		/** 当前余油 */
		remainder: number;
		oilDate: Date;
		/** 是否开票 */
		isInvoice: boolean;
		ticketType: ticketType;
		/** 开票单位 */
		billingCompany: string;
		/** 开票单价 */
		billingPrice: number;
		/** 开票数量 */
		billingCount: number;
		state: salesPlanState;
		/** 审核人 */
		auditor: string;
		/** 审核时间 */
		auditTime?: Date;
		totalMoney: number;
	}
	const enum salesPlanType {
		水上,
		陆上,
        机油,
        全部
	}
	const enum salesPlanState {
		未审批,
		已审批,
		已完成,
	}
}
