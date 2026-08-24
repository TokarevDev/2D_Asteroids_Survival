namespace Game.Gameplay.Combat
{
    public interface IDamageable
    {
        int CurrentHealth { get; }

        void TakeDamage(int damage);
    }
}
