using System;
using TMPro;
using UnityEngine;

public class HUDCombatInfo : MonoSingleton<HUDCombatInfo>
{
    [SerializeField] private TMP_Text _currentGoldText;
    [SerializeField] private TMP_Text _bossHealthText;
    [SerializeField] private string _bossHealthStringFormat = "{0}/{1}";
    [SerializeField] private ProgressBar _bossHealthBar;
    [SerializeField] private GameObject _endTurnButton;

    protected override void OnInitialized()
    {
        // TODO: subscribe to events
    }

    private void OnDestroy()
    {
        // TODO: unsubscribe to events
    }

    private void OnCombatStart()
    {
        // _bossHealthBar.Init(maxHealth, currentHealth);
        OnBossHealthChange();
    }

    private void OnPlayerTurnStart()
    {
        ShowEndTurnButton();
    }

    private void OnCoinsChanged()
    {
        var coinCountString = "3";
    }

    private void OnBossHealthChange()
    {
        var bossHealthString = "20/30";
        _bossHealthText.SetText(bossHealthString);
        // _bossHealthBar.UpdateProgress(currentHealth);
    }

    public void EndTurnButtonPressed()
    {
        HideEndTurnButton();
        // TODO: end turn functionality
    }

    // When placing a room or excavating, probably should hide this button
    public void HideEndTurnButton()
    {
        _endTurnButton.SetActive(false);
    }

    public void ShowEndTurnButton()
    {
        _endTurnButton.SetActive(true);
    }
}
