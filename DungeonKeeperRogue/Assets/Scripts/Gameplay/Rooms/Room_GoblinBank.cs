using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;

public class Room_GoblinBank : MapUnitBehaviour
{
    [SerializeField] private int _goldGain;
    [SerializeField] private int _maxTurns;
    [SerializeField] private FlyingText _flyingTextFX;
    [SerializeField] private float _duration;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _deactivatedColor;

    private int _turnCounter;

    public override IEnumerator OnStartOfTurn(MapUnit unit, MapUnitBehaviourContext context)
    {
        if (_turnCounter >= _maxTurns)
        {
            yield break;
        }
        
        yield return new WaitForSeconds(_duration);

        var flyingText = Instantiate(_flyingTextFX, transform.position, Quaternion.identity);
        flyingText.Init(_goldGain.ToString());
        Scenario.Instance.Player.AddCoins(_goldGain);
        ++_turnCounter;

        if (_turnCounter < _maxTurns)
        {
            yield break;
        }

        _spriteRenderer.color = _deactivatedColor;
    }
}
