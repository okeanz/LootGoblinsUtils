using System;

namespace LootGoblinsUtils.Submods.Farming.Pieces.Configurators;

public abstract class Configurable
{
    public string Name;
    public void Configure()
    {
        try
        {
            Setup();
            Jotunn.Logger.LogInfo($"{Name} constructing completed");
        }
        catch (Exception e)
        {
            Jotunn.Logger.LogError($"{Name} constructing failed");
            Jotunn.Logger.LogError(e);
        }
    }

    protected abstract void Setup();
}