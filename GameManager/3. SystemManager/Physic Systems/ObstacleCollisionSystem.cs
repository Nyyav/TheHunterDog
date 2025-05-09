using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> которая управляет обнаружением и разрешением столкновений между объектами и уровневыми препятствиями.
    /// </summary>
    public class ObstacleCollisionSystem : System
    {
        private List<EntityData> _entitiesData;
        /// <summary>
        /// Словарь, содержащий данные о препятствиях для каждого слоя игровой среды.
        /// </summary>
        private Dictionary<string, List<Rectangle>> _obstacles;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref= "ObstacleCollisionSystem"/> класса.
        /// </summary>
        /// <param name="LevelID">Идентификатор данных о препятствиях на уровне.</param>

        public ObstacleCollisionSystem(LevelID levelID)
        {
            _entitiesData = new List<EntityData>();
            _obstacles = new Dictionary<string, List<Rectangle>>();
            _obstacles = Loader.tiledHandler.objects[levelID.ToString()];
        }

        /// <summary>
        /// Добавляет объект в систему.
        /// </summary>
        /// <param name="entity"> Сущность, которую нужно добавить.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            CollisionBoxComponent collisionBox = entity.GetComponent<CollisionBoxComponent>();

            if (state == null || movement == null || collisionBox == null)
            {
                return;
            }

            _entitiesData.Add(new EntityData
            {
                Entity = entity,
                State = state,
                Movement = movement,
                CollisionBox = collisionBox,
            });
        }

        /// <summary>
        /// Удаляет объект из системы.
        /// </summary>
        /// <param name="entity">Сущность, которую удаляем.</param>
        public override void RemoveEntity(Entity entity)
        {
            _entitiesData.RemoveAll(data => data.Entity == entity);
        }

        /// <summary>
        /// Обрабатывает столкновения между объектом и препятствиями на всех уровнях.
        /// </summary>
        /// <param name="gameTime">Текущее время игры.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in _entitiesData)
            {
                if (!data.Entity.IsActive || data.State.CurrentSuperState == SuperState.IsAppearing || data.State.CurrentSuperState == SuperState.IsDead)
                {
                    continue;
                }

                Rectangle box = data.CollisionBox.GetRectangle();
                float positionX = data.Movement.Position.X;
                float positionY = data.Movement.Position.Y;
                data.State.CanMoveRight = true;
                data.State.CanMoveLeft = true;

                foreach (string key in _obstacles.Keys)
                {
                    foreach (Rectangle rect in _obstacles[key])
                    {
                        if (!box.Intersects(rect))
                        {
                            continue;
                        }

                        switch (data.State.CurrentSuperState)
                        {
                            case SuperState.IsFalling:
                                HandleFallCollision(data, box, rect, ref positionX, ref positionY, key);
                                break;

                            case SuperState.IsOnGround:
                                HandleGroundCollision(data, box, rect, ref positionX);
                                break;

                            case SuperState.IsJumping:
                            case SuperState.IsDoubleJumping:
                                if (key == "float")
                                {
                                    break;
                                }

                                HandleJumpCollision(data, box, rect, ref positionX, ref positionY);
                                break;

                            default:
                                break;
                        }
                    }
                }

                //Обновить позицию объекта и поле столкновения
                data.Movement.Position = new Vector2(positionX, positionY);
                data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);

                //Проверка не находится ли объект больше на платформе
                if (data.CollisionBox.CheckIfInAir(data.Movement.Position.X, data.State.HorizontalDirection))
                {
                    if (data.State.CurrentSuperState == SuperState.IsOnGround)
                    {
                        data.State.CurrentSuperState = SuperState.IsFalling;
                    }
                }
                //Проверка не скользит ли объект больше
                if (data.CollisionBox.CheckIfBelow(data.Movement.Position.Y))
                {
                    if (data.State.CurrentState == State.Slide)
                    {
                        data.State.CurrentState = State.Idle;
                    }
                }
            }
        }

        /// <summary>
        /// Обрабатывает столкновение, когда объект находится в падающем состоянии.
        /// </summary>
        /// <param name="data">Содержит компоненты сущности.</param>
        /// <param name="box">Поле коллизии объекта.</param>
        /// <param name="rect">The obstacle's rectangle.</param>
        /// <param name="positionX">Текущая позиция по X.</param>
        /// <param name="positionY">Текущая позиция по Y.</param>
        private void HandleFallCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX, ref float positionY, string key)
        {
            data.State.CurrentState = State.Idle;
            bool wasAbove = data.Movement.LastPosition.Y + data.CollisionBox.OriginalHeight - data.CollisionBox.VertBottomOffset <= rect.Top + 1;

            if (wasAbove)
            {
                data.State.CurrentSuperState = SuperState.IsOnGround;
                positionY = rect.Top - data.CollisionBox.OriginalHeight + data.CollisionBox.VertBottomOffset - 0.1f;
                data.CollisionBox.SetGroundLocation(rect.Left, rect.Right);
            }
            else if (key != "float")
            {
                HandleHorizontalInAirCollision(data, box, rect, ref positionX);
            }
        }

        /// <summary>
        /// Обрабатывает столкновение, когда объект находится в переходном состоянии.
        /// </summary>
        private void HandleJumpCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX, ref float positionY)
        {
            bool wasBelow = data.Movement.LastPosition.Y + data.CollisionBox.VertTopOffset >= rect.Bottom - 1;
            if (wasBelow)
            {
                positionY = rect.Bottom - data.CollisionBox.VertTopOffset + 0.1f;
                data.State.CurrentSuperState = SuperState.IsFalling;
                data.Movement.Velocity = Vector2.Zero;
            }
            else
            {
                HandleHorizontalInAirCollision(data, box, rect, ref positionX);
            }
        }

        /// <summary>
        /// Обрабатывает столкновение, когда объект находится на земле.
        /// </summary>

        private void HandleGroundCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX)
        {

            if (data.State.CurrentState == State.Slide)
            {
                data.State.CurrentState = State.Idle;
            }

            if (data.Movement.Velocity.X > 0 && box.Left <= rect.Left)
            {
                positionX = rect.Left - data.CollisionBox.OriginalWidth + data.CollisionBox.HorRightOffset - 0.1f;
                data.State.CanMoveRight = false;
            }
            if (data.Movement.Velocity.X < 0 && box.Right >= rect.Right)
            {
                positionX = rect.Right - data.CollisionBox.HorRightOffset + 0.1f;
                data.State.CanMoveLeft = false;
            }
        }

        /// <summary>
        /// Обрабатывает горизонтальное столкновение, когда объект находится в воздухе (прыгает или падает).
        /// </summary>
        private void HandleHorizontalInAirCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX)
        {

            if (data.Movement.Velocity.X > 0 && box.Left <= rect.Left)
            {
                positionX = rect.Left - data.CollisionBox.OriginalWidth + data.CollisionBox.HorRightOffset;
                data.State.CanMoveRight = false;
                data.CollisionBox.SetSlidingLocation(rect.Bottom);
                data.State.CurrentSuperState = SuperState.IsFalling;
                data.Movement.Velocity = Vector2.Zero;
            }
            if (data.Movement.Velocity.X < 0 && box.Right >= rect.Right)
            {
                positionX = rect.Right - data.CollisionBox.HorRightOffset;
                data.State.CanMoveLeft = false;
                data.CollisionBox.SetSlidingLocation(rect.Bottom);
                data.State.CurrentSuperState = SuperState.IsFalling;
                data.Movement.Velocity = Vector2.Zero;
            }

        }

        /// <summary>
        /// Отображает состояние системы с помощью предоставленного SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Используется для рисования.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!GameConstants.DisplayCollisionBoxes)
            {
                return;
            }
            foreach (EntityData data in _entitiesData)
            {
                Texture2D collisionBox = Loader.collisionBox;

                var color = Color.Orange;
                spriteBatch.Draw(collisionBox, data.CollisionBox.GetRectangle(), color);
            }
        }
    }
}