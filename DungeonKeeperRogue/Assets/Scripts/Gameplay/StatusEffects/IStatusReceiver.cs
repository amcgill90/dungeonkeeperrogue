public interface IStatusReceiver
{
    StatusEffectContainer StatusContainer { get; }
    bool CanReceiveStatus(StatusEffect status);
}
