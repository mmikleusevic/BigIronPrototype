using CombatRoom;
using Managers;

public class CombatSceneInitializer : SceneInitializer
{
    public override void Initialize()
    {
        EncounterManager.Instance.InitializeEncounter();
    }
}