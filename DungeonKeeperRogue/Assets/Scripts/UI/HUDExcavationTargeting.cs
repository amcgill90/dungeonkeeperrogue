using TMPro;
using UnityEngine;

public class HUDExcavationTargeting : MonoSingleton<HUDExcavationTargeting>
{
    [SerializeField] private TMP_Text _excavationCountText;
    [SerializeField] private string _excavationCountFormat;

	public delegate void SkipDelegate();
	public static event SkipDelegate OnSkip;


    protected override void OnInitialized()
    {
        HideUI();
    }
    
    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowExcavationUI(int digIndex, int totalDigs)
    {
        gameObject.SetActive(true);
        var excavationString = string.Format(_excavationCountFormat, digIndex, totalDigs);
        _excavationCountText.SetText(excavationString);
        
        HUDCombatInfo.Instance.HideEndTurnButton();
    }
    
    public void CompleteExcavation()
    {
        HideUI();
        
        HUDCombatInfo.Instance.ShowEndTurnButton();
    }

	public void SkipExcavation()
	{
		OnSkip?.Invoke();
	}
}
