declare module server {
	/** 充值记录 */
	interface chargeLog extends entityBase {
        /** 类型 */
        chargeType: chargeType;
		/** 金额 */
		money: number;
        /** 支付方式 */
        payType: orderPayType;
        /** 客户Id */
        clientId?: number;
        /** 客户Id */
        client: client;
        /** 公司名称 */
        companyName: string;
	}
	const enum chargeType {
		充值,
		消费,
	}
}
