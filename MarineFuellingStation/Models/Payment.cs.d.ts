declare module server {
	/** name 为order的单号 */
	interface payment extends entityBase {
        /** 支付方式 */
        payTypeId: orderPayType;
		/** 金额 */
        money: number;
        /** 订单Id */
        orderId: number;
	}
}
