declare module server {
	/** 实体基类 */
	interface entityBase {
		/** 主键Id (主键类型根据继承时确定) */
		id: number;
		/** 名称 */
		name: string;
		/** 创建时间 */
		createTime: Date;
	}
}
