using System;

internal interface IHealthBar
{
    public event Action<float> OnHealthChange;
}