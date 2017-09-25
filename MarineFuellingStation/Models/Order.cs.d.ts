declare module server {
	/** 销售单 Name字段为单号 */
	interface order extends EntityBase {
		salesPlanId?: number;
		salesPlan: {
			salesPlanType: any;
			carNo: string;
			productId: number;
			oilName: string;
			price: number;
			count: number;
			unit: string;
			remainder: number;
			oilDate: Date;
			isInvoice: boolean;
			ticketType: any;
			billingCompany: string;
			billingPrice: number;
			billingCount: number;
			state: any;
			auditor: string;
			auditTime?: Date;
			totalMoney: number;
		};
		orderType: any;
		/** 船号/车号 */
		carNo: string;
		/** 客户 */
		clientId?: number;
		client: {
			placeType: any;
			clientType: any;
			companyId?: number;
			followSalesman: string;
			carNo: string;
			defaultProductId?: number;
			contact: string;
			mobile: string;
			idCard: string;
			address: string;
			phone: string;
			maxOnAccount: number;
			balances: number;
			totalAmount: number;
			company: {
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
		};
		productId: number;
		product: {
			minPrice: number;
			lastPrice: number;
			isUse: boolean;
			productTypeId: number;
			productType: {
				products: any[];
			};
		};
		/** 商品单价 */
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
		/** 皮重 陆上（空车） */
		emptyCarWeight: number;
		/** 毛重 陆上(油 + 车) */
		oilCarWeight: number;
		/** 油重 陆上 */
		diffWeight: number;
		/** 销售提成 */
		salesCommission: number;
		transportOrderId?: number;
		/** 订单状态 */
		state: any;
		/** 订单支付状态 */
		payState: any;
		ticketType: any;
		/** 是否运输 */
		isTrans: boolean;
		/** 油车磅秤图片地址 陆上加油 */
		oilCarWeightPic: string;
		/** 空车车磅秤图片地址 陆上加油 */
		emptyCarWeightPic: string;
		/** 销售仓Id 陆上加油 */
		storeId?: number;
		/** 对应油仓 */
		store: {
			storeClass: any;
			volume: number;
			avgPrice: number;
			lastValue: number;
			value: number;
			lastSurveyAt: Date;
			cost: number;
			isUse: boolean;
			storeTypeId: number;
			storeType: {
				stores: any[];
			};
			sumOutValue: number;
			sumInValue: number;
		};
		payments: any[];
		/** 底价 */
		minPrice: number;
	}
	const enum orderState {
		已开单,
		选择油仓,
		空车过磅,
		装油中,
		油车过磅,
		已完成,
	}
	const enum payState {
		未结算,
		已结算,
		挂账,
	}
	const enum orderPayType {
		现金,
		微信,
		支付宝,
		刷卡一,
		刷卡二,
		刷卡三,
		账户扣减,
	}
}
