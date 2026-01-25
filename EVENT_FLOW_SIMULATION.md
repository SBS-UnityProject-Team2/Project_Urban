# 🎮 이벤트 플로우 시뮬레이션

## 📋 시나리오 설정
- **현재 플레이어**: 체력 80/100, 골드 250, 덱: [파괴 1개], [사이킥 1개], [정화 2개] 포함
- **실행할 이벤트**: EventCode 10 "불길한 사내"
- **선택 예정**: 첫 번째 선택지 (ChoiceCode 1000) "왼쪽 손을 선택한다."

---

## 🚀 단계별 플로우

### **[STEP 1] 이벤트 시작 - EventManager.StartEvent(10)**

```
EventManager.StartEvent(eventCode: 10)
    ↓
EventScript.GetEvent(10) 호출
    ↓
EventInfo 반환:
{
  EventCode: 10,
  EventName: "불길한 사내",
  EventScript: 100,           ← NPC 대사 스크립트 ID
  EventChoice1: 1000,        ← 1번 선택지
  EventChoice2: 1001,        ← 2번 선택지
  EventChoice3: 1002         ← 3번 선택지
}
```

---

### **[STEP 2] UI 초기화 - EventUI.SetupUI()**

```
EventUI.SetupUI(dialogue, imgName)
    ↓
EventScript.GetScript(100) 호출
    ↓
ScriptInfo 반환:
{
  ScriptCode: 100,
  Dialogue: "\"\" 잠깐, 자네…… 나와 거래 하나 하지 않겠는가... \"\"",
  Illustration: "Npc"
}
    ↓
화면 설정:
├─ npcDialogueText.text = 
│  "\"\" 잠깐, 자네…… 나와 거래 하나 하지 않겠는가... \"\""
├─ Resources.Load<Sprite>("NPC/Npc") 시도
└─ illustrationImage.sprite = Npc 이미지 설정
```

**화면에 표시되는 모습:**
```
┌─────────────────────────────────┐
│  📷                             │
│  불길한 NPC 이미지               │
│                                 │
│  "잠깐, 자네……                 │
│   나와 거래 하나 하지 않겠는가..." │
│                                 │
│  [로브를 걸친 사람이 절뚝거리며   │
│   다가와 말을 걸어온다. ...]      │
└─────────────────────────────────┘
```

---

### **[STEP 3] 선택지 표시 - ShowChoices()**

```
ShowChoices(EventInfo eventInfo)
    ↓
3개의 선택지 순회: [1000, 1001, 1002]
    ↓
각 선택지마다:
```

#### **선택지 1번 (ChoiceCode: 1000)**
```
1️⃣ EventChoice.GetChoice(1000) 호출
    ↓
ChoiceInfo 반환:
{
  ChoiceCode: 1000,
  ChoiceName: "왼쪽 손을 선택한다.",
  ChoiceCondition: 0,           ← ConditionType.None
  ChoiceResult: "체력 [h] 피해\n카드 [소이탄], [전자기장], [개화] 중 1개 선택 획득",
  ResultCode: 10000,
  ScriptCode: 11000
}
    ↓
2️⃣ CheckCondition(ConditionType.None) 호출
    ↓
switch (ConditionType.None) → return true  // 조건 없음
    ↓
3️⃣ EventUI.CreateButton(choiceData, isSelectable: true, callback)
    ↓
버튼 생성:
┌────────────────────────────┐
│ 왼쪽 손을 선택한다.         │  ✅ 활성화 (흰색)
│ [체력 피해 / 카드 획득]      │  클릭 가능
└────────────────────────────┘
```

#### **선택지 2번 (ChoiceCode: 1001)**
```
EventChoice.GetChoice(1001) → ChoiceCondition: 0 (None)
    ↓
CheckCondition(ConditionType.None) → true
    ↓
버튼 생성:
┌────────────────────────────┐
│ 오른쪽 손을 선택한다.       │  ✅ 활성화 (흰색)
│ [체력 피해 / 카드 획득]      │  클릭 가능
└────────────────────────────┘
```

