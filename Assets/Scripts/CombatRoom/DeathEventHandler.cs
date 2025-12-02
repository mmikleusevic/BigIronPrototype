using System;
using Managers;

namespace CombatRoom
{
    public class DeathEventHandler
    {
        private readonly CombatRoomController combatRoomController;
        private readonly Combatant combatant;
        public Action DeathDelegate { get; private set; }
        
        public DeathEventHandler(Combatant combatant, CombatRoomController combatRoomController)
        {
            this.combatant = combatant;
            this.combatRoomController = combatRoomController;
            
            DeathDelegate = OnDied; 
        }
        
        private void OnDied()
        {
            combatant.Health.OnDied -= DeathDelegate;
            
            combatRoomController.UnsubscribeFromDeath(combatant);
            
            combatRoomController.HandleDeathCleanup(combatant);
        }
    }
}