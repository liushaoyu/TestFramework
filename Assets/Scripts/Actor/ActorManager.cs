using System.Collections.Generic;

public class ActorManager : Singleton<ActorManager>
{
    public List<Actor> actorList { get; private set; } = new List<Actor>();

    public void Init(uint actorNum)
    {
        for (uint i = 0; i < actorNum; i++)
        {
            Actor actor = new Actor();
            actor.Init(i);
            
            actorList.Add(actor);
        }
    }

    public int GetActorCount() { return actorList.Count; }

    public Actor GetActor(int index) {  return actorList[index]; }
}
