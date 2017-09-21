declare module server {
	interface userDTO {
		/** 企业微信部分信息 */
		workInfo: work.memberResult;
		/** 本地数据库信息 */
		localInfo: server.user;
	}
}
