declare module server {
	interface product extends entityBase {
        /** 底价 */
        minPrice: number;
        /** 开票底价 */
        minInvoicePrice: number;
		/** 最后报价 */
		lastPrice: number;
		isUse: boolean;
        productTypeId: number;
        isForLand: boolean | string;
        unit: string;
	}
    interface productType extends entityBase {
        products: product[];
	}
}
