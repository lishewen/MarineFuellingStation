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
        /** 客户 */
        client: client;
        /** 公司Id */
        companyId?: number;
        /** 公司 */
        company: company;
        /** 标识个人还是充值 */
        isCompany: boolean;
	}
	const enum chargeType {
		充值,
		消费,
	}
}
