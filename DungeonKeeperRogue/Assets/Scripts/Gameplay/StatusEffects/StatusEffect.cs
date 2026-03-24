using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    [SerializeField] private float _duration = -1;
    [SerializeField] protected bool _isStackable;
    [SerializeField] protected int _maxStacks = -1;
    [SerializeField] private bool _removeOnZeroStacks = true;
    [Tooltip("At the end of the duration, remove a single stack, instead of all.")]
    [SerializeField] private bool _durationRemoveSingleStack;
    [Tooltip("Each individual stack life time. Allows multiple to be culled at once")]
    [SerializeField] private float _stackDuration = -1;
    [SerializeField] private bool _refreshStacksOnStackAdded;
    [SerializeField] private bool _showStatusIcon = true;
    
    private float _totalLifeTime;
    private float _refreshableLifeTime;
    private StatusEffectContainer _container;
    private StatusEffectDefinition _statusDefinition;
    private readonly List<float> _stacks = new();
    
    public IStatusReceiver Owner => _container.Owner;
    public bool IsStackable => _isStackable;
    public bool ShowStatusIcon => _showStatusIcon;
    public float Duration => _duration;
    public float StackDuration => _stackDuration;
    public float RefreshableLifetime => _refreshableLifeTime;
    public float FirstStackTimeRemaining => TimeRemainingOnFirstStack();
    public int StackCount => _stacks.Count;
    public StatusEffectDefinition StatusDefinition => _statusDefinition;

    public event Action<StatusEffectDefinition> OnStatusReadyToBeRemoved;
    public event Action<StatusEffectDefinition> OnStatusDestroyed;
    public event Action OnStackCountChanged;

    #region Public Methods
    public void Init(StatusEffectContainer container, StatusEffectDefinition definition, int stackCount)
    {
        _container = container;
        _statusDefinition = definition;

        if (_isStackable)
        {
            AdjustStacks(stackCount);
        }
        
        InitInternal();
    }

    public void DestroyStatus()
    {
        DestroyStatusInternal();
        
        OnStatusReadyToBeRemoved?.Invoke(_statusDefinition);
        OnStatusDestroyed?.Invoke(_statusDefinition);
        
        Destroy(gameObject);
    }

    public void Refresh(int delta)
    {
        if (_isStackable)
        {
            AdjustStacks(delta);
            return;
        }

        RefreshLifeTime();
    }

    private void RefreshLifeTime()
    {
        _refreshableLifeTime = 0f;
    }
    
    public void AdjustStacks(int delta)
    {
        if (_isStackable == false)
        {
            return;
        }

        if (_refreshStacksOnStackAdded && delta > 0 && StackCount > 0)
        {
            for (int i = 0; i < StackCount; i++)
            {
                _stacks[i] = _totalLifeTime;
            }
        }

        var absoluteDelta = Mathf.Abs(delta);

        for (int i = 0; i < absoluteDelta; i++)
        {
            if (delta > 0)
            {
                if (_maxStacks > 0 && StackCount >= _maxStacks)
                {
                    break;
                }
                
                _stacks.Add(_totalLifeTime);
                continue;
            }

            if (StackCount <= 0)
            {
                break;
            }
            
            _stacks.RemoveAt(0);
        }
        
        AdjustStacksInternal(delta);
        OnStackCountChanged?.Invoke();
        CheckForStatusRemoval();
    }

    #endregion

    private void Update()
    {
        _totalLifeTime += Time.deltaTime;
        _refreshableLifeTime += Time.deltaTime;

        if (_isStackable && _refreshableLifeTime >= _duration && _durationRemoveSingleStack)
        {
            RefreshLifeTime();
            AdjustStacks(-1);
        }

        if (_isStackable && _stackDuration > 0)
        {
            for (int i = StackCount - 1; i >= 0; i--)
            {
                if (_totalLifeTime - _stacks[i] < _stackDuration)
                {
                    continue;
                }
                
                AdjustStacks(-1);
            }
        }
        
        OnUpdate();
        CheckForStatusRemoval();
    }

    private float TimeRemainingOnFirstStack()
    {
        bool hasStacks = _isStackable == false || StackCount <= 0;
        return hasStacks == false ? 0f : _stackDuration - (_totalLifeTime - _stacks[0]);
    }
    
    protected void CheckForStatusRemoval()
    {
        if (IsStatusReadyToRemove())
        {
            DestroyStatus();
        }
    }

    private bool IsStatusReadyToRemove()
    {
        return Owner.CanReceiveStatus(this) == false || HasExpired() || RemoveOnAllStacksLost();
    }

    private bool HasExpired()
    {
        return _duration > 0 && _refreshableLifeTime > _duration;
    }

    private bool RemoveOnAllStacksLost()
    {
        return _isStackable && _removeOnZeroStacks && StackCount <= 0;
    }
    
    protected virtual void InitInternal() { }
    protected virtual void DestroyStatusInternal() { }
    protected virtual void OnUpdate() { }
    protected virtual void AdjustStacksInternal(int delta) { }
}
