using DungeonKeeperRogue.Gameplay;
using UnityEngine;
using UnityEngine.Events;

public class MoveTowardsBoss : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _destroyAfterReachingDuration;
    [SerializeField] private UnityEvent _onReachedTarget;
    
    private bool _hasReachedTarget;
    private MapUnit _bossUnit;
    private float _destroyTimer;

    private void Awake()
    {
        MapActor ma = Scenario.Instance.GetMapActorForTeam(Team.Enemy);
        if (ma == false || ma.UnitCount == 0)
        {
            return;
        }

        _bossUnit = ma.Units[Random.Range(0, ma.Units.Count)];

        if (_bossUnit == false || _bossUnit.Health == false)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_hasReachedTarget == false)
        {
            return;
        }
        
        _destroyTimer += Time.deltaTime;

        if (_destroyTimer >= _destroyAfterReachingDuration)
        {
            Destroy(gameObject);
        }
    }
    
    private void FixedUpdate()
    {
        if (_bossUnit == false || _hasReachedTarget)
        {
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, 
            _bossUnit.transform.position, _speed * Time.fixedDeltaTime);
        
        _hasReachedTarget = Vector2.Distance(transform.position, _bossUnit.transform.position) < 0.1f;

        if (_hasReachedTarget)
        {
            _onReachedTarget.Invoke();
        }
    }
}
