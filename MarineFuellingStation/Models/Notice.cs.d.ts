declare module server {
	const enum apps {
		销售计划,
		销售单,
		陆上卸油,
		陆上装油,
		水上加油
	}
	/** 通知 */
	interface notice extends entityBase {
		/** 内容 */
		content: string;
		/** 是否启用 */
		isUse: boolean;
		/** 所通知的应用，多应用用'|'分开 */
		toApps: string;
	}
}
