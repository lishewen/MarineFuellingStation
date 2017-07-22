declare module server {
	interface product extends entityBase {
		/** 底价 */
		minPrice: number;
		/** 最后报价 */
		lastPrice: number;
		isUse: boolean;
	}
	interface productType extends entityBase {
        products: product[];
	}
}
