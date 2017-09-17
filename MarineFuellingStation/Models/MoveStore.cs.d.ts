declare module server {
	/** 转仓单 */
	interface moveStore extends entityBase {
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
		inStoreTypeId?: number | string;
		inStoreId?: number | string;
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
        /** 误差 */
        state: moveStoreState;
		/** 误差 */
		deviation: number;
    }
    /** 用于GET的model */
    interface moveStoreGET {
        outStoreTypeName: string;
        inStoreTypeName: string;
        outStoreName: string;
        inStoreName: string;
        stateName: string;
        outPlan: number;
        name: string;
        lastUpdateAt: string;
    }
    enum moveStoreState {
        已开单,
        施工中,
        已完成
    }
}