#### **선택지 3번 (ChoiceCode: 1002)**
```
EventChoice.GetChoice(1002) → ChoiceCondition: 0 (None)
    ↓
CheckCondition(ConditionType.None) → true
    ↓
버튼 생성:
┌────────────────────────────┐
│ 양쪽 손을 선택한다.         │  ✅ 활성화 (흰색)
│ [없음]                      │  클릭 가능
└────────────────────────────┘
```

**화면에 표시되는 모습:**
```
┌─────────────────────────────────┐
│  [불길한 NPC 이미지 & 대사]     │
│                                 │
│  버튼1: 왼쪽 손을 선택한다.      │ ← 마우스 오버
│  버튼2: 오른쪽 손을 선택한다.    │
│  버튼3: 양쪽 손을 선택한다.      │
└─────────────────────────────────┘
```

---

### **[STEP 4] 플레이어 선택 - OnChoiceSelected()**

```
플레이어가 버튼1 클릭
    ↓
EventButtonUI.onClickCallback 실행
    ↓
OnChoiceSelected(choiceData: ChoiceInfo{ChoiceCode: 1000, ...})
    ↓
모든 버튼 제거: eventUI.ClearButtons()
```

---

### **[STEP 5] 보상 데이터 조회**

```
var rewardData = eventResultSO.GetReward(10000)
    ↓
EventReward.json에서 ResultCode: 10000 검색
    ↓
ResultInfo 반환:
{
  ResultCode: 10000,
  ResultHpPresent: -0.1,       ← 현재 체력의 10% 감소
  ResultHpMaximum: 0,
  ResultGold: 0,               ← 골드 변화 없음
  ResultRandomCard: 0,         ← 랜덤 카드 없음
  ResultRangeCard: 1,          ← 범위 카드 1 (선택 카드)
  ResultRemove: 0              ← 카드 제거 없음
}
```

---

### **[STEP 6] 보상 적용 - ApplyRewardLogic()**

#### **6-1. HP 변화 계산**
```
플레이어 체력: 80/100

계산식:
val = (CurrentHp × ResultHpPresent) + (MaxHp × ResultHpMaximum)
val = (80 × -0.1) + (100 × 0)
val = -8 + 0
val = -8

amount = (int)-8 = -8
    ↓
amount < 0이므로 피해 적용
player.Health.DecreaseHp(Mathf.Abs(-8))
player.Health.DecreaseHp(8)
    ↓
✅ 플레이어 체력: 80 → 72/100
```

#### **6-2. 골드 변화**
```
ResultGold = 0 → 스킵 (변화 없음)
```

#### **6-3. 카드 보상 (구현 시나리오)**
```
ResultRangeCard = 1 (범위 카드 풀 1번)
    ↓
EventManager.ProcessCardSelection([소이탄, 전자기장, 개화])
    ↓
플레이어에게 3개 중 1개 선택하는 UI 표시
    ↓
플레이어가 "전자기장" 선택
    ↓
DeckManager.AddCard(Card: 전자기장)
    ↓
✅ 덱에 전자기장 추가됨
```

---

### **[STEP 7] 결과 스크립트 표시 - PlayTypeWriter()**

```
var resultScript = eventResultSO.GetScript(11000)
    ↓
EventResult.json에서 ScriptCode: 11000 검색
    ↓
ResultScriptInfo 반환:
{
  ScriptCode: 11000,
  ResultScript: "정체를 알 수 없는 힘에 공격당했다. 동시에 그 힘이 그대로 나에게 스며들었다.\n
                 그는 여전히 불길한 미소로 가볍게 손짓하고는 멀어졌다.",
  Dialogue: "\"\"응당 거래는 주고받는 것이지…\"",
  EndScript: ""
}
    ↓
eventUI.SetupUI(Dialogue, null)
    ↓
npcDialogueText.text = "\"\"응당 거래는 주고받는 것이지…\"\""
    ↓
eventUI.PlayTypeWriter(ResultScript, callback)
    ↓
타이핑 애니메이션 시작:
    ↓
"정체를 알 수 없는 힘에 공격당했다. 동시에 그 힘이 그대로 나에게 스며들었다.
그는 여전히 불길한 미소로 가볍게 손짓하고는 멀어졌다."

(글자가 하나씩 나타남... 타닥타닥타닥)
```

