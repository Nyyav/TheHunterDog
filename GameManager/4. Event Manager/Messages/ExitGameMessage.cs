namespace MonogameExamples
{
    /// <summary>
    /// Представляет сообщение, указывающее, что игра должна завершиться.
    /// Реализует интерфейс IMessage для использования с MessageBus.
    /// </summary>
    public class ExitGameMessage : IMessage
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ExitGameMessage"/>.
        /// </summary>
        public ExitGameMessage()
        {
        }
    }
}