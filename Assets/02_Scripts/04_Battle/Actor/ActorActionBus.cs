using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActorAction
{
    None = 0,
    AtkDmg = 50001,
    AtkFixedDmg,
    AtkLossHp,
    AddBlock = 50101,
    HealHp,
    AddMaxHp,
    AddCost
}

public class ActorActionBus
{
    private static readonly float damageModifier = 0.3f;

    private ActorStatus status;
    private ActorEventBus eventBus;

    readonly private Dictionary<ActorAction, ActorActionPayload> actionMap;

    public ActorActionBus()
    {
       
    }

    public void Bind(ActorStatus status, ActorEventBus eventBus)
    {
        this.status = status;
        this.eventBus = eventBus;
    }

    private void AtkDmg(ActorActionPayload actionPayload)
    {   
        Actor attackTarget = actionPayload.target;
        Element attackElement = actionPayload.attackElement;
        int damage = actionPayload.damage;
    }

    private void AtkFixedDmg(Actor target, int damage, Element element)
    {
      
    }

    private void AtkLossHp(Actor target, int damage)
    {
        target.status.Hp -= damage;
    }

    private void AddBlock(ActorActionPayload actorActionPayload)
    {
        int block = actorActionPayload.block;

        status.Block += block;
    }

    private void HealHp(int healPoint)
    {
        status.Hp += healPoint;
    }

    private void AddMaxHp(int maxHp)
    {
        status.MaxHp += maxHp;
    }

    private void AddCost(int cost)
    {
        status.Cost += cost;
    }

    private int ModifyDamageByElement(int hitPoint, Element attackType)
    {
        Element element = status.Element;

        if (element == Element.None) return hitPoint;

        int modifiedDamage = 0;

        if (attackType == Element.Ruin)
        {
            if (element == Element.Bio)
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (element == Element.Psychic)
                modifiedDamage = (int)(-hitPoint * damageModifier);
        }

        else if (attackType == Element.Psychic)
        {
            if (element == Element.Ruin)
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (element == Element.Bio)
                modifiedDamage = (int)(-hitPoint * damageModifier);
        }

        else if (attackType == Element.Bio)
        {
            if (element == Element.Psychic)
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (element == Element.Ruin)
                modifiedDamage = (int)(-hitPoint * damageModifier);
        }

        return hitPoint + modifiedDamage;
    }
}

public class ActorActionPayload
{
    public ActorAction actionId;
    public Actor source;
    public Actor target;
    public int damage;
    public Element attackElement;
    public int block;

    public void Init()
    {
        actionId = ActorAction.None;
    }
}