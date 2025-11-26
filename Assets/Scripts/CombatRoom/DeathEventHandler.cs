using System;

namespace CombatRoom
{
    public class DeathEventHandler
    {
        private readonly CombatRoomManager combatRoomManager;
        private readonly Combatant combatant;
        public Action DeathDelegate { get; private set; }
        
        public DeathEventHandler(Combatant combatant, CombatRoomManager combatRoomManager)
        {
            this.combatant = combatant;
            this.combatRoomManager = combatRoomManager;
            
            DeathDelegate = OnDied; 
        }
        
        private void OnDied()
        {
            combatant.Health.OnDied -= DeathDelegate;
            
            combatRoomManager.UnsubscribeFromDeath(combatant);
            
            combatRoomManager.HandleDeathCleanup(combatant);
        }
    }
}