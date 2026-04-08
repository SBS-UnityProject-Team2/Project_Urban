using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAction : MonoBehaviour
{
    private List<PhaseEntry> phaseEntries = new();
    private List<PatternEntry> curPattern;
    private List<PhaseStepEntry> phaseStep;

    private int curPhase = 0;
    private int curIndex = 0;
    
    private MonsterActionDataEntry curAction;
    public MonsterActionDataEntry CurAction;

    public int NextPhaseHp => phaseStep[curPhase].triggerHp;
    public UnityEvent<MonsterActionDataEntry> OnUpdateNextAction = new(); 

    public void Init(Monster monster, MonsterDataEntry monsterDataEntry)
    {
        phaseEntries = monsterDataEntry.pattern; 
        phaseStep = monsterDataEntry.phaseStep;

        Debug.Assert(phaseStep.Count == phaseEntries.Count,
            $"phaseStep({phaseStep.Count}) != pattern phases({phaseEntries.Count})");

        curPattern = phaseEntries[curPhase].patterns;        
        SetNextAction();
    }

    public async UniTask Execute(Monster source)
    {
        await MonsterActionMethod.Execute(curAction.actionId, source);        
    }

    public void SetNextAction()
    {
        int actionId = curPattern[curIndex][Random.Range(0, curPattern[curIndex].Count)];
        curAction = MonsterManager.Instance.GetMonsterAction(actionId);

        curIndex++;
        curIndex %= curPattern.Count;

        OnUpdateNextAction?.Invoke(curAction);
    }

    public void SetNextPhase()
    {
        if (NextPhaseHp == 0) return;

        curPhase++;
        curPattern = phaseEntries[curPhase].patterns;
        
        Reset();
        if (phaseStep[curPhase].actionId == 0)
            SetNextAction();
        else
            curAction = MonsterManager.Instance.GetMonsterAction(phaseStep[curPhase].actionId);
    }

    public void Reset()
    {
        curIndex = 0;
    }
} 