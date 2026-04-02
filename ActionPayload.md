# ActionPayload Write/Read 규약

ActionPayload는 바이너리 버퍼 기반으로 동작합니다.
Dispatch 측에서 `Write` 한 순서 그대로 Handler 측에서 `Read` 해야 합니다.

```json
{
  "AtkDmg (50001)":          [ "ElementType elementType", "int damage" ],
  "AtkFixedDmg (50002)":     [ "ElementType elementType", "int damage" ],
  "AtkLossHp (50003)":       [ "int damage" ],
  "DmgAdjust (50004)":       [ "int maxDamage" ],
  "DmgRateAdjust (50005)":   [ "float maxDamageRate" ],

  "AddBlock (50101)":        [ "int block" ],
  "HealHp (50102)":          [ "int healPoint" ],
  "AddMaxHp (50103)":        [ "int maxHpPoint" ],
  "AddCost (50104)":         [ "int costPoint" ],
  "ChangeElement (50105)":   [ "ElementType elementType" ],
  "TakenDmgRateAdjust (50106)": [ "float damageRate" ],

  "ShuffleDeck (50200)":     [],
  "MoveCardFromDeck (50201)":    [ "Location to", "int cardCount" ],
  "MoveCardFromHand (50202)":    [ "Location to", "int cardCount" ],
  "MoveCardFromDiscard (50203)": [ "Location to", "int cardCount" ],
  "MoveCardFromExhaust (50204)": [ "Location to", "int cardCount" ],
  "SearchCard (50205)":      [ "int cardCount" ],
  "MoveSelectedCard (50206)": [],
  "DrawCount (50207)":       [ "int drawCount" ],

  "SetCardCost (50208)":     [ "int cardInstanceId", "int costPoint" ],
  "AddCardCost (50209)":     [ "int cardInstanceId", "int costPoint" ],
  "ReduceCardCost (50210)":  [ "int cardInstanceId", "int costPoint" ],
  "RandomizeCardCost (50211)": [ "int cardInstanceId", "int maxCostPoint" ],
  "ResetCardCost (50212)":   [ "int cardInstanceId" ],

  "CreateCard (50213)":      [ "Location to", "CardName cardName" ],
  "CopyCard (50214)":        [ "Location to", "CardName cardName" ],
  "TransformCard (50215)":   [ "int cardId", "CardName to" ],
  "ResetCardTransform (50216)": [ "int cardId" ],

  "GiveBuffDur (50301)":     [ "StatusEffectName effectName", "int duration" ],
  "GiveBuffSta (50302)":     [ "StatusEffectName effectName", "int stack" ],
  "RemoveBuffDur (50303)":   [ "StatusEffectName effectName", "int duration" ],
  "RemoveBuffSta (50304)":   [ "StatusEffectName effectName", "int stack" ],
  "ClearBuffs (50305)":      [ "StatusEffectName effectName" ],
  "CancelBuff (50306)":      [ "StatusEffectName before", "StatusEffectName after" ],
  "ActionSkip (50307)":      []
}
```
