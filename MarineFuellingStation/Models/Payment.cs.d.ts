declare module server {
	/** 付款方式记录表 */
	interface payment extends entityBase {
		/** 支付方式 */
		payTypeId: any;
		/** 金额 */
		money: number;
		/** 订单Id */
		orderId?: number;
	}
}
