declare module server {
	interface product extends entityBase {
		/** 底价 */
		minPrice: number;
	}
	interface productType extends entityBase {
		products: any[];
	}
}
