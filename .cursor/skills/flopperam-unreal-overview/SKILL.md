---
name: flopperam-unreal-overview
description: >
  Flopperam Unreal MCP（flopperam-unreal）伺服器概覽——工具分類、
  工作流程與跨工具關係。首次連線 Unreal Editor、不確定該用哪個工具、
  或使用者詢問可用功能時使用。
---

# Flopperam Unreal MCP（flopperam-unreal）— 概覽

此 MCP 伺服器透過 WebSocket 控制**執行中的 Unreal Editor** 實例。所有工具都作用於正在執行的編輯器——若工具回傳連線錯誤，可能是編輯器未啟動，或 FlopAI 外掛未載入。

## 意圖分類 (Intent Classification)

呼叫任何工具之前，先判斷使用者需要什麼：

- **提問 / 說明** — 以知識回答即可。僅在需要具體資訊時使用唯讀工具。不要進行修改。
- **檢視 / 分析** — 使用唯讀工具並回報結果。不要修改任何內容。
- **修改 / 建立** — 依完整流程進行：觀察 → 執行 → 驗證。

意圖不明確時，預設以回答為主，而非直接修改。

## 工具分類 (Tool Categories)

### 場景與關卡 (Scene & Level)

- `scene_query` — 依 class/label/tag/folder 尋找 Actor，支援空間篩選（near/box/frustum）
- `scene_brief` — 精簡的關卡概覽
- `scene_compose` — 以宣告式方式生成/修改/刪除 Actor（prefab + pattern）
- `actor_inspect` — 深入檢視 `scene_query` 找到的單一 Actor
- `level_inspect` — 持久關卡、串流、World Partition、data layer
- `search_assets` — 依名稱/prefix/class 搜尋 Content Browser
- `asset_references` — 硬參考關係圖（X 引用了什麼，或誰引用了 X）
- `project_context` — 引擎版本、已啟用外掛、專案設定、地圖、內容統計

### Blueprint 唯讀 (Read-Only — All Tiers)

- `bp_brief` — 單次呼叫的精簡 BP 概覽。初次了解時**優先使用**。
- `bp_inspect` — 21 種目標查詢操作，可批次於單次呼叫
- `bp_export` — 完整 GraphSpec JSON 匯出，供複製或比對

### Blueprint 編寫 (Authoring — Paid Tier)

- `bp_create` — 建立 Actor/Pawn/Character/Component/Object/Interface/Enum/Struct BP
- `bp_class` — 更換父類、新增/移除介面、覆寫函式、設定 CDO 屬性
- `bp_variable` — 批次變數 CRUD，支援完整型別詞彙
- `bp_component` — 批次元件 CRUD，含內嵌屬性寫入
- `bp_graph` — 函式/macro/event graph CRUD
- `bp_nodes` — 批次新增/移除節點（約 40 種節點類型）
- `bp_wire` — 批次 exec + 資料連線、pin 預設值、pin 操作
- `bp_input` — Enhanced Input 套件（IA + IMC + 按鍵對應）
- `bp_commit` — 編譯 + 自動排版 + 健康報告
- `bp_author` — 一次性完整 GraphSpec（複合逃生口）
- `bp_dry_run` — 非平凡寫入前的能力探測
- `repair_blueprint` — 修復損壞的 BP

### 材質與著色 (Materials & Shading)

- `material_inspect` — 唯讀概覽、expression、連線、參數
- `material_edit` — 建立材質/實例/函式，編寫 expression graph

### 視覺特效 (VFX)

- `niagara_inspect` — 唯讀 Niagara 系統檢視
- `niagara_edit` — system/emitter/user-parameter/renderer 寫入
- `niagara_script_edit` — 可重用的 NiagaraScript module graph
- `chaos_edit` — Geometry Collection（破壞、碎裂、clustering）

### 動畫 (Animation)

- `animation_inspect` — 唯讀動畫檢視
- `animation_edit` — sequence/montage/notify/BlendSpace/socket 寫入
- `animation_graph_edit` — AnimBlueprint AnimGraph 編寫
- `ik_rig_edit` — IK Rig 鏈與 solver
- `ik_retarget` — 骨架鏈對應，供批次 retarget

