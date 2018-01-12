declare module server {
	/** 销售单 Name字段为单号 */
	interface order extends entityBase {
        salesPlanId?: number;
        salesPlan: salesPlan;
		orderType: any;
		/** 船号/车号 */
        carNo: string;
        /** 销售员 */
        salesman: string;
		/** 客户 */
        clientId?: number;
        client: client;
        productId: number;
        product: product;
		/** 商品单价 */
		price: number | string;
		count: number;
		/** 单位 */
		unit: string;
		totalMoney: number;
		/** 是否开票 */
        isInvoice: boolean;
        /** 送货上门/自提 */
        isDeliver: boolean;
        /** 送货上门 运费 */
        deliverMoney: number;
        /** 是否打印单价 送货 */
        isPrintPrice: boolean;
		/** 开票单位 */
		billingCompany: string;
		/** 开票单价 */
		billingPrice: number;
		/** 开票数量 */
		billingCount: number;
		/** 实际加油数量 */
        oilCount: number;
        /** 实际加油数量 单位：升 */
        oilCountLitre: number;
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
		/** 销售超额提成：高于商品最低限价，按 差值（升） * 数量 * 0.2 / 1200 或 差值（吨） * 数量 * 0.2 */
		salesCommission: number;
		transportOrderId?: number;
		/** 订单状态 */
		state: any;
		/** 订单支付状态 */
		payState: any;
		ticketType: any;
		/** 是否运输 */
		isTrans: boolean;
		/** 油车磅秤图片地址 陆上装车 */
		oilCarWeightPic: string;
		/** 空车车磅秤图片地址 陆上装车 */
		emptyCarWeightPic: string;
		/** 销售仓Id 陆上装车 */
		storeId?: number;
        /** 对应油仓 */
        store: store;
		payments: any[];
		/** 最低限价 */
        minPrice: number;
        /** 备注 */
        remark: string;
        /** 收银员 */
        cashier: string;
        /** 单据是否删除标识 */
        isDel: boolean;
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
        桂行刷卡,
        工行刷卡,
		刷卡三,
        账户扣减,
        公司账户扣减
	}
}
