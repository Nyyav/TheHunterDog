namespace MonogameExamples
{
    /// <summary>
    /// Представляет сообщение, указывающее, что игра должна вернуться к предыдущему уровню.
    /// Реализует интерфейс IMessage для использования с MessageBus.
    /// </summary>
    public class PreviousLevelMessage : IMessage
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PreviousLevelMessage"/>.
        /// </summary>
        public PreviousLevelMessage()
        {
        }
    }
}