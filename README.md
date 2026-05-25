# AE-Development based in Shanghai

此项目是基于上海地区的路径规划示例应用，使用 ArcGIS Runtime 与 WinForms 开发。

## 功能特性
- 支持 **车行**（速度 40 km/h） 与 **人行**（速度 5 km/h） 两种出行模式
- 在地图右上角以悬浮窗形式展示 **路径长度** 与 **预计行驶时间**
- 可选择起点、途经点、终点进行路径计算

## 使用说明
1. 打开 `Form1.cs`，在 `button1_Click` 中会根据选择的出行模式自动计算时间。
2. 结果会通过 `ShowPathResult` 方法显示在右上角悬浮窗，支持多行显示。
3. 如需修改速度，可在 `button1_Click` 中调整 `speed` 的值（单位：m/s）。

## 环境要求
- .NET Framework（WinForms）
- ArcGIS Runtime SDK for .NET（已在项目引用中）

## 贡献
欢迎提交 Pull Request，帮助完善功能或修复 Bug。
