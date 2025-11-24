using CombatRoom;
using Managers;

public class CombatSceneInitializer : SceneInitializer
{
    protected override void Initialize()
    {
        EncounterManager.Instance.InitializeEncounter();
    }
}