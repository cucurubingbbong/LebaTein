# LebaTein

> **적에게 지워지는 전장을 다시 칠하며 버티는 Tower Defense**

[상세 기획서](https://docs.google.com/document/d/1NOhl0Zz-ZHif_uDQGmxbX08JtxK7_TpaP-FgKAFrGdU/edit)

---

## 핵심 규칙

전장의 모든 Tile은 **Color + Saturation**을 가진다.

적이 Tile을 지나가거나 공격하면 Saturation이 감소하고,  
0이 되면 **Blank Tile**이 된다.

- Blank Tile은 기능 정지
- 위에 있는 Tower도 비활성화
- 복구하면 Tower도 다시 작동

```text
Color Tile
   ↓ Enemy
Damaged Tile
   ↓ Saturation 0
Blank Tile
```

---

## Pigment & Brush

적을 처치하면 해당 색의 **Pigment**가 남는다.

```text
Red Enemy    → Red Pigment
Blue Enemy   → Blue Pigment
Purple Enemy → Purple Pigment
```

회수한 Pigment는 Palette에 저장되고  
Brush로 전장에 다시 사용할 수 있다.

### Brush

- Blank Tile 복구
- 손상된 Tile의 Saturation 회복
- Tile 재도색
- Tower 속성 변경

Tower는 **설치된 Tile의 Color를 그대로 상속**한다.

따라서 Tile을 다시 칠하는 행동 하나가  
**수리 + 속성 변경 + 다음 적 대응**을 동시에 담당한다.

---

## Color Combat

| Color | Strong Against |
| --- | --- |
| Red | Green |
| Green | Red |
| Orange | Blue |
| Blue | Orange |
| Yellow | Purple |
| Purple | Yellow |

- 보색: ×1.5
- 일반: ×1.0
- 동일색: ×0.6

새 Tower를 계속 늘리는 대신  
**기존 전장을 어떤 색으로 유지할지**가 핵심 선택이 된다.

---

## Tower

Tower는 3종만 사용한다.

- **Needle** — Single Target
- **Brush** — Area Attack
- **Prism** — Directional Attack

활성 Tower 수에는 제한이 있으며  
Tower 성장보다 **배치와 재도색**에 집중한다.

---

## Game Loop

```text
Wave Preview
     ↓
Enemy Color 확인
     ↓
Tile 도색 / Tower 배치
     ↓
Wave Start
     ↓
Tile 손상 / Blank 발생
     ↓
Enemy 처치 → Pigment
     ↓
복구 / 재도색
     ↓
Next Wave
```

플레이어가 지키는 것은 Base만이 아니라  
**전장 그 자체**다.

---

## Development

### 현재 구현

- Grid Build System
- BuildCommand / Ghost Preview
- Circle / Box / Cone Range
- Single / Area Attack
- Enemy Object Pool
- Enemy Registry
- Tower Attack Scheduler
- ScriptableObject TowerData

### 3주 개선

- Tile Saturation / Blank State
- Pigment Palette
- Brush Repaint
- Tower Color Inheritance
- Enemy Movement / Tile Attack
- Wave System
- EnemySpatialGrid
- Scheduler 개선
- Profiler Before / After

---

## Tech Goal

```text
Enemy → Tile Damage
  ↓
Pigment
  ↓
Palette → Brush → TileState
                    ↓
             Tower Color / Active

Tower → Scheduler → Spatial Grid → Enemy
```

최종 목표는 **1 Stage / 10 Wave**를 완성하고,  
색상 시스템과 대규모 Enemy 처리 구조를 포트폴리오에서 명확하게 보여주는 것이다.