**화면에 표시되는 모습:**
```
┌─────────────────────────────────┐
│  [NPC 이미지]                    │
│  "응당 거래는 주고받는 것이지…" │
│                                 │
│  정체를 알 수 없는 힘에 공격당  │
│  했다. 동시에 그 힘이 그대로   │
│  나에게 스며들었다. 그는 여전  │
│  히 불길한 미소로 가볍게 손    │
│  짓하고는 멀어_ (계속 타이핑)   │
│                                 │
│  [마우스 클릭 대기]              │
└─────────────────────────────────┘
```

---

### **[STEP 8] 타이핑 완료 & 이벤트 종료**

```
플레이어가 마우스 클릭
    ↓
PlayTypeWriter() 콜백 실행
    ↓
StartCoroutine(EndEventDelay())
    ↓
0.2초 대기
    ↓
마우스 입력 대기
    ↓
플레이어가 다시 마우스 클릭
    ↓
eventPanel.SetActive(false)
    ↓
✅ 이벤트 종료, 게임 복귀
```

---

## 📊 최종 상태 정리

| 항목 | 변화 | 결과 |
|------|------|------|
| **체력** | 80/100 → 72/100 | ✅ 8 피해 |
| **골드** | 250 | ➡️ 변화 없음 |
| **덱** | [파괴, 사이킥, 정화x2] | ➕ 전자기장 추가 |
| **이벤트** | 진행 중 | ✅ 종료 |

---

## 🔄 조건부 선택 예시 (if ChoiceCondition ≠ 0)

만약 선택지 조건이 다르다면:

```
선택지: ChoiceCode 1009
├─ ChoiceName: "나서서 강신회의 편을 든다."
├─ ChoiceCondition: 3 (ConditionType.RequireBio)
└─ ChoiceResult: "[파괴] 속성 카드 1개 획득\n[사이킥] 속성 카드 1개 랜덤 제거"

CheckCondition(ConditionType.RequireBio) 호출
    ↓
DeckManager.CardList.Any(card => card.Element == Element.Bio)
    ↓
Bio 속성 카드 검색:
플레이어 덱: [파괴, 사이킥, 정화x2] 
    → Bio 속성 없음!
    ↓
return false
    ↓
버튼 상태:
┌────────────────────────────┐
│ 나서서 강신회의 편을 든다.  │  ❌ 비활성화 (회색)
│ [파괴 1개 획득 / 사이킥 1개│  클릭 불가능
│  제거]                      │
└────────────────────────────┘
```

---

## 🎯 핵심 데이터 흐름 다이어그램

```
JSON 파일들
    ↓
EventManager (중앙 컨트롤러)
    ├─ EventScript (대사/이미지 로드)
    ├─ EventChoice (선택지 데이터)
    ├─ CheckCondition (조건 검사)
    └─ EventResult (보상 정보)
        ↓
    ┌───────────────────────┐
    │   플레이어에 영향      │
    ├───────────────────────┤
    │ • HP 변화             │
    │ • 골드 변화           │
    │ • 덱 카드 추가/제거    │
    └───────────────────────┘
        ↓
    EventUI (시각적 표현)
    ├─ SetupUI (대사 & 이미지)
    ├─ PlayTypeWriter (타이핑 애니메이션)
    ├─ CreateButton (선택지 버튼)
    └─ ClearButtons (버튼 정리)
```

---

## ⚙️ 동시 실행 처리 흐름

```
Button Click
    ↓
┌─────────────────────────────────────────┐
│ 동시에 처리되는 작업들:                  │
├─────────────────────────────────────────┤
│ 1. HP 감소 + BattleManager 업데이트     │
│ 2. 카드 선택 UI 표시                    │
│ 3. 결과 대사 타이핑 애니메이션 시작     │
└─────────────────────────────────────────┘
    ↓
모든 작업 완료 대기
    ↓
이벤트 종료
```

---

## 💡 예상 실행 시간

| 단계 | 소요 시간 |
|------|---------|
| 데이터 로드 | ~50ms |
| UI 표시 | ~100ms |
| 선택지 버튼 생성 | ~150ms |
| 플레이어 대기 | ∞ (클릭 대기) |
| 보상 적용 | ~50ms |
| 타이핑 애니메이션 | ~2-3초 (속도별) |
| **총 이벤트 진행** | **~3-5초 (플레이어 입력 제외)** |
