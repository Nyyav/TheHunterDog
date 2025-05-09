using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// <summary>
    /// Система, которая управляет возрождением сущностей на основе RespawnComponent.
    /// </summary>
    public class RespawnSystem : System
    {
        private List<Entity> _entities;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RespawnSystem"/>.
        /// </summary>
        public RespawnSystem()
        {
            _entities = new List<Entity>();
        }

        /// <summary>
        /// Добавляет сущность в систему, если у нее есть RespawnComponent.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно добавить.</param>
        public override void AddEntity(Entity entity)
        {
            if (entity.GetComponent<RespawnComponent>() != null)
            {
                _entities.Add(entity);
            }
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
        /// Обновляет систему, проверяя наличие сущностей, которые необходимо возродить.
        /// </summary>
        /// <param name="gameTime">Текущее игровое время.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (Entity entity in _entities)
            {
                RespawnComponent respawn = entity.GetComponent<RespawnComponent>();

                if (!respawn.IsRespawning || entity.IsActive)
                {
                    continue;
                }

                respawn.Update(gameTime);

                if (respawn.IsRespawning)
                {
                    continue;
                }

                StateComponent state = entity.GetComponent<StateComponent>();
                MovementComponent movement = entity.GetComponent<MovementComponent>();
                CollisionBoxComponent collision = entity.GetComponent<CollisionBoxComponent>();

                // Для любой сущности компонент состояния всегда должен существовать
                state.CurrentSuperState = SuperState.IsAppearing;
                state.HorizontalDirection = state.DefaultHorizontalDirection;

                if (movement != null)
                {
                    movement.Position = respawn.Position;
                    if (collision != null)
                    {
                        collision.UpdateBoxPosition(respawn.Position.X, respawn.Position.Y, state.HorizontalDirection);
                    }
                }

                entity.IsActive = true;
                MessageBus.Publish(new EntityReAppearsMessage(entity));
            }
        }

    }
}
