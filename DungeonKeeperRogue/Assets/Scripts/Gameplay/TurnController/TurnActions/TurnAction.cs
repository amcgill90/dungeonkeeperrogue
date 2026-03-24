using System.Collections;

namespace DungeonKeeperRogue
{
    public abstract class TurnAction
    {
        public delegate void ActionTriggered(TurnAction action);

        public static event ActionTriggered OnActionTriggered;

        public void Trigger()
        {
            OnActionTriggered?.Invoke(this);
        }

        // Coroutine so we can wait for animations of tiles being placed, combat playing out etc?
        // Not sure if this should be handled some other way
        public abstract IEnumerator Run();
    }
}