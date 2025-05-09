using System;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// Представляет сообщение, которое можно опубликовать и на которое можно подписаться.
    /// </summary>
    public interface IMessage { }

    /// <summary>
    /// Предоставляет реализацию шины сообщений для декомпозиции взаимодействия
    /// между компонентами, используя шаблон "издатель-подписчик".
    /// </summary>
    public class MessageBus
    {
        // Словарь, который содержит данные о подписке.
        // Ключи - это типы сообщений, а значения - списки действий, которые нужно вызвать при публикации сообщения.
        private static Dictionary<Type, List<Action<IMessage>>> subscribers = new Dictionary<Type, List<Action<IMessage>>>();

        /// <summary>
        /// Подписывает действие на определенный тип сообщения.
        /// </summary>
        /// <typeparam name="T">Тип сообщения, на которое нужно подписаться. Должен реализовывать IMessage.</typeparam>
        /// <param name="action">Действие, которое нужно выполнить, когда публикуется сообщение указанного типа.</param>
        public static void Subscribe<T>(Action<T> action) where T : IMessage
        {
            if (!subscribers.ContainsKey(typeof(T)))
            {
                subscribers.Add(typeof(T), new List<Action<IMessage>>());
            }

            subscribers[typeof(T)].Add(message => action((T)message));
        }

        /// <summary>
        /// Отписывает действие от определенного типа сообщения.
        /// </summary>
        /// <typeparam name="T">Тип сообщения, от которого нужно отписаться. Должен реализовывать IMessage.</typeparam>
        /// <param name="action">Действие, которое нужно удалить из списка подписчиков для указанного типа сообщения.</param>
        /// <remarks>
        /// Этот метод ищет всех подписчиков с той же целью, что и предоставленное действие, и удаляет их из списка подписчиков.
        /// </remarks>
        public static void Unsubscribe<T>(Action<T> action) where T : IMessage
        {
            Type messageType = typeof(T);

            if (subscribers.ContainsKey(messageType))
            {
                List<Action<IMessage>> actionList = subscribers[messageType];
                List<Action<IMessage>> actionsToRemove = new List<Action<IMessage>>();

                foreach (var subscriber in actionList)
                {
                    if (subscriber.Target == action.Target)
                    {
                        actionsToRemove.Add(subscriber);
                    }
                }

                foreach (var actionToRemove in actionsToRemove)
                {
                    actionList.Remove(actionToRemove);
                }
            }
        }

        /// <summary>
        /// Публикует сообщение всем подписчикам типа сообщения.
        /// </summary>
        /// <param name="message">Сообщение, которое нужно опубликовать.</param>
        public static void Publish(IMessage message)
        {
            Type messageType = message.GetType();

            if (subscribers.ContainsKey(messageType))
            {
                foreach (var subscriber in subscribers[messageType])
                {
                    subscriber(message);
                }
            }
        }
    }
}