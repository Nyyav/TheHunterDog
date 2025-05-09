namespace MonogameExamples
{
    /// <summary>
    /// Представляет сообщение, указывающее, что сущность должна быть уничтожена.
    /// Реализует интерфейс IMessage для использования с MessageBus.
    /// </summary>
    public class DestroyEntityMessage : IMessage
    {
        /// <summary>
        /// Получает сущность, которую нужно уничтожить.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DestroyEntityMessage"/>.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно уничтожить.</param>
        public DestroyEntityMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}