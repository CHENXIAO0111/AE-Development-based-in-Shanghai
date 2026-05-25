# AE-Development 基于上海的路径规划

**项目简介**
此项目展示了基于 **ArcGIS Runtime SDK for .NET** 与 **WinForms** 实现的上海地区路径规划示例。用户可以在地图上选择起点、终点和途经点，系统根据选择的 **车行**（40 km/h）或 **人行**（5 km/h）模式计算最短路径，并在地图右上角以悬浮窗形式展示 **路径长度** 与 **预计行驶时间**，支持导出路径为 Shapefile。

---

## 功能特性
- 支持两种出行模式（车行 / 人行）并自动换算预计行驶时间
- 悬浮窗实时显示路径信息，支持多行文本
- 可选择起点、途经点、终点进行路径规划
- 支持导出计算结果为 **Shapefile**（.shp）
- 遗产点查询侧边栏，可快速定位上海优秀建筑与文物点
- UI 采用现代 WinForms 设计，布局自适应窗口大小

---

## 项目结构
| 文件/目录 | 说明 |
|-----------|------|
| `Form1.cs` / `Form1.Designer.cs` | 主窗体逻辑与 UI 布局，包含路径计算、悬浮窗显示等核心功能 |
| `FrmBuildingHeritageQuery.cs` / `FrmBuildingHeritageQuery.Designer.cs` | 遗产点查询侧边栏，实现点数据加载、选择与导出 |
| `Program.cs` | 程序入口 |
| `API.md` | 完整的类、方法、事件文档（已生成） |
| `README.md` | 本文件，提供项目概览与使用指南 |
| `heritages_cn.*` | 遗产点数据文件（shp、dbf、shx 等） |
| `network_peo.*` | 路网数据文件 |
| `bin/`、`obj/` | 编译产物 |

---

## 环境要求
- **操作系统**：Windows 10/11（64 位）
- **开发环境**：Visual Studio 2022（或更高）
- **框架**：.NET Framework（WinForms）
- **依赖**：ArcGIS Runtime SDK for .NET（已在项目引用中）
- **Git**：用于代码管理与推送

---

## 快速开始
1. **克隆仓库**（已完成）或直接打开本地目录。
2. 在 Visual Studio 中打开 `Route_of_Shanghai.sln`（若不存在，请自行创建）。
3. **还原 NuGet 包**，确保 ArcGIS SDK 正常加载。
4. 编译并运行项目。
5. 在界面中选择 **起点 / 途经点 / 终点**，在 **出行模式** 下拉框中选择 **车行** 或 **人行**，点击 **计算路径**（`button1`）。
6. 右上角悬浮窗会显示 **路径长度** 与 **预计行驶时间**；如需导出，使用 **文件 → 导出路径** 功能。

---

## 使用说明（详细）
1. **路径计算**：`button1_Click` 根据 `cbo_TravelMode` 选取的模式设置速度（车行 40 km/h，步行 5 km/h），调用 ArcGIS 网络分析得到最短路径。
2. **结果展示**：`ShowPathResult(string message)` 将结果文字写入 `lblPathResult` 并显示 `pnlPathResult`，计时器控制显示时长（默 15 s）。
3. **悬浮窗定位**：`UpdatePathResultPanelLocation` 将悬浮窗锚定在地图右上角，随窗口大小变化自动调整。
4. **遗产点查询**：侧边栏 `FrmBuildingHeritageQuery` 加载 `heritages_cn` 数据，可在表格中勾选点并在地图上高亮显示。
5. **导出功能**：`ExportPathToShapefile` 将当前路径保存为 Shapefile，便于在 GIS 软件中进一步分析。

---

## 代码仓库 & 持续集成
项目已推送至 GitHub：
<https://github.com/CHENXIAO0111/AE-Development-based-in-Shanghai>

> **Tip**：在本地修改后，可使用以下命令同步到远程仓库：
> ```bash
> git add .
> git commit -m "更新 README 与文档"
> git push origin master
> ```

---

## 贡献指南
- Fork 本仓库，创建新分支进行开发。
- 保持代码风格统一（使用 Visual Studio 代码格式化功能）。
- 如新增功能，请同步更新 `README.md` 与 `API.md` 中的说明。
- 提交 Pull Request 前，请确保 **本地编译通过**，并通过所有已有的单元测试（若有）。

---

## 许可证
本项目采用 **MIT 许可证**，详见仓库根目录的 `LICENSE` 文件。

---

## 其他资源
- **API 文档**：[API.md](API.md)
- **示例截图**（可自行添加）：`assets/screenshot1.png`
- **常见问题**（FAQ）将在后续更新中补充。

---

*本 README 依据最新代码自动生成，若后续添加新文件或功能，请及时同步更新。*