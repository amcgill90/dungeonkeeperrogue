namespace DungeonKeeperRogue.Gameplay
{
    public class DamageDetails
    {
        public float _damage;
        public bool _showFlyingText = true;
        public float _flyingTextFontSizeMultiplier = 1.0f;
        public FlyingText _flyingTextPrefabOverride;
    
        public DamageDetails() { }
    
        public DamageDetails(int damage)
        {
            _damage = damage;
        }
        
        public void Clone(DamageDetails details)
        {
            _damage = details._damage;
            _showFlyingText = details._showFlyingText;
            _flyingTextFontSizeMultiplier = details._flyingTextFontSizeMultiplier;
            _flyingTextPrefabOverride = details._flyingTextPrefabOverride;  
        }
    }
}