declare module server {
	interface store extends entityBase {
		storeClass: storeClass;
		/** 容量 */
		volume: number;
		/** 平均单价成本 */
		avgPrice: number;
		/** 上次数量 */
		lastValue: number;
		/** 数量 */
		value: number;
		/** 当前价值 */
		cost: number;
		isUse: boolean;
		storeTypeId: number;
	}
	const enum storeClass {
		销售仓,
		存储仓,
	}
	interface storeType extends entityBase {
		stores: any[];
	}
}
