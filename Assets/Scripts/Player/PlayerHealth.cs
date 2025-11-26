namespace Player
{
    public class PlayerHealth : Health
    {
        protected override void Die()
        {
            base.Die();
            
            //TODO trigger game over
        }
    }
}
