declare module server {
    interface company extends entityBase {
		/** 搜索用的关键字 */
		keyword: string;
		ticketType: ticketType;
		/** 发票抬头 */
		invoiceTitle: string;
		/** 税号 */
		taxFileNumber: string;
		/** 对公账户 */
		businessAccount: string;
		/** 开户银行 */
		bank: string;
		address: string;
		phone: string;
		/** 账户余额 */
		balances: number;
		/** 总消费金额 */
		totalAmount: number;
	}
	const enum ticketType {
		普通票,
		专用票,
	}
}
