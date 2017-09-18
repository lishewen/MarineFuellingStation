declare module server {
	/** 转仓单 */
	interface moveStore extends EntityBase {
		/** 生产员 */
		manufacturer: string;
		outStoreTypeId: number;
		outStoreId: number;
		/** 密度 */
		outDensity: number;
		/** 油温 */
		outTemperature: number;
		/** 计划转出 */
		outPlan: number;
		/** 实际转出 */
		outFact: number;
		inStoreTypeId: number;
		inStoreId: number;
		/** 密度 */
		inDensity: number;
		/** 油温 */
		inTemperature: number;
		/** 实际转入 */
		inFact: number;
		/** 安排转入 */
		inPlan: number;
		startTime: Date;
		endTime?: Date;
		/** 耗时（分钟） */
		elapsed: number;
		/** 生产单状态 */
		state: any;
		/** 误差 */
		deviation: number;
	}
	const enum moveStoreState {
		已开单,
		施工中,
		已完成,
	}
}
