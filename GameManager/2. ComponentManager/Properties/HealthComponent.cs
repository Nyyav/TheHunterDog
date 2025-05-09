using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/>, представляющий здоровье сущности.
    /// </summary>
    public class HealthComponent : Component
    {
        private int _currentHealth;
        private int _maxHealth;

        /// <summary>
        /// Получает или устанавливает текущее значение здоровья.
        /// </summary>
        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = MathHelper.Clamp(value, 0, MaxHealth);
        }

        /// <summary>
        /// Получает или устанавливает максимальное значение здоровья.
        /// </summary>
        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = MathHelper.Max(0, value);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HealthComponent"/> с указанным максимальным значением здоровья.
        /// </summary>
        /// <param name="maxHealth">Максимальное значение здоровья.</param>
        public HealthComponent(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
    }
}