using System;
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
        
        private event Action OnContinue;

        private void Awake()
        {
            Show(false);
        }

        private void Show(bool active)
        {
            gameObject.SetActive(active);
        }
        
        public void Open(Team winningTeam, Action onContinue)
        {
            Show(true);
            
            bool playerWon = winningTeam == Team.Player;
            
            _titleText.SetText(playerWon ? _winMessage : _lostMessage);
            _wonRoot.SetActive(playerWon);
            _lostRoot.SetActive(playerWon == false);
            
            OnContinue = onContinue;
        }

        public void ContinuePressed()
        {
            Show(false);
            OnContinue?.Invoke();
        }
    }
}
