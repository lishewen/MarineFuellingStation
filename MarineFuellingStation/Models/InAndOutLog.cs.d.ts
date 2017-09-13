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
		/** 值 */
		value: number;
	}
	const enum logType {
		出仓,
		入仓,
	}
}
