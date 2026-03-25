using System.Collections;
using DungeonKeeperRogue.Gameplay;
using UnityEngine;
using UnityEngine.Events;

public class Room_TeslaCoil : MapUnitBehaviour
{
    [SerializeField] private Team _targetTeam;
    [SerializeField] private int _damageAmount = 10;
    [SerializeField] private float _fxStartDuration;
    [SerializeField] private float _fxSecondStageDuration;
    [SerializeField] private ParticleSystem _travellingParticles;
    [SerializeField] private UnityEvent _onFXStart;
    [SerializeField] private UnityEvent _onFXSecondStage;

    private MapUnit _targetUnit;

    public override IEnumerator OnEndOfTurn(MapUnit unit, MapUnitBehaviourContext context)
    {
        MapActor ma = Scenario.Instance.GetMapActorForTeam(_targetTeam);
        if (ma == false || ma.UnitCount == 0)
        {
            yield break;
        }

        _targetUnit = ma.Units[Random.Range(0, ma.Units.Count)];

        if (_targetUnit == false || _targetUnit.Health == false)
        {
            yield break;
        }
        
        _onFXStart.Invoke();
        yield return new WaitForSeconds(_fxStartDuration);

        var secondStageTimer = 0f;
        _travellingParticles.transform.position = transform.position;
        _travellingParticles.Play();
        _onFXSecondStage.Invoke();

        while (secondStageTimer < _fxSecondStageDuration)
        {
            var normalizedTimer = secondStageTimer / _fxSecondStageDuration;
            _travellingParticles.transform.position = Vector2.Lerp(_travellingParticles.transform.position,
                _targetUnit.transform.position, normalizedTimer);
            secondStageTimer += Time.deltaTime;
            yield return null;
        }
        
        _travellingParticles.Stop();

        DamageDetails damageDetails = new(_damageAmount, DamageSource.Room);
        _targetUnit.Health.TryDamage(damageDetails);
        
        yield return null;
    }
}
