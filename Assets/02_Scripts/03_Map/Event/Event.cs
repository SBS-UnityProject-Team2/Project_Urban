using System;

// 1. 이벤트 테이블 (EventTable.json)
[Serializable]
public class EventData
{
    public int EventCode;       // 이벤트 ID
    public int Stage;           // 등장 스테이지
    public string EventName;    // 이름
    public int EventScript;     // 상황 설명 스크립트 ID
    public int EventChoice1;    // 선택지 1 ID
    public int EventChoice2;    // 선택지 2 ID
    public int EventChoice3;    // 선택지 3 ID
}

// 2. 이벤트 스크립트 (EventScript.json)
[Serializable]
public class EventScriptData
{
    public int ScriptCode;
    public int EventCode;
    public string EventScript;  // 상황 설명
    public string Dialogue;     // NPC 대사
    public string Illustration; // 이미지 파일명
}

// 3. 선택지 (EventChoice.json)
[Serializable]
public class EventChoiceData
{
    public int ChoiceCode;
    public int CascadeEvent;
    public string ChoiceName;      // 선택지 버튼 텍스트
    public string ChoiceCondition; // 활성화 조건
    public string ChoiceResult;    // 예상 결과 텍스트 (UI 표시용)
    public int ResultCode;         // 결과 데이터 ID
    public int ScriptCode;         // 결과 스크립트 ID (ResultScriptTable 참조)
}

// 4. 결과 데이터 (EventReward.json / EventResultEfTable)
[Serializable]
public class EventResultData
{
    public int ResultCode;
    public float ResultHpPresent; // 현재 체력 비율 변화 (예: -0.1은 10% 감소)
    public float ResultHpMaximum; // 최대 체력 비율 변화
    public int ResultGold;        // 골드 변화 (정수)
    public int ResultRandomCard;  // 랜덤 카드 풀 ID
    public int ResultRangeCard;   // 범위 카드 풀 ID
    public int ResultRemove;      // 카드 제거 여부 (Enum)
}

// 5. 결과 스크립트 
[Serializable]
public class ResultScriptData
{
    public int ScriptCode;
    public string ResultScript; // 결과 설명
    public string Dialogue;     // 결과 NPC 대사
    public string EndScript;    // 최종 확인 텍스트
}