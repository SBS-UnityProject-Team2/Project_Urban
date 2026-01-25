using System.Linq;

public enum ConditionType
{
    None = 0,           // 조건 없음
    RequireNone = 1,    // [None] 속성 카드 필요
    RequireRuin = 2,    // [Ruin] 파괴 속성 카드 필요
    RequirePsychic = 3, // [Psychic] 사이킥 속성 카드 필요
    RequireBio = 4      // [Bio] 생체 속성 카드 필요
}

/// <summary>
/// 이벤트 선택지 조건 체크 유틸리티
/// </summary>
public static class ConditionCheck
{
    /// <summary>
    /// 조건 충족 여부 확인 (덱에 필요한 속성 카드가 있는지)
    /// </summary>
    public static bool CheckCondition(ConditionType condition)
    {
        // 조건이 없으면 항상 활성화
        if (condition == ConditionType.None) 
            return true;

        // 조건 Enum → Element 변환
        Element requiredElement = condition switch
        {
            ConditionType.RequireNone => Element.None,
            ConditionType.RequireRuin => Element.Ruin,
            ConditionType.RequirePsychic => Element.Psychic,
            ConditionType.RequireBio => Element.Bio,
            _ => Element.None
        };

        // 덱에 필요한 속성 카드가 있는지 확인
        return DeckManager.Instance.CardList.Any(card => card.Element == requiredElement);
    }
}