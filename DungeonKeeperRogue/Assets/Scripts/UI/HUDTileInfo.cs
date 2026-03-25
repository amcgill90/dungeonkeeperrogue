using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDTileInfo : MonoSingleton<HUDTileInfo>
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _iconImage;

    protected override void OnInitialized()
    {
        Hide();
        
        // Subscribe to combat end
    }

    private void OnDestroy()
    {
        // Unsubscribe to combat end
    }

    private void OnCombatEnd()
    {
        Hide();
    }

    public void ShowTileInfo(string name, string description, Sprite icon)
    {
        gameObject.SetActive(true);
        
        _nameText.SetText(name);
        _descriptionText.SetText(description);
        _iconImage.sprite = icon;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
