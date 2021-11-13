using Tarret;
public interface ITarretStateChangeable
{
    public void ToIdle();
    public void ToRotate();
    public void ToAttack();
    public void ToBreak();
}

public interface ITarretStateExecutable
{
    public void EnterTarretState();
    public void StateUpdate();
}