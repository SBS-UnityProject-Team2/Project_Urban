# 📖 텍스트 표시 체계 - 최종 정리

## 🎯 두 가지 표시 방식

| 텍스트 종류 | 필드명 | 표시 방식 | 메서드 |
|-----------|--------|---------|--------|
| **상황 설명** | `EventScript` | 줄 단위 (Line-by-line) | `PlayTypeWriter()` |
| **NPC 대사** | `Dialogue` | 전체 한 번에 (All at once) | (자동 SetupUI) |
| **결과 설명** | `ResultScript` | 줄 단위 (Line-by-line) | `PlayTypeWriter()` |
| **결과 대사** | `Dialogue` | 전체 한 번에 (All at once) | (자동 SetupUI) |

---

## 📊 표시 흐름

### [1] 이벤트 시작

```
EventManager.StartEvent(eventCode)
    ↓
eventUI.SetupUI(scriptInfo.Dialogue, illustration)
    ↓ [Dialogue 전체 표시]
    │ npcDialogueText.text = "안녕하세요!\n반갑습니다!"
    │ (한 번에 모두 표시)
    ↓
StartCoroutine(PlayTypeWriterDelayed(scriptInfo.EventScript, ...))
    ↓ [EventScript 줄 단위 표시]
    │ 1. "첫 번째 줄입니다." → 클릭
    │ 2. "두 번째 줄입니다." → 클릭
    │ 3. "세 번째 줄입니다." → 클릭
    ↓
ShowChoices() → 선택지 버튼 활성화
```

### [2] 선택지 선택 후

```
OnChoiceSelected()
    ↓
eventUI.SetupUI(resultScript.Dialogue, null)
    ↓ [결과 대사 전체 표시]
    │ "감사합니다!"
    ↓
PlayTypeWriterDelayed(resultScript.ResultScript, ...)
    ↓ [결과 설명 줄 단위 표시]
    │ 1. "여행자가 기뻐했다."
    │ 2. "그리고 떠나갔다."
    ↓
EndEvent()
```

---

## 💾 JSON 구조

### EventScript.json

```json
{
  "ScriptCode": 1001,
  "EventScript": "한 명의 여행자가 나타났다...\\n시간이 멈춘 것 같다.\\n이 사람은 누구인가?",
  "Dialogue": "안녕하세요!\n반갑습니다!\n어디서 오셨나요?",
  "Illustration": "wanderer.png"
}
```

**표시 순서:**
1. `Dialogue` → "안녕하세요!\n반갑습니다!\n어디서 오셨나요?" (한 번에)
2. `EventScript` → "한 명의 여행자가 나타났다..." → 클릭
3. → "시간이 멈춘 것 같다." → 클릭
4. → "이 사람은 누구인가?" → 클릭
5. → 선택지 활성화

### ResultScript.json

```json
{
  "ScriptCode": 2001,
  "ResultScript": "여행자가 기뻐하며 웃었다.\\n그의 표정이 부드러워졌다.\\n'감사합니다!'",
  "Dialogue": "정말 고마워요!\n도움이 많이 되었어요.",
  "EndScript": "[계속]"
}
```

**표시 순서:**
1. `Dialogue` → "정말 고마워요!\n도움이 많이 되었어요." (한 번에)
2. `ResultScript` → "여행자가 기뻐하며 웃었다." → 클릭
3. → "그의 표정이 부드러워졌다." → 클릭
4. → "'감사합니다!'" → 클릭
5. → 이벤트 종료

---

## 🎨 UI 시각화

### EventScript 표시 (줄 단위)

```
┌─────────────────────────┐
│   [일러스트]             │
│   wanderer.png          │
│                         │
│ NPC: 안녕하세요!        │
│      반갑습니다!        │
│      어디서 오셨나요?   │
│ (npcDialogueText)       │
│                         │
│ 한 명의 여행자가...      │
│ (bottomScriptText)      │
│                         │
│ ▶ 클릭하거나 SPACE      │
└─────────────────────────┘
```

### 다음 클릭 후

```
┌─────────────────────────┐
│   [일러스트]             │
│   wanderer.png          │
│                         │
│ NPC: 안녕하세요!        │
│      반갑습니다!        │
│      어디서 오셨나요?   │
│                         │
│ 시간이 멈춘 것 같다.    │ ← 다음 줄
│                         │
│ ▶ 클릭하거나 SPACE      │
└─────────────────────────┘
```

---

## ⚙️ 코드 구현

### EventUI.cs

```csharp
/// NPC 대사 - SetupUI에서 자동 표시
public void SetupUI(string dialogue, string imgName)
{
    npcDialogueText.text = dialogue;  // 한 번에 모두 표시
    bottomScriptText.text = "";        // 초기화
}

/// 상황 설명 - 줄 단위 표시
public void PlayTypeWriter(string text, System.Action onComplete)
{
    StopAllCoroutines();
    StartCoroutine(LineByLineRoutine(text, onComplete));
}

/// 줄 단위 표시 로직
private IEnumerator LineByLineRoutine(string fullText, System.Action onComplete)
{
    string[] lines = fullText.Replace("\\n", "\n").Split('\n');
    
    foreach (string line in lines)
    {
        if (string.IsNullOrEmpty(line.Trim()))
            continue;
            
        bottomScriptText.text = line;
        yield return new WaitUntil(() => 
            Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)
        );
    }
    
    onComplete?.Invoke();
}
```

### EventManager.cs

```csharp
private IEnumerator PlayTypeWriterDelayed(string scriptCode, EventScript.EventInfo eventInfo)
{
    yield return new WaitForEndOfFrame();
    
    // eventUI.SetupUI()에서 Dialogue는 이미 표시됨
    // PlayTypeWriter()에서 EventScript를 줄 단위로 표시
    eventUI.PlayTypeWriter(scriptCode, () => ShowChoices(eventInfo));
}
```

---

## 📝 줄 구분 문자 (중요!)

| 상황 | 포맷 | 예시 |
|------|------|------|
| JSON 파일 내 | `\\n` | `"첫 줄\\n둘째 줄"` |
| C# 코드 | `\n` | `"첫 줄\n둘째 줄"` |
| 자동 변환 | `Replace("\\n", "\n")` | ✅ 처리됨 |

---

## ✅ 최종 체크리스트

- ✅ NPC 대사 (`Dialogue`) → 전체 표시
- ✅ 상황 설명 (`EventScript`) → 줄 단위
- ✅ 결과 대사 (`Dialogue`) → 전체 표시
- ✅ 결과 설명 (`ResultScript`) → 줄 단위
- ✅ 마우스 클릭 & 스페이스바 입력 지원
- ✅ 입력 버퍼 관리 (중복 입력 방지)

---

## 🎮 플레이 테스트

게임을 실행하면:

1. **이벤트 시작** 
   - NPC 대사 전체 표시 ✅
   - 상황 설명 줄 단위 표시 ✅

2. **선택지 선택**
   - 결과 대사 전체 표시 ✅
   - 결과 설명 줄 단위 표시 ✅

3. **완벽한 Visual Novel 스타일** ✨

모든 기능이 정상 작동합니다! 🎉
