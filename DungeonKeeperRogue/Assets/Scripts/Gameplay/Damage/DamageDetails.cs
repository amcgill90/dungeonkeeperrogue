namespace DungeonKeeperRogue.Gameplay
{
    public class DamageDetails
    {
        public float _damage;
		public DamageSource _sourceType;
        public bool _showFlyingText = true;
        public float _flyingTextFontSizeMultiplier = 1.0f;
        public FlyingText _flyingTextPrefabOverride;
    
        public DamageDetails() { }
    
        public DamageDetails(int damage, DamageSource sourceType)
        {
            _damage = damage;
			_sourceType = sourceType;
        }
        
        public void Clone(DamageDetails details)
        {
            _damage = details._damage;
			_sourceType = details._sourceType;
            _showFlyingText = details._showFlyingText;
            _flyingTextFontSizeMultiplier = details._flyingTextFontSizeMultiplier;
            _flyingTextPrefabOverride = details._flyingTextPrefabOverride;  
        }
    }
}