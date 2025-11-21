namespace Player
{
    public class PlayerHealth : Health
    {
        protected override void Die()
        {
            base.Die();
            
            //TODO trigger game over
        }
        
        public void RefreshState()
        {
            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        }
    }
}
