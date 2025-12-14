public class StatusInstance
{
    public StatusEffect definition;
    public float remaining;
    public float tickAccumulator;
    public int stacks;

    public StatusInstance(StatusEffect def)
    {
        definition = def;
        remaining = def.baseDuration;
        tickAccumulator = 0f;
        stacks = 1;
    }

    public void Refresh()
    {
        remaining = definition.baseDuration;
    }
}