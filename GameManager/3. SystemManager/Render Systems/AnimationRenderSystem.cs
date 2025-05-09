using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> отвечает за рендеринг объектов в игре.
    /// </summary>
    public class AnimationRenderSystem : System
    {
        private List<EntityData> _entitiesData;

        /// <summary>
        /// Инициализирует новый экземпляр<see cref="AnimationRenderSystem"/> класса.
        /// </summary>
        public AnimationRenderSystem()
        {
            _entitiesData = new List<EntityData>();
        }

        /// <summary>
        /// Добавляет объект в систему визуализации.
        /// </summary>
        /// <param name="entity">Объект, который будет добавлен.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            AnimationComponent animations = entity.GetComponent<AnimationComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();

            if (state == null || animations == null || movement == null)
            {
                return;
            }

            EntityData data = new EntityData
            {
                Entity = entity,
                State = state,
                Animations = animations,
                Movement = movement,
            };

            _entitiesData.Add(data);
        }

        /// <summary>
        /// Удаляет объект из системы визуализации.
        /// </summary>
        /// <param name="entity">Объект, подлежащий удалению.</param>
        public override void RemoveEntity(Entity entity)
        {
            var index = _entitiesData.FindIndex(data => data.Entity == entity);
            if (index != -1)
            {
                _entitiesData.RemoveAt(index);
            }
        }

        /// <summary>
        /// Обновляет систему рендеринга в зависимости от затраченного игрового времени.
        /// </summary>
        /// <param name="gameTime">Истекшее игровое время.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in _entitiesData)
            {
                if(!data.Entity.IsActive)
                {
                    continue;
                }
                GetAnimationForState(data.State, data.Animations);
                data.Animations.Update(gameTime);
            }
        }

        /// <summary>
        /// Рисует объекты в системе рендеринга.
        /// </summary>
        /// <param name="spriteBatch"> используется для рисования.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var data in _entitiesData)
            {
                if(!data.Entity.IsActive)
                {
                    continue;
                }
                data.Animations.Draw(spriteBatch, data.Movement.Position, data.State.HorizontalDirection);
            }
        }

        /// <summary>
        /// Устанавливает текущую анимацию для объекта на основе его текущего состояния.
        /// </summary>
        /// <param name="state">Компонент состояния сущности.</param>
        /// <param name="animations">Анимированный компонент объекта.</param>
        private void GetAnimationForState(StateComponent state, AnimationComponent animations)
        {
            animations.SetCurrentAction(state.AnimationState);
        }
    }
}
