using UnityEngine;

namespace DungeonKeeperRogue.Gameplay
{
    [CreateAssetMenu(menuName = "DungeonKeeperRogue/Map Units/Map Unit")]
    public class MapUnitConfig : ScriptableObject
    {
        [SerializeField] private Team _team;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _baseHealth;

        public Team Team => _team;
        public Sprite Sprite => _sprite;
        public int BaseHealth => _baseHealth;
    }
}