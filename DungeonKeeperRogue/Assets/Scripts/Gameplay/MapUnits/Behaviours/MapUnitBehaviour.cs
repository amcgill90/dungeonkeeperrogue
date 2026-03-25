using System.Collections;
using UnityEngine;

namespace DungeonKeeperRogue.Gameplay
{
    public class MapUnitBehaviour : MonoBehaviour
    {
		public virtual void Init()
		{
		}

		private void OnDestroy()
		{
			OnDestroyInternal();
		}

		protected virtual void OnDestroyInternal()
		{
			
		}

        public virtual IEnumerator OnStartOfTurn(MapUnit unit, MapUnitBehaviourContext context)
		{
			// override this to run behaviour at the start of the owning player's turn
			yield return null;
		}

		public virtual IEnumerator OnEndOfTurn(MapUnit unit, MapUnitBehaviourContext context)
		{
			// override this to run behaviour at the end of the owning player's turn
			yield return null;
		}
    }
}