using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> которая управляет событиями появления объектов.
    /// </summary>
    public class AppearSystem : System
    {
        private List<Entity> _entities;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="AppearSystem"/> класса.
        /// </summary>
        public AppearSystem()
        {
            _entities = new List<Entity>();
        }

        /// <summary>
        /// Подписывается на соответствующие сообщения
        /// </summary>
        public override void Subscribe()
        {
            MessageBus.Subscribe<EntityReAppearsMessage>(EntityAppeared);
        }

        /// <summary>
        /// Отписывается от всех сообщений
        /// </summary>
        public override void Unsubscribe()
        {
            MessageBus.Unsubscribe<EntityReAppearsMessage>(EntityAppeared);
        }

        /// <summary>
        /// Добавляет объект в систему.
        /// </summary>
        /// <param name="entity">Объект, который нужно добавить.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            if (state == null || entity.GetComponent<AnimationComponent>() == null)
            {
                return;
            }
            if (state.CurrentSuperState == SuperState.IsAppearing)
            {
                _entities.Add(entity);
            }
        }

        /// <summary>
        /// Управляет добавлением вновь появляющегося объекта в систему.
        /// </summary>
        /// <param name="message">Сообщение, содержащее объект для добавления.</param>
        public void EntityAppeared(EntityReAppearsMessage message)
        {
            AddEntity(message.Entity);
        }

        /// <summary>
        /// Удаляет сущность из системы.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно удалить.</param>
        public override void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        /// <summary>
        /// Обновляет систему, проверяя наличие сущностей в состоянии IsAppearing и запуская соответствующее действие.
        /// </summary>
        /// <param name="gameTime">Текущее игровое время.</param>
        public override void Update(GameTime gameTime)
        {
            int n = _entities.Count -1;
            if (n == -1)
            {
                //Console.WriteLine("IsEmpty!");  //Убеждаемся, что список пуст после появления сущностей
                return;
            }
            for (int i = n; i >= 0; i--)
            {
                Entity entity = _entities[i];
                if (!entity.IsActive)
                {
                    continue;
                }
                StateComponent state = entity.GetComponent<StateComponent>();
                AnimationComponent animations = entity.GetComponent<AnimationComponent>();

                // Проверяем, находится ли сущность в состоянии IsAppearing
                if (state.CurrentSuperState == SuperState.IsAppearing)
                {
                    ActionAnimation appearAnimation = animations.GetCurrentAnimation();

                    // Проверяем, завершилась ли анимация
                    if (appearAnimation.IsFinished)
                    {
                        state.CanMoveLeft = true;
                        state.CanMoveRight = true;
                        state.CurrentSuperState = state.DefaultSuperState;
                        state.CurrentState = state.DefaultState;

                        RemoveEntity(entity);
                    }
                }
            }
        }
    }
}
