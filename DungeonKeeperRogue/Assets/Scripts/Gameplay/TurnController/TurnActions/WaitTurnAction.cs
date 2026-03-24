using System.Collections;
using UnityEngine;

namespace DungeonKeeperRogue
{
    public class WaitTurnAction : TurnAction
    {
        public WaitTurnAction(float duration)
        {
            _duration = duration;
        }

        private readonly float _duration;
        
        public override IEnumerator Run()
        {
            yield return new WaitForSeconds(_duration);
        }
    }
}