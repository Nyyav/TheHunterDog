namespace MonogameExamples
{
    /// <summary>
    /// Сообщение о том, что данная сущность умерла.
    /// Реализует интерфейс IMessage для использования с MessageBus.
    /// </summary>
    public class EntityDiedMessage : IMessage
    {
        /// <summary>
        /// Получает сущность, которая умерла.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityDiedMessage"/>.
        /// </summary>
        /// <param name="entity">Сущность, которая умерла.</param>
        public EntityDiedMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}