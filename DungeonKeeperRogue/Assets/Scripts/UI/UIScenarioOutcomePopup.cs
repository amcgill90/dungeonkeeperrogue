using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DungeonKeeperRogue.UI
{
    public class UIScenarioOutcomePopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private string _winMessage = "VICTORY";
        [SerializeField] private string _lostMessage = "DEFEATED";
        [SerializeField] private GameObject _wonRoot;
        [SerializeField] private GameObject _lostRoot;
		[SerializeField] private Transform _rewardsRoot;
		[SerializeField] private UIScenarioOutcomeRewardItem _rewardItemPrefab;
		
        
		private List<Card> _rewardOptions;
        private event Action<Card> OnContinue;

        private void Awake()
        {
            Show(false);
        }

        private void Show(bool active)
        {
            gameObject.SetActive(active);

			if (active == false)
			{
				Cleanup();
			}
        }

		private void Cleanup()
		{
			for (int i = _rewardsRoot.childCount - 1; i >= 0; --i)
			{
				Destroy(_rewardsRoot.GetChild(i).gameObject);
			}
		}
        
        public void Open(Team winningTeam, List<Card> rewardOptions, Action<Card> onContinue)
        {
			Cleanup();
            Show(true);
            
            bool playerWon = winningTeam == Team.Player;
            
            _titleText.SetText(playerWon ? _winMessage : _lostMessage);
            _wonRoot.SetActive(playerWon);
            _lostRoot.SetActive(playerWon == false);
			_rewardOptions = rewardOptions;

			if (playerWon)
			{
				foreach (Card card in rewardOptions)
				{
					UIScenarioOutcomeRewardItem item = Instantiate(_rewardItemPrefab, _rewardsRoot);
					item.ShowInfo(card, OnCardSelected);
				}
			}
            
            OnContinue = onContinue;
        }

		private void OnCardSelected(Card card)
		{
			Show(false);
			OnContinue?.Invoke(card);
		}

        public void ContinuePressed()
        {
            Show(false);
            OnContinue?.Invoke(null);
        }
    }
}
