declare module server {
	interface user extends entityBase {
		userId: string;
		/** 入职时间 */
		reportDutyTime: Date;
		/** 基本工资 */
		baseWage: number;
		/** 社保自负 */
		socialSecurity: number;
		/** 安全保障金 */
		security: number;
		/** 身份证 */
		iDCard: string;
		/** 地址 */
		address: string;
		/** 开户银行 */
		bank: string;
		/** 银行账户 */
		account: string;
		/** 开户人 */
		accountName: string;
		/** 离职时间 */
		leaveTime?: Date;
		/** 是否离职 */
		isLeave: boolean;
	}
}
