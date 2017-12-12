declare module server {
	/** 测量记录 */
	interface survey extends entityBase {
		storeId: number;
		store: store;
		/** 油温 */
		temperature: number;
		/** 密度 */
		density: number;
		/** 油高 */
        height: number;
        /** 油高对应的升数 */
        count: number;
	}
}
