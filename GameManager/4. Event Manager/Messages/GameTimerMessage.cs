namespace MonogameExamples
{
    /// <summary>
    /// Указывает, что игровой таймер был создан.
    /// Реализует интерфейс IMessage для использования с <see cref ="MessageBus"/>.
    /// </summary>
    public class GameTimerMessage : IMessage
    {
        /// <summary>
        /// Сущность, которая содержит таймер.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Значение таймера в секундах.
        /// </summary>
        public float Timer { get; }

        /// <summary>
        /// Позиция таймера.
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GameTimerMessage"/>.
        /// </summary>
        /// <param name="entity">Сущность, которая содержит таймер.</param>
        public GameTimerMessage(Entity entity, float timer, Vector2 position)
        {
            Entity = entity;
            Timer = timer;
            Position = position;
        }
    }
}