### 介面元件 (UMG / Widgets)

- `widget_inspect` — 唯讀 UMG widget 樹檢視
- `widget_edit` — 樹/屬性/樣式/動畫/MVVM/事件寫入

### AI 與能力系統 (AI & Ability System)

- `behavior_tree` — BT、Blackboard、AI Controller、perception、NavMesh、EQS、Smart Object
- `gas_edit` — Gameplay Ability、Effect、Attribute Set
- `tag_registry_edit` — 專案級 Gameplay Tag 階層

### 地形與植被 (Landscape & Foliage)

- `landscape_inspect` — 高度/坡度/法線查詢、統計
- `landscape_edit` — 雕刻、語意特徵、繪製圖層、heightmap 匯入/匯出
- `foliage_inspect` — 類型與數量列表
- `foliage_edit` — 散布、精確放置 transform、依半徑/類型移除

### 電影與音訊 (Cinematics & Audio)

- `sequencer_edit` — Level Sequence、Actor 綁定、track、關鍵影格、鏡頭切換
- `metasound_edit` — 程序化音訊（MetaSound graph）
- `sound_asset_edit` — SoundCue graph、SoundClass、Attenuation

### 程序化 (Procedural)

- `pcg_graph_edit` — PCG graph（generator、sampler、filter、mesh spawner）

### 資料資產 (Data Assets)

- `asset_factory` — enum/struct/DataTable/DataAsset/Enhanced Input

### 其他 (Other)

- `ramp_authoring` — 曲線資產（UCurveFloat、UCurveVector、UCurveLinearColor）
- `collision_profile_edit` — 碰撞設定檔、物理材質、物件類型回應

### 編輯器與診斷 (Editor & Diagnostics)

- `editor_actions` — 儲存、復原、重做、選取、建置光照、編譯、PIE 啟動/停止
- `editor_log` — Output Log + Message Log（含篩選）
- `performance_audit` — 三角面數、Actor 數量、貼圖記憶體、最佳化警告
- `window_capture` — 視埠截圖（PNG）
- `cpp_source` — 讀寫/搜尋 C++ 原始碼、Live Coding 熱重載

### 執行期驗證 (Runtime Verification)

- `pie_test_bp` — 以 Blueprint 為主的 PIE 測試框架，30+ 種斷言（付費）
- `pie_test_scene` — 以場景為主的 PIE 測試框架（免費）

### 執行 (Execution — Paid Tier)

- `python_execution` — 在編輯器程序內執行任意 Python（最後手段）
- `unreal_api` — 15,000+ UE API 簽名查詢
- `skills` / `bp_skills` — 按需載入工作流程文件

## 核心工作流程 (Core Workflow)

1. **定向** — `bp_brief`、`scene_brief`、`search_assets` 或 `project_context`
2. **執行** — 使用對應領域的工具
3. **驗證** — 以 inspect 工具重新讀取；執行期行為則跑 PIE 測試

## 效率規則 (Efficiency Rules)

- **全部批次化** — 每個接受陣列參數的工具（`queries`、`variables`、`components`、`nodes`、`edges`、`operations`）都設計為一次呼叫處理多個項目
- **獨立工具平行呼叫** — 若兩者都需要，不必等 `scene_query` 完成再呼叫 `search_assets`
- **使用篩選** — `bp_inspect`、`material_inspect`、`widget_inspect`、`animation_inspect` 皆支援 compact 模式與篩選
- **延遲編譯** — 窄範圍 bp_* 寫入會延遲編譯；先批次處理多項，再呼叫一次 `bp_commit`

## 限制 (Constraints)

- 回應上限為 512KB — 請使用篩選與目標查詢
- `search_assets` 回傳 Content Browser 路徑，格式為 `PackageName.AssetName` — 一律使用回傳的**完整**路徑
- `scene_query` 回傳的是 Actor 標籤 — 這些**不是**資產路徑
- 猜測的 `/Game/...` 路徑會靜默解析為 nullptr — 一律先透過 `search_assets` 解析
