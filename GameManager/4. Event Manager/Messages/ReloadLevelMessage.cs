namespace MonogameExamples
{
    /// <summary>
    /// Представляет сообщение, указывающее, что текущий уровень игры должен быть перезагружен.
    /// Реализует интерфейс IMessage для использования с MessageBus.
    /// </summary>
    public class ReloadLevelMessage : IMessage
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ReloadLevelMessage"/>.
        /// </summary>
        public ReloadLevelMessage()
        {
        }
    }
}