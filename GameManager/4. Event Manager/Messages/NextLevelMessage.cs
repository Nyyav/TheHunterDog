namespace MonogameExamples
{
    /// <summary>
    /// Представляет сообщение, указывающее, что игра должна перейти на следующий уровень.
    /// Реализует интерфейс IMessage для использования с MessageBus.
    /// </summary>
    public class NextLevelMessage : IMessage
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NextLevelMessage"/>.
        /// </summary>
        public NextLevelMessage()
        {
        }
    }
}