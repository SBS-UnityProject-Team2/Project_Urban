using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using System.Threading.Tasks;

public class CardAction : MonoBehaviour
{
    private List<ActionDataEntry> actionData;
    private readonly ActionPayload payload = new();

    public void Init(int linkId)
    {
        actionData = CardManager.Instance.GetActionData(linkId);
    }
    
    public IEnumerable Execute(Actor selectedActor)
    {
        foreach (ActionDataEntry action in actionData)
        {
            payload.Init();
            payload.actionId = action.actId;
            payload.source = Battle.Instance.Player;

            // 타겟 지정 로직
            switch (action.actTarget)
            {
                case Target.TarSelf:
                    payload.targets.Add(payload.source);
                    break;

                case Target.TarEnemy:
                    payload.targets.Add(selectedActor);
                    break;

                case Target.TarEnemyRandom:
                case Target.TarEnemyRandomEach:
                    {
                        int count = Battle.Instance.Monsters.Count;
                        payload.targets.Add(Battle.Instance.Monsters[Random.Range(0, count)]);
                        break;
                    }

                case Target.TarEnemiesAll:
                    payload.targets.AddRange(Battle.Instance.Monsters);
                    break;


                // 적 위치 관련된 부분은 몬스터 관리자 작성 후 추가하기
                case Target.TarAdjacentEnemies:
                    {
                        payload.targets.Add(selectedActor);


                        break;
                    }
            }

            payload.Write(action.actValue);
            // actParam처리하기

            // 아래 두줄은 루프할 수 있음
            ActionBus.Dispatch(payload);
            yield return action.seq;
        }
    }
}