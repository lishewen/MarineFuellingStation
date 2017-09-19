declare module server {
	interface client extends entityBase {
		placeType: placeType;
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
		company: company;
		product: product;
	}
	const enum placeType {
		陆上,
		水上,
	}
	const enum clientType {
		个人,
		公司,
		全部,
	}
}
