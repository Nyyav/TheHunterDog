namespace MonogameExamples
{
    /// <summary>
    /// Представляет сообщение для добавления сущности.
    /// </summary>
    public class AddEntityMessage : IMessage
    {
        /// <summary>
        /// Получает сущность, которую нужно добавить.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AddEntityMessage"/>.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно добавить.</param>
        public AddEntityMessage(Entity entity)
        {
            Entity = entity;
        }
    }
}