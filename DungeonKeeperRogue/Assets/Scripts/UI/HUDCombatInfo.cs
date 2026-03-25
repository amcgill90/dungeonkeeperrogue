using TMPro;
using UnityEngine;

public class HUDCombatInfo : MonoSingleton<HUDCombatInfo>
{
    [SerializeField] private TMP_Text _currentGoldText;
    [SerializeField] private TMP_Text _bossHealthText;
    [SerializeField] private string _bossHealthStringFormat = "{0}/{1}";
    [SerializeField] private ProgressBar _bossHealthBar;
    [SerializeField] private GameObject _endTurnButton;

    public delegate void EndTurnDelegate();
    public static event EndTurnDelegate OnEndTurn;
    
    protected override void OnInitialized()
    {
        Player.OnCoinsChanged += OnCoinsChanged;
		MapActor.OnHealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        Player.OnCoinsChanged -= OnCoinsChanged;
		MapActor.OnHealthChanged -= OnHealthChanged;
    }

    private void OnCoinsChanged(int change, int newAmount)
    {
        _currentGoldText.text = newAmount.ToString();
    }

    private void OnHealthChanged(Team team, int currentHp, int maxHp)
    {
        var bossHealthString = string.Format(_bossHealthStringFormat, currentHp, maxHp);
        _bossHealthText.SetText(bossHealthString);

		if (_bossHealthBar.MaxValue != maxHp)
		{
			_bossHealthBar.Init(maxHp, currentHp);
		}

        _bossHealthBar.UpdateProgress(currentHp);
    }

    public void EndTurnButtonPressed()
    {
        HideEndTurnButton();
        OnEndTurn?.Invoke();
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
