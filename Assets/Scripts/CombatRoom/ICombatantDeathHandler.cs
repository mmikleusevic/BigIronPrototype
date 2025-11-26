namespace CombatRoom
{
    public interface ICombatantDeathHandler
    {
        public void HandleDeathEffects(Combatant killer);
    }
}