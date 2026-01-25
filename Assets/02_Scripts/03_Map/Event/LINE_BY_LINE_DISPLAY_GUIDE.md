# 📝 줄 단위 텍스트 표시 시스템 (Line-by-Line Display)

## 🎯 개요

기존의 **전체 문장 한 번에 표시** → **줄 구분에 따라 1줄씩 순차 표시**로 변경되었습니다.

**Visual Novel 스타일 대사 표시:**
- ✅ 한 번에 1줄만 표시
- ✅ 마우스 좌클릭 또는 스페이스바로 다음 줄 진행
- ✅ 모든 줄 출력 후 선택지 버튼 활성화

---

## 📊 동작 흐름

```
EventManager.StartEvent(eventCode)
    ↓
eventUI.PlayTypeWriter(script, callback)
    ↓
LineByLineRoutine() 실행
    ↓
[1단계] "첫 번째 줄 표시"
    ↓ (마우스 클릭 또는 Space)
[2단계] "두 번째 줄 표시"
    ↓ (마우스 클릭 또는 Space)
[3단계] "세 번째 줄 표시"
    ↓ (마우스 클릭 또는 Space)
[완료] ShowChoices() 또는 EndEvent() 콜백 실행
```

---

## 🔧 구현 상세

### 1. 텍스트 분할 로직

```csharp
// 입력: "첫 번째 문장\n두 번째 문장\n세 번째 문장"
string processedText = fullText.Replace("\\n", "\n");
string[] lines = processedText.Split('\n');
// 결과: ["첫 번째 문장", "두 번째 문장", "세 번째 문장"]
```

**주의:** JSON에서 줄 구분은 `\\n` (이스케이프)로 저장됨

### 2. 줄 단위 표시

```csharp
for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
{
    string currentLine = lines[lineIndex];
    
    // 빈 줄 스킵 (자동)
    if (string.IsNullOrEmpty(currentLine.Trim()))
        continue;

    // UI에 표시
    bottomScriptText.text = currentLine;
    
    // 사용자 입력 대기 (마우스 또는 스페이스)
    yield return new WaitUntil(() => 
        Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)
    );
}
```

### 3. 입력 처리

- **마우스 좌클릭:** `Input.GetMouseButtonDown(0)`
- **스페이스바:** `Input.GetKeyDown(KeyCode.Space)`
- 둘 중 하나가 입력되면 다음 줄로 진행

---

## 📋 사용 예시

### EventScript.json에서 줄 구분

```json
{
  "ScriptCode": 1001,
  "EventScript": "한 명의 여행자가 나타났다...",
  "Dialogue": "안녕하세요!\n반갑습니다.\n어디서 오셨나요?",
  "Illustration": "wanderer.png"
}
```

**표시 순서:**
1. 클릭 → "안녕하세요!"
2. 클릭 → "반갑습니다."
3. 클릭 → "어디서 오셨나요?"
4. 클릭 → 선택지 버튼 활성화

---

## 🎨 UI 시각화

```
┌─────────────────────────────────────┐
│          이벤트 화면                 │
├─────────────────────────────────────┤
│  [일러스트]      │  안녕하세요!      │
│  wanderer.png    │  (클릭 대기)      │
│                  │                  │
│                  │ ▶ 클릭하거나     │
│                  │   스페이스바      │
├─────────────────────────────────────┤
│ [선택지 버튼들 - 아직 비활성]        │
└─────────────────────────────────────┘
```

---

## ⚙️ 메모리 최적화

**개선사항:**
- ✅ `WaitForSeconds` 캐싱 (이미 구현됨)
- ✅ 코루틴 정리: `StopAllCoroutines()` (중복 실행 방지)
- ✅ 빈 줄 자동 스킵 (불필요한 대기 제거)
- ✅ 입력 버퍼 관리: `yield return null` (중복 입력 방지)

---

## 🐛 주의사항

### 1. 줄 구분 문자

| 상황 | 형식 | 예시 |
|------|------|------|
| JSON 파일 | `\\n` | `"대사\\n다음대사"` |
| C# 코드 | `\n` | `"대사\n다음대사"` |
| 자동 처리 | 둘 다 | `Replace("\\n", "\n")` |

### 2. 빈 줄 처리

```csharp
// 자동으로 빈 줄 스킵 처리됨
if (string.IsNullOrEmpty(currentLine.Trim()))
    continue;
```

따라서 JSON에서 `"대사1\\n\\n대사2"`처럼 빈 줄을 넣어도 안전합니다.

### 3. 입력 버퍼 관리

```csharp
// 줄마다 입력 버퍼 초기화
yield return null;  // 1프레임 대기
```

이를 통해 한 번의 클릭으로 여러 줄이 한 번에 진행되는 것을 방지합니다.

---

## 🔄 콜백 메커니즘

```csharp
// 모든 줄 표시 후 실행
onComplete?.Invoke();
```

**EventManager에서의 사용:**

```csharp
// 선택지 표시 전 스크립트 표시
eventUI.PlayTypeWriter(scriptInfo.EventScript, 
    () => ShowChoices(eventInfo));

// 이벤트 종료 전 결과 스크립트 표시
eventUI.PlayTypeWriter(resultScript.ResultScript, 
    () => StartCoroutine(EndEventDelay()));
```

---

## 📝 결과

이제 게임 대사가 다음과 같이 표시됩니다:

```
[화면 1] "안녕하세요!" (사용자 클릭 대기)
    ↓ 클릭
[화면 2] "반갑습니다." (사용자 클릭 대기)
    ↓ 클릭
[화면 3] "어디서 오셨나요?" (사용자 클릭 대기)
    ↓ 클릭
[화면 4] 선택지 버튼 3개 활성화
```

더 몰입감 있는 Visual Novel 스타일의 게임 경험을 제공합니다! ✨
