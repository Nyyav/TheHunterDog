using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> отвечает за управление перемещением объектов в игре.
    /// </summary>
    public class MovementSystem : System
    {
        // Список экземпляров EntityData, в которых хранятся ссылки на связанный объект, StateComponent и MovementComponent.
        private List<EntityData> _entitiesData;

        /// <summary>
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MovementSystem">.
        /// </summary>
        public MovementSystem()
        {
            _entitiesData = new List<EntityData>();
        }

        /// <summary>
        /// Добавляет сущность в систему движения.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно добавить.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();

            if (state == null || movement == null)
            {
                return;
            }

            EntityData data = new EntityData
            {
                Entity = entity,
                State = state,
                Movement = movement,
            };

            _entitiesData.Add(data);
        }

        /// <summary>
        /// Удаляет сущность из системы движения.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно удалить.</param>
        public override void RemoveEntity(Entity entity)
        {
            var index = _entitiesData.FindIndex(data => data.Entity == entity);
            if (index != -1)
            {
                _entitiesData.RemoveAt(index);
            }
        }

        /// <summary>
        /// Обновляет систему движения на основе прошедшего игрового времени.
        /// Примечание: Горизонтальные и вертикальные границы не проверяются.
        /// Примечание: Выше, потому что границы рисуются в Tiled и обрабатываются в ObstacleCollisionSystem.
        /// </summary>
        /// <param name="gameTime">Прошедшее игровое время.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in _entitiesData)
            {
                if (!data.Entity.IsActive || data.State.CurrentSuperState == SuperState.IsAppearing)
                {
                    continue;
                }

                UpdatePositionBasedOnState(gameTime, data.Movement, data.State);
                
                CollisionBoxComponent collisionBox = data.Entity.GetComponent<CollisionBoxComponent>();
                if(collisionBox != null)
                {
                    collisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);
                }
            }
        }

        /// <summary>
        /// Обновляет позицию сущности на основе ее состояния, движения и прошедшего игрового времени.
        /// </summary>
        /// <param name="gameTime">Прошедшее игровое время.</param>
        /// <param name="movement">Компонент движения сущности.</param>
        /// <param name="state">Компонент состояния сущности.</param>
        private void UpdatePositionBasedOnState(GameTime gameTime, MovementComponent movement, StateComponent state)
        {
            // Переменные движения
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            VerticalMovement(state, movement);
            HorizontalMovement(state, movement);

            // Обновление скорости
            movement.Velocity += movement.Acceleration * deltaTime;

            // Ограничение скорости
            if (state.CurrentSuperState != SuperState.IsOnGround)
            {
                movement.Velocity = new Vector2(MathHelper.Clamp(movement.Velocity.X, -GameConstants.SpeedX, GameConstants.SpeedX), movement.Velocity.Y);
            }

            // Обновление позиции
            movement.Position += movement.Velocity * deltaTime;

            if(movement.Position.Y >= GameConstants.SCREEN_HEIGHT)
            {
                MessageBus.Publish(new ReloadLevelMessage());
            }
        }

        /// <summary>
        /// Обновляет вертикальную скорость сущности на основе ее состояния.
        /// </summary>
        /// <param name="state">Компонент состояния сущности.</param>
        /// <param name="movement">Компонент движения сущности.</param>
        private void VerticalMovement(StateComponent state, MovementComponent movement)
        {
            // Вертикальное движение
            switch (state.CurrentSuperState)
            {
                case SuperState.IsOnGround:
                    movement.Acceleration = Vector2.Zero;
                    movement.Velocity = Vector2.Zero;
                    if (state.CurrentState == State.Jump)
                    {
                        movement.Velocity = new Vector2(movement.Velocity.X, GameConstants.SpeedY);
                        state.CurrentSuperState = SuperState.IsJumping;
                    }
                    break;

                case SuperState.IsFalling:
                    movement.Acceleration = new Vector2(0, GameConstants.GRAVITY);
                    if(state.CurrentState == State.Slide)
                    {
                        movement.Acceleration = new Vector2(0, GameConstants.GRAVITY/10);
                    }
                    if (state.CurrentState == State.DoubleJump)
                    {
                        movement.Velocity = new Vector2(movement.Velocity.X, GameConstants.SpeedY); 
                        state.CurrentSuperState = SuperState.IsDoubleJumping;
                    }
                    break;

                case SuperState.IsDead:
                    break;

                case SuperState.IsJumping:
                case SuperState.IsDoubleJumping:
                    movement.Acceleration = new Vector2(0, GameConstants.GRAVITY);
                    if (movement.Velocity.Y > 0)
                    {
                        state.CurrentSuperState = SuperState.IsFalling;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Обновляет горизонтальную скорость объекта в зависимости от его состояния.
        /// </summary>
        private void HorizontalMovement(StateComponent state, MovementComponent movement)
        {
            switch (state.CurrentState)
            {
                case State.WalkLeft:
                    state.HorizontalDirection = -1;
                    movement.Velocity += new Vector2(-GameConstants.SpeedX, 0);
                    break;

                case State.WalkRight:
                    state.HorizontalDirection = 1;
                    movement.Velocity += new Vector2(GameConstants.SpeedX, 0); 
                    break;

                default:
                    break;
            }
        }
    }
}