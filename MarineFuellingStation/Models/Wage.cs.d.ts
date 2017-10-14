declare module server {
	interface wage extends entityBase {
		年月: number;
		职务: string;
		departmentId: number;
		基本: number;
        出勤天数: number;
        绩效工资: number;
		提成: number;
		超额: number;
		交通: number;
		应付: number;
		社保: number;
		请假: number;
		餐费: number;
		借支: number;
		安全保障金: number;
		实发: number;
		转卡金额: number;
		现金: number;
	}
}
