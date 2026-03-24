using System;

[Serializable]
public class IntStatProperty
{
    public IntStatProperty(int baseValue)
    {
        _baseValue = baseValue;
        _modifierTotal = 0;
    }
    
    private int _baseValue;
    private int _modifierTotal;

    public int GetBaseValue()
    {
        return _baseValue;
    }

    public int GetCurrentValue()
    {
        return _baseValue + _modifierTotal;
    }

    public void SetBaseValue(int baseValue)
    {
        _baseValue = baseValue;
    }

    public void AddModifier(int amount)
    {
        _modifierTotal += amount;
    }

    public void RemoveModifier(int amount)
    {
        _modifierTotal -= amount;
    }
}

[Serializable]
public class FloatStatProperty
{
    public FloatStatProperty(float baseValue)
    {
        _baseValue = baseValue;
        _modifierTotal = 0;
    }
    
    private float _baseValue;
    private float _modifierTotal;

    public float GetBaseValue()
    {
        return _baseValue;
    }

    public float GetCurrentValue()
    {
        return _baseValue + _modifierTotal;
    }

    public int GetCurrentIntValue()
    {
        return (int) (_baseValue + _modifierTotal);
    }

    public void SetBaseValue(float baseValue)
    {
        _baseValue = baseValue;
    }

    public void AddModifier(float amount)
    {
        _modifierTotal += amount;
    }

    public void RemoveModifier(float amount)
    {
        _modifierTotal -= amount;
    }
}