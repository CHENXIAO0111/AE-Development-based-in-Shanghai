# 上海市优秀建筑与不可移动文物游览路径规划系统

**AE-Development-based-in-Shanghai**

基于 ArcGIS Engine 10.8 开发的上海历史建筑与文化遗产路径规划与查询系统，为游客提供自助游览路线规划服务，为文化管理部门提供资源空间化管理工具。

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Platform](https://img.shields.io/badge/platform-Windows-green.svg)
![Framework](https://img.shields.io/badge/.NET-4.8-orange.svg)

---

## 📋 项目简介

上海作为历史文化名城，拥有 845 余处优秀历史建筑与不可移动文物。本项目通过 GIS 技术实现：

- 🗺️ **空间可视化**：地图展示遗产点分布
- 🔍 **多条件查询**：按名称、类别、区域筛选
- 🚗 **智能路径规划**：车行/人行双模式最短路径
- 📊 **结果展示**：半透明悬浮窗显示路径信息

---

## ✨ 功能特性

| 功能模块 | 说明 |
|----------|------|
| **矢量数据加载** | 支持 SHP 遗产点数据、Access MDB 路网数据 |
| **基础地图浏览** | 放大、缩小、平移、全图显示、刷新 |
| **图层控制** | 遗产点、路网图层显示/隐藏切换 |
| **多条件属性查询** | 按名称、类别、区域单项/联合查询 |
| **最短路径规划** | 支持起点/途经点/终点规划，高亮显示路径 |
| **结果展示** | 右上角悬浮窗显示路径长度(km)和预计时间(h:mm:ss) |
| **路径导出** | 支持导出路径为 Shapefile 格式 |

---

## 🖼️ 界面预览

```
┌─────────────────────────────────────────────────────────────────┐
│  文件  视图  地图工具  查询                                      │
├──────────┬──────────────────────────────────────────────────────┤
│          │                                                      │
│  图层    │                    地图显示区                        │
│  ─────   │                                                      │
│  ☑ 遗产点 │                                                      │
│  ☑ 路网   │              🏛️ 上海历史遗产分布                    │
│          │                                                      │
│  ─────   │                                                      │
│  路径规划 │                                                      │
│  起点: [▼]│                                                      │
│  途经点:[▼]│                                                      │
│  终点: [▼]│                                                      │
│  模式: [▼]│                                                      │
│  [开始规划]│                                                      │
│  [清空路径]│                                                      │
│          │                                                      │
└──────────┴──────────────────────────────────────────────────────┘
```

---

## 🛠️ 环境要求

| 项目 | 要求 |
|------|------|
| 操作系统 | Windows 10/11 (64位) |
| 开发工具 | Visual Studio 2019/2022 |
| 运行框架 | .NET Framework 4.8 |
| GIS 组件 | ArcGIS Engine 10.8 (需授权) |
| 数据格式 | SHP、Access MDB |

---

## 📁 项目结构

```
Route_of_Shanghai/
├── Route_of_Shanghai/
│   ├── Form1.cs / Form1.Designer.cs      # 主窗体，地图控制与路径规划
│   ├── FrmBuildingHeritageQuery.cs       # 外挂式查询窗口
│   ├── Program.cs                        # 程序入口
│   ├── API.md                            # 详细API技术文档
│   ├── README.md                         # 项目说明（本文件）
│   ├── heritages_cn.shp                  # 上海历史遗产点数据
│   ├── network_peo.mdb                   # 路网几何网络数据 ⚠️ 需单独放置
│   └── bin/Debug/                        # 编译输出目录
├── Route_of_Shanghai.sln                 # 解决方案文件
├── .gitignore                            # Git忽略配置
└── API文档.md                            # 完整技术文档
```

---

## 🚀 快速开始

### 1. 克隆仓库

```bash
git clone https://github.com/CHENXIAO0111/AE-Development-based-in-Shanghai.git
cd AE-Development-based-in-Shanghai/Route_of_Shanghai
```

### 2. 准备数据文件

⚠️ **重要**：由于 `network_peo.mdb` 文件超过 100MB，无法放入 Git 仓库，请确保：

1. 将 `network_peo.mdb` 文件复制到 `Route_of_Shanghai/` 目录下
2. 确保 `heritages_cn.shp` 及其配套文件在同一目录

### 3. 编译运行

1. 使用 Visual Studio 打开 `Route_of_Shanghai.sln`
2. 确保已安装 ArcGIS Engine 10.8 并授权
3. 按 `F5` 编译运行

---

## 📖 使用指南

### 基础地图操作

| 操作 | 方法 |
|------|------|
| 放大 | 点击工具栏【放大】按钮，在地图上框选区域 |
| 缩小 | 点击【缩小】按钮 |
| 平移 | 点击【平移】按钮，拖拽地图 |
| 全图 | 菜单【视图】→【全图显示】 |
| 刷新 | 菜单【视图】→【刷新地图】 |

### 文旅资源查询

1. 点击菜单【建筑或文物点查询】→【进入查询】
2. 输入查询条件（名称/类别/区域）
3. 点击查询按钮，结果在表格中显示
4. **双击表格行**，地图自动定位到该遗产点

### 路径规划

1. 在左侧面板选择：
   - **起点**：路径起始位置
   - **途经点（可选）**：中间经过点
   - **终点**：路径终点
2. 选择出行模式：**车行**（40 km/h）或 **人行**（5 km/h）
3. 点击【开始规划】
4. 地图显示红色高亮路径，右上角悬浮窗显示结果
5. 点击【清空规划路径】清除结果

---

## 📝 技术文档

- **[API.md](API.md)** - 类、方法、事件详细文档
- **[API文档.md](../API文档.md)** - 完整技术设计文档（概述、原理、架构、实现）

---

## ⚠️ 常见问题

| 问题 | 解决方法 |
|------|----------|
| 数据无法加载 | 检查 `network_peo.mdb` 和 `heritages_cn.shp` 是否在项目目录中 |
| 路径规划失败 | 确保 MDB 文件中的几何网络完整，选择的点在路网附近 |
| 定位后地图空白 | 确保代码中调用了 `ProjectPoint()` 进行坐标转换 |
| 程序闪退 | 检查 ArcGIS 授权是否有效 |

---

## 🤝 贡献指南

1. Fork 本仓库
2. 创建新分支 (`git checkout -b feature/xxx`)
3. 提交更改 (`git commit -am 'Add xxx'`)
4. 推送到分支 (`git push origin feature/xxx`)
5. 创建 Pull Request

**注意事项**：
- 保持代码风格统一
- 更新 `README.md` 和 `API.md` 文档
- 确保本地编译通过

---

## 📄 许可证

本项目采用 [MIT License](LICENSE) 开源许可。

---

## 🔗 相关链接

- **项目主页**：<https://github.com/CHENXIAO0111/AE-Development-based-in-Shanghai>
- **ArcGIS Engine 文档**：<https://developers.arcgis.com/>
- **ESRI 中国**：<https://www.esrichina.com.cn/>

---

## 👨‍💻 开发者

- 项目维护：[@CHENXIAO0111](https://github.com/CHENXIAO0111)
- 创建日期：2026年5月

---

*本 README 最后更新于 2026年5月25日*