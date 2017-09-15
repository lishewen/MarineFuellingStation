declare module server {
	/** 测量记录 */
	interface survey extends entityBase {
		storeId: number;
		store: store;
		/** 油温 */
		temperature: number | string;
		/** 密度 */
		density: number | string;
		/** 油高 */
		height: number | string;
	}
}
