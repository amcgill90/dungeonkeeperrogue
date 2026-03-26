using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScenarioOutcomeRewardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _iconImage;

	private Card _card = null;
	private Action<Card> _action;


    public void ShowInfo(Card card, Action<Card> action)
    {
        gameObject.SetActive(true);
        
        _nameText.SetText(card.Name);
        _descriptionText.SetText(card.GetDescription());
        _iconImage.sprite = card.Icon;
		_card = card;
		_action = action;
    }

	public void Select()
	{
		_action?.Invoke(_card);
	}
}
