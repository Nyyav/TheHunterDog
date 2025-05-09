using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/>, представляющий таймер возрождения для сущности.
    /// </summary>
    public class RespawnComponent : Component
    {

        private readonly float _respawnTimer;
        private float _currentTimer;
        private bool _isRespawning;

        /// <summary>
        /// Получает значение, указывающее, находится ли сущность в процессе возрождения.
        /// </summary>
        public bool IsRespawning => _isRespawning;

        /// <summary>
        /// Получает и устанавливает позицию возрождения для сущности.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RespawnComponent"/> с указанным таймером возрождения.
        /// </summary>
        /// <param name="respawnTimer">Продолжительность в секундах для возрождения сущности.</param>
        public RespawnComponent(Vector2 respawnPosition, float respawnTimer = 5)
        {
            _respawnTimer = respawnTimer;
            _currentTimer = 0f;
            _isRespawning = false;
            Position = respawnPosition;
        }

        /// <summary>
        /// Запускает таймер возрождения для сущности.
        /// </summary>
        public void StartRespawn()
        {
            _currentTimer = 0f;
            _isRespawning = true;
        }

        /// <summary>
        /// Обновляет компонент возрождения.
        /// </summary>
        /// <param name="gameTime">Текущее игровое время.</param>
        public void Update(GameTime gameTime)
        {
            if (_isRespawning)
            {
                _currentTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_currentTimer >= _respawnTimer)
                {
                    // Respawned
                    _isRespawning = false;
                }
            }
        }
    }
}