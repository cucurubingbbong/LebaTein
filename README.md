# LebaTein

> **색을 지키고, 빼앗고, 다시 칠하는 타워 디펜스.**

LebaTein은 일반적인 골드 기반 타워 디펜스가 아니라,  
**타일의 색과 상태 자체를 자원으로 사용하는 전략 디펜스**를 목표로 한다.

📄 [상세 기획서](https://docs.google.com/document/d/1NOhl0Zz-ZHif_uDQGmxbX08JtxK7_TpaP-FgKAFrGdU/edit)

---

## Core Idea

전장은 여러 색의 타일로 구성된다.

적이 타일을 지나가거나 공격하면 타일의 **Saturation(채도)** 이 감소한다.

```text
Colored Tile
    ↓ Enemy Attack
Damaged Tile
    ↓ Saturation 0
Blank Tile
```

Blank 상태가 된 타일은 기능을 잃으며,
그 위의 타워도 함께 비활성화된다.

플레이어는 적을 처치해 남은 **Pigment**를 회수하고,
Brush를 이용해 전장을 다시 칠한다.

---

## Paint System

### Pigment

적을 처치하면 해당 적의 색에 맞는 Pigment가 남는다.

Pigment는 범용 화폐가 아니다.

- Red Enemy → Red Pigment
- Blue Enemy → Blue Pigment
- Purple Enemy → Purple Pigment

획득한 색은 Palette에 저장된다.

### Brush

Brush는 전투 중 타일에 직접 사용한다.

- Blank Tile 복구
- 손상된 Tile 채도 회복
- Tile 색상 변경
- Tower 색상 변경

타워는 자신이 설치된 Tile의 색을 따라간다.

즉, 타일을 다시 칠하는 행동이
**수리 + 속성 변경 + 전투 대응**을 동시에 담당한다.

---

## Combat

색상 상성은 보색 관계를 사용한다.

| Color | Advantage |
| --- | --- |
| Red | Green |
| Green | Red |
| Orange | Blue |
| Blue | Orange |
| Yellow | Purple |
| Purple | Yellow |

- 보색 공격: ×1.5
- 일반 공격: ×1.0
- 동일 색상: ×0.6

적 구성에 따라 타워를 새로 구매하는 대신,
**전장을 다시 칠해 기존 타워의 속성을 바꾸는 것**이 핵심이다.

---

## Tower

3종만 제작한다.

- **Needle** — 단일 대상
- **Brush** — 광역 공격
- **Prism** — 방향성 범위 공격

타워 수는 제한하고,
업그레이드보다 **배치 / 색상 / 타일 유지**에 집중한다.

---

## Game Loop

```text
Wave Preview
      ↓
Enemy Color 확인
      ↓
Tile / Tower 색상 구성
      ↓
Wave Start
      ↓
적이 Tile 오염 및 파괴
      ↓
Enemy 처치 → Pigment 획득
      ↓
Brush로 복구 / 재도색
      ↓
다음 Wave
```

Base HP만 지키는 것이 아니라,
**전장 자체가 계속 망가지는 상황을 관리하는 것**이 핵심 플레이가 된다.

---

## Tech

현재 구현되어 있는 기반:

- Grid Build System
- BuildCommand
- Ghost Preview
- Tower Range — Circle / Box / Cone
- Single / Area Attack
- Enemy Object Pool
- Enemy Registry
- Tower Attack Scheduler
- ScriptableObject 기반 TowerData

3주 개선 목표:

- Tile Saturation / Blank State
- Pigment Palette
- Brush Repaint System
- Tower Color Inheritance
- Wave System
- Enemy Movement / Tile Attack
- EnemySpatialGrid
- Scheduler 시간 처리 개선
- Profiler Before / After

---

## Architecture

```text
Enemy
  ├─ Tile Damage
  └─ Death
       ↓
    Pigment
       ↓
    Palette
       ↓
     Brush
       ↓
TileState ──→ Tower Color / Active State
       ↑
 Build System

Tower
  ↓
Attack Scheduler
  ↓
Spatial Grid
  ↓
Enemy
```

---

## Portfolio Goal

3주 동안 기능 수를 늘리는 것보다 아래 세 가지를 확실히 완성한다.

1. **색을 자원으로 사용하는 독특한 게임 루프**
2. **Pool / Scheduler / Spatial Partitioning 기반 대규모 적 처리**
3. **처음부터 Result까지 플레이 가능한 1 Stage / 10 Wave**

완성 후 Gameplay GIF, Architecture Diagram, Profiler 결과와 함께 정리할 예정이다.
