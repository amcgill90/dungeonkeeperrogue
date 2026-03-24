using UnityEngine;

[CreateAssetMenu(fileName = "New Status Effect Definition", menuName = "Prototyping/Status Effect Definition")]
public class StatusEffectDefinition : ScriptableObject
{
    [SerializeField] private string _identifier;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Color _statusDisplayBackingColor = Color.black;
    [SerializeField] private StatusEffect _prefab;
    
    public string Identifier => _identifier;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public Color StatusDisplayBackingColor => _statusDisplayBackingColor;
    public StatusEffect Prefab => _prefab;
}
