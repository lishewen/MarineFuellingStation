declare module server {
	/** 化验单 */
	interface assay extends entityBase {
		assayType: assayType;
		storeId?: number;
		/** 化验油仓 */
		store: store;
		purchaseId?: number;
		/** 采购来源 */
		purchase: purchase;
		视密: number | string;
		标密: number | string;
		闭口闪点: string;
		/** 油温 */
		temperature: number;
		/** 味道 */
		smellType: smellType;
		混水反应: string;
		十六烷值: string;
		初硫: number | string;
		/** 10% */
		percentage10: number;
		/** 50% */
		percentage50: number;
		/** 90% */
		percentage90: number;
		回流: number;
		干点: number;
		/** 化验员 */
		assayer: string;
		isUse: boolean;
	}
	const enum assayType {
		油舱化验,
		采购化验,
	}
	const enum smellType {
		一般刺鼻,
		刺鼻,
		不刺鼻,
	}
}
