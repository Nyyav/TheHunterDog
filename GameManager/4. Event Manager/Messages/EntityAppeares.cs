namespace MonogameExamples
{
    /// <summary>
    /// Сообщение о том, что данная сущность должна появиться снова.
    /// Реализует интерфейс IMessage для использования с MessageBus.
    /// </summary>
    public class EntityReAppearsMessage : IMessage
    {
        /// <summary>
        /// Получает сущность, которая должна появиться снова.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityReAppearsMessage"/>.
        /// </summary>
        /// <param name="entity">Сущность, которая должна появиться снова.</param>
        public EntityReAppearsMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}