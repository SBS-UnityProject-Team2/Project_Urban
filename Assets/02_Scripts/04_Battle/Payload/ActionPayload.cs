using System.Collections.Generic;

public class ActionPayload : Payload
{
    public ActorAction actionId;
    public Actor source;
    public List<Actor> targets = new();

    public override void Init()
    {
        base.Init();
        
        source   = null;
        targets.Clear();
    }

    public void AddTarget(Actor actor)
    {
        targets.Add(actor);
    }
}