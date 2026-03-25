using System.Collections;
using UnityEngine;

namespace DungeonKeeperRogue.Gameplay
{
    public abstract class MapUnitBehaviour : MonoBehaviour
    {
        public abstract IEnumerator Run(MapUnit unit);
    }
}