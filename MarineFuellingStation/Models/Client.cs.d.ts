declare module server {
    interface client extends entityBase {
		placeType: any;
		clientType: any;
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
		maxOnAccount: number;
		/** 账户余额 */
		balances: number;
		/** 总消费金额 */
        totalAmount: number;
        /** 最近消费时间 */
        lastUpdatedAt: string;
        company: {
            name: string;
			keyword: string;
			ticketType: any;
			invoiceTitle: string;
			taxFileNumber: string;
			businessAccount: string;
			bank: string;
			address: string;
			phone: string;
			balances: number;
            totalAmount: number;
		};
		product: {
			minPrice: number;
			lastPrice: number;
			isUse: boolean;
			productTypeId: number;
			productType: {
				products: any[];
			};
		};
	}
	const enum placeType {
		陆上,
		水上,
	}
	const enum clientType {
		个人,
		公司,
	}
}
