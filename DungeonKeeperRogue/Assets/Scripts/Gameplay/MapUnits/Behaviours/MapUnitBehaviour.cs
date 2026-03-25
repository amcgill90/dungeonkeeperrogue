using System.Collections;
using UnityEngine;

namespace DungeonKeeperRogue.Gameplay
{
    public class MapUnitBehaviour : MonoBehaviour
    {
		public virtual void Init()
		{
		}

        public virtual IEnumerator OnStartOfTurn(MapUnit unit)
		{
			// override this to run behaviour at the start of the owning player's turn
			yield return null;
		}

		public virtual IEnumerator OnEndOfTurn(MapUnit unit)
		{
			// override this to run behaviour at the end of the owning player's turn
			yield return null;
		}
    }
}