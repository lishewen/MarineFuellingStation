declare module server {
	interface product extends EntityBase {
		/** 底价 */
		minPrice: number;
		/** 最后报价 */
		lastPrice: number;
		isUse: boolean;
		productTypeId: number;
	}
	interface productType extends EntityBase {
		products: any[];
	}
}
