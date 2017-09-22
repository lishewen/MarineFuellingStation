declare module server {
	/** 出入仓记录 */
	interface inAndOutLog extends EntityBase {
		type: any;
		storeId: number;
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
		};
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
		全部,
	}
}
