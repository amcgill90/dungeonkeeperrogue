using System.Collections.Generic;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playerTurnElements;

    private void Awake()
    {
        TurnController.OnTurnStart += OnTurnStart;
    }

    private void Start()
    {
        if (Scenario.Instance)
        {
            SetPlayerOnlyElementsActive(Scenario.Instance.TurnController.CurrentTeam == Team.Player);
        }
    }

    private void OnDestroy()
    {
        TurnController.OnTurnStart -= OnTurnStart;
    }

    private void OnTurnStart(Team team)
    {
        SetPlayerOnlyElementsActive(team == Team.Player);
    }

    private void SetPlayerOnlyElementsActive(bool active)
    {
        _playerTurnElements.ForEach(x => x.SetActive(active));
    }
}
