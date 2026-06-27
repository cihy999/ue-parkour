---
name: flopperam-unreal-overview
description: >
  Flopperam Unreal MCP（flopperam-unreal）伺服器概覽——工具分類、
  工作流程與跨工具關係。首次連線 Unreal Editor、不確定該用哪個工具、
  或使用者詢問可用功能時使用。
---

# Flopperam Unreal MCP（flopperam-unreal）— Overview

This MCP server controls a **live Unreal Editor** instance over WebSocket. Every tool operates on the running editor — if a tool returns a connection error, the editor may not be running or the FlopAI plugin is not loaded.

## Intent Classification

Before calling any tool, classify what the user needs:

- **Question / explanation** — Answer from knowledge. Use read-only tools only if specifics are needed. Do NOT make changes.
- **Inspection / analysis** — Use read-only tools and report findings. Do NOT modify anything.
- **Modification / creation** — Proceed with the full observe → execute → verify workflow.

When intent is unclear, default to answering rather than modifying.

## Tool Categories

### Scene & Level
- `scene_query` — find actors by class/label/tag/folder with spatial filters (near/box/frustum)
- `scene_brief` — compact level overview
- `scene_compose` — declaratively spawn/modify/delete actors with prefabs + patterns
- `actor_inspect` — drill into one actor found via scene_query
- `level_inspect` — persistent level + streaming + World Partition + data layers
- `search_assets` — search Content Browser by name/prefix/class
- `asset_references` — hard-reference graphs (what does X pull in, or what uses X)
- `project_context` — engine version, enabled plugins, project settings, maps, content stats

### Blueprint (Read-Only — All Tiers)
- `bp_brief` — compact one-call BP overview. Use FIRST for orientation.
- `bp_inspect` — 21 targeted query operations batched in one call
- `bp_export` — full GraphSpec JSON dump for cloning/diffing

### Blueprint Authoring (Paid Tier)
- `bp_create` — create Actor/Pawn/Character/Component/Object/Interface/Enum/Struct BPs
- `bp_class` — reparent, add/remove interfaces, override functions, set CDO properties
- `bp_variable` — batched variable CRUD with full type vocabulary
- `bp_component` — batched component CRUD with inline property writes
- `bp_graph` — function/macro/event graph CRUD
- `bp_nodes` — batched node add/remove (~40 node types)
- `bp_wire` — batched exec + data edges + pin defaults + pin ops
- `bp_input` — Enhanced Input bundle (IA + IMC + key mappings)
- `bp_commit` — compile + auto-layout + health report
- `bp_author` — full GraphSpec one-shot (compound escape hatch)
- `bp_dry_run` — capability probe before non-trivial writes
- `repair_blueprint` — rescue corrupted BPs

### Materials & Shading
- `material_inspect` — read-only overview, expressions, connections, parameters
- `material_edit` — create materials/instances/functions, author expression graphs

### VFX
- `niagara_inspect` — read-only Niagara system inspection
- `niagara_edit` — system/emitter/user-parameter/renderer writes
- `niagara_script_edit` — reusable NiagaraScript module graphs
- `chaos_edit` — Geometry Collections (destruction, fracture, clustering)

### Animation
- `animation_inspect` — read-only animation inspection
- `animation_edit` — sequence/montage/notify/BlendSpace/socket writes
- `animation_graph_edit` — AnimBlueprint AnimGraph authoring
- `ik_rig_edit` — IK Rig chains + solvers
- `ik_retarget` — skeleton chain mapping for batch retargeting

### UMG / Widgets
- `widget_inspect` — read-only UMG widget tree inspection
- `widget_edit` — tree/property/style/animation/MVVM/event writes

### AI & Ability System
- `behavior_tree` — BTs, Blackboards, AI Controllers, perception, NavMesh, EQS, Smart Objects
- `gas_edit` — Gameplay Abilities, Effects, Attribute Sets
- `tag_registry_edit` — project-wide Gameplay Tag hierarchy

### Landscape & Foliage
- `landscape_inspect` — height/slope/normal queries, statistics
- `landscape_edit` — sculpting, semantic features, paint layers, heightmap import/export
- `foliage_inspect` — type + count listing
- `foliage_edit` — scatter, place exact transforms, remove by radius/type

### Cinematics & Audio
- `sequencer_edit` — Level Sequences with actor bindings, tracks, keyframes, camera cuts
- `metasound_edit` — procedural audio (MetaSound graphs)
- `sound_asset_edit` — SoundCue graphs, SoundClass, Attenuation

### Procedural
- `pcg_graph_edit` — PCG graphs with generators, samplers, filters, mesh spawners

### Data Assets
- `asset_factory` — enums/structs/DataTables/DataAssets/Enhanced Input

### Other
- `ramp_authoring` — curve assets (UCurveFloat, UCurveVector, UCurveLinearColor)
- `collision_profile_edit` — collision profiles, physical materials, object type responses

### Editor & Diagnostics
- `editor_actions` — save, undo, redo, select, build lighting, compile, PIE start/stop
- `editor_log` — Output Log + Message Log with filters
- `performance_audit` — triangle counts, actor counts, texture memory, optimization warnings
- `window_capture` — viewport screenshot as PNG
- `cpp_source` — read/write/search C++ source, Live Coding hot-reload

### Runtime Verification
- `pie_test_bp` — Blueprint-focused PIE harness with 30+ assertion types (paid)
- `pie_test_scene` — scene-focused PIE harness (free)

### Execution (Paid Tier)
- `python_execution` — arbitrary Python inside the editor process (last resort)
- `unreal_api` — 15,000+ UE API signature lookups
- `skills` / `bp_skills` — load workflow documentation on demand

## Core Workflow

1. **Orient** — `bp_brief`, `scene_brief`, `search_assets`, or `project_context`
2. **Execute** — use the appropriate domain tools
3. **Verify** — re-read with inspect tools, run PIE test for runtime behavior

## Efficiency Rules

- **Batch everything** — every tool that accepts arrays (`queries`, `variables`, `components`, `nodes`, `edges`, `operations`) is designed for one call with many items
- **Call independent tools in parallel** — don't wait for scene_query before calling search_assets if both are needed
- **Use filters** — `bp_inspect`, `material_inspect`, `widget_inspect`, `animation_inspect` all support compact mode + filters
- **Defer compile** — narrow bp_* writes defer compile; batch many, then `bp_commit` once

## Constraints

- Responses are capped at 512KB — use filters and targeted queries
- `search_assets` returns Content Browser paths in `PackageName.AssetName` format — always use the FULL path as returned
- `scene_query` returns actor labels — these are NOT asset paths
- Guessed `/Game/...` paths silently resolve to nullptr — always resolve via `search_assets` first
