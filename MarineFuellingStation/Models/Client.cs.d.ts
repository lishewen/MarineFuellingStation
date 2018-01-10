declare module server {
	interface client extends entityBase {
        /** 陆上?水上? */
        placeType: placeType;
        /** 客户类型（个人、公司) */
        clientType: clientType;
		companyId?: number;
		/** 跟进销售 */
		followSalesman: string;
		carNo: string;
		defaultProductId?: number;
		/** 联系人 */
		contact: string;
		mobile: string;
		idCard: string;
		address: string;
		/** 固定电话 */
		phone: string;
        /** 最高挂账金额 */
        maxOnAccount: number | string;
		/** 账户余额 */
		balances: number;
		/** 总消费金额 */
		totalAmount: number;
		/** 是否标注 用于标记当前客户，方便销售区分客户是否联系过 */
		isMark: boolean;
		/** 备注 */
        remark: string;
        company: company;
        product: product;
	}
	const enum placeType {
		陆上,
        水上,
        全部
	}
	const enum clientType {
		个人,
		公司,
        全部,
        无销售员
	}
}
