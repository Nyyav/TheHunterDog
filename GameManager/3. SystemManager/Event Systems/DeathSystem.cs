using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> которая управляет событиями смерти объекта, запуская действия в зависимости от типа объекта.
    /// </summary>
    public class DeathSystem : System
    {
        private List<Entity> _entities;

        /// <summary>
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DeathSystem"/>.
        /// </summary>
        public DeathSystem()
        {
            _entities = new List<Entity>();
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
        /// Подписывается на соответствующие сообщения.
        /// </summary>
        public override void Subscribe()
        {
            MessageBus.Subscribe<EntityDiedMessage>(EntityDied);
        }

        /// <summary>
        /// Отписывается от всех сообщений.
        /// </summary>
        public override void Unsubscribe()
        {
            MessageBus.Unsubscribe<EntityDiedMessage>(EntityDied);
        }

        /// <summary>
        /// Обновляет систему, проверяя наличие сущностей в состоянии смерти и запуская соответствующее действие.
        /// </summary>
        /// <param name="gameTime">Текущее игровое время.</param>
        public override void Update(GameTime gameTime)
        {
            int n = _entities.Count - 1;
            for (int i = n; i >= 0; i--)
            {
                Entity entity = _entities[i];
                if (!entity.IsActive)
                {
                    continue;
                }

                StateComponent stateComponent = entity.GetComponent<StateComponent>(); // Не может быть null
                AnimationComponent animatedComponent = entity.GetComponent<AnimationComponent>(); // Не может быть null
                RespawnComponent canBeRespawned = entity.GetComponent<RespawnComponent>();
                EntityTypeComponent entityType = entity.GetComponent<EntityTypeComponent>();

                if (stateComponent.CurrentSuperState != SuperState.IsDead)
                {
                    continue;
                }

                ActionAnimation deathAnimation = animatedComponent.GetCurrentAnimation();
                if (!deathAnimation.IsFinished)
                {
                    continue;
                }

                // Запускаем событие смерти
                if (entityType != null && entityType.Type == EntityType.Player)
                {
                    MessageBus.Publish(new DestroyEntityMessage(entity));
                    MessageBus.Publish(new ReloadLevelMessage());
                }
                else if (canBeRespawned != null)
                {
                    entity.IsActive = false;
                    deathAnimation.Reset();
                    canBeRespawned.StartRespawn();
                }
                else
                {
                    MessageBus.Publish(new DestroyEntityMessage(entity));
                }
            }
        }

        /// <summary>
        /// Добавляет объекты в систему
        /// </summary>
        /// <param name="message">Сообщение, содержащее объект для добавления.</param>
        private void EntityDied(EntityDiedMessage message)
        {
            _entities.Add(message.Entity);
        }
    }
}