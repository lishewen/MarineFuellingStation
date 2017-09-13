declare module server {
	/** 测量记录 */
	interface survey extends EntityBase {
		/** 油温 */
		temperature: number;
		/** 密度 */
		density: number;
		/** 油高 */
		height: number;
	}
}
