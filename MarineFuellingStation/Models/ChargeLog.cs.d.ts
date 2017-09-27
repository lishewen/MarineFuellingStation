declare module server {
	/** 充值记录 */
	interface chargeLog extends entityBase {
        /** 类型 */
        chargeType: chargeType;
		/** 金额 */
		money: number;
        /** 支付方式 */
        payType: orderPayType;
	}
	const enum chargeType {
		充值,
		消费,
	}
}
