declare module server {
	/** 船舶清污单 */
	interface boatClean extends entityBase {
		/** 船号 */
		carNo: string;
		/** 金额 */
		money: number;
		/** 航次 */
		voyage: number;
		/** 吨位 */
		tonnage: number;
		/** 批文号 */
		responseId: string;
		/** 作业地点 */
		address: string;
		/** 作业单位 */
		company: string;
		/** 联系电话 */
		phone: string;
		/** 是否开票 */
		isInvoice: boolean;
		/** 开票单位 */
		billingCompany: string;
		/** 开票单价 */
		billingPrice: number;
		/** 开票数量 */
		billingCount: number;
		/** 开始作业时间 */
		startTime: Date;
		/** 完成时间 */
		endTime: Date;
        state: boatCleanState;
        payState: boatCleanPayState;
        payments: payment[];
	}
	const enum boatCleanState {
		已开单,
		施工中,
		已完成,
    }
    const enum boatCleanPayState {
        未结算,
        已结算,
        挂账
    }
}
