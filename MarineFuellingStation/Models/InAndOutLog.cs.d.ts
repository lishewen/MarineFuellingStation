declare module server {
	/** 出入仓记录 */
    interface inAndOutLog extends entityBase {
        type: logType;
        storeId: number;
        store: store;
		/** 操作员 | 分隔 */
		operators: string;
		/** 单位 */
		unit: string;
		/** 对应单位的值 */
        value: number;
        /** 单位为升的值 */
        valueLitre: number;
	}
	const enum logType {
		出仓,
		入仓,
		全部
	}
}
