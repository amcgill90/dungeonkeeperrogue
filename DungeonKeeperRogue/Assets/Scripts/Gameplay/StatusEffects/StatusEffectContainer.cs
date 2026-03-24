using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusApplication
{
    public StatusEffectDefinition _statusEffectDefinition;
    public int _stackCount = 1;

    public StatusApplication(StatusEffectDefinition status, int stackCount)
    {
        _statusEffectDefinition = status;
        _stackCount = stackCount;
    }
}

public class StatusEffectContainer : MonoBehaviour
{
    private IStatusReceiver _owner;
    
    private List<StatusTag> _statusTags = new();
    private readonly Dictionary<StatusEffectDefinition, StatusEffect> _statusEffectMap = new();
    
    public IStatusReceiver Owner => _owner;
    
    public event Action<StatusEffect> OnStatusEffectAdded;
    
    private void OnStatusReadyToBeRemoved(StatusEffectDefinition statusDefinition)
    {
        RemoveStatus(statusDefinition);
    }
    
    public void Init(IStatusReceiver owner)
    {
        _owner = owner;
    }

    public bool HasTag(StatusTag statusTag)
    {
        return _statusTags.Contains(statusTag);
    }

    public void AddTag(StatusTag statusTag)
    {
        _statusTags.Add(statusTag);
    }
    
    public bool FindAndConsumeTag(StatusTag statusTag)
    {
        StatusTag foundTag = _statusTags.Find(t => t == statusTag);
        if (foundTag == null)
        {
            return false;
        }
        
        _statusTags.Remove(foundTag);
        return true;
    }

    public void RemoveTag(StatusTag statusTag)
    { 
        _statusTags.Remove(statusTag);
    }
    
    public bool HasStatus(StatusEffectDefinition statusEffect)
    {
        return _statusEffectMap.ContainsKey(statusEffect);
    }
    
    public void RemoveStatus(StatusEffectDefinition statusDefinition)
    {
        if (HasStatus(statusDefinition))
        {
            _statusEffectMap.Remove(statusDefinition);
        }
    }
    
    public void AddStatus(StatusApplication application)
    {
        var statusDefinition = application._statusEffectDefinition;
        
        if (HasStatus(statusDefinition))
        {
            GetStatus(statusDefinition).Refresh(application._stackCount);
            return;
        }

        StatusEffect newStatus = Instantiate(statusDefinition.Prefab, transform);
        newStatus.Init(this, statusDefinition, application._stackCount);
        
        _statusEffectMap[statusDefinition] = newStatus;
        
        OnStatusEffectAdded?.Invoke(newStatus);
        
        newStatus.OnStatusReadyToBeRemoved += OnStatusReadyToBeRemoved;
    }

    public StatusEffect GetStatus(StatusEffectDefinition statusDefinition)
    {
        return _statusEffectMap[statusDefinition];
    }

    public List<StatusEffect> GetAllStatuses()
    {
        var statuses = new List<StatusEffect>();
        foreach (var kvp in _statusEffectMap)
        {
            statuses.Add(kvp.Value);
        }
        
        return statuses;
    }
}