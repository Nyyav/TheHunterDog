using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/>которая обрабатывает коллизии между объектом player и другими объектами.
    /// </summary>
    public class PlayerEntityCollisionSystem : System
    {
        private List<EntityData> _entitiesData;
        private EntityData _playerData;

        /// <summary>
        /// Создает экземпляр <see cref="PlayerEntityCollisionSystem"/> класса.
        /// </summary>
        public PlayerEntityCollisionSystem()
        {
            _entitiesData = new List<EntityData>();
            _playerData = new EntityData();
        }

        /// <summary>
        /// Добавляет объект в систему.
        /// </summary>
        /// <param name="entity"> Сущность, которую нужно добавить.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            CollisionBoxComponent collisionBox = entity.GetComponent<CollisionBoxComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();

            if (state == null || collisionBox == null || movement == null)
            {
                return;
            }

            var data = new EntityData
            {
                Entity = entity,
                State = state,
                CollisionBox = collisionBox,
                Movement = movement,
            };

            if (entity.GetComponent<EntityTypeComponent>().Type == EntityType.Player)
            {
                _playerData = data;
            }
            else
            {
                _entitiesData.Add(data);
            }
        }

        /// <summary>
        /// Удаляет объект из системы.
        /// </summary>
        /// <param name="entity">Сущность, которую удаляем.</param>
        public override void RemoveEntity(Entity entity)
        {
            if (entity.GetComponent<EntityTypeComponent>().Type == EntityType.Player)
            {
                _playerData = new EntityData();
            }
            else
            {
                _entitiesData.RemoveAll(data => data.Entity == entity);
            }
        }

        /// <summary>
        /// Обрабатывает столкновения между объектом и препятствиями на всех уровнях.
        /// </summary>
        /// <param name="gameTime">Текущее время игры.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (EntityData data in _entitiesData)
            {
                if (_playerData.State.CurrentSuperState == SuperState.IsDead || _playerData.State.CurrentSuperState == SuperState.IsAppearing || !_playerData.Entity.IsActive)
                {
                    return;
                }
                if (!data.Entity.IsActive || data.State.CurrentSuperState == SuperState.IsAppearing || data.State.CurrentSuperState == SuperState.IsDead)
                {
                    continue;
                }


                // Проверка не сталкиваются ли эти два объекта
                if (_playerData.CollisionBox.GetRectangle().Intersects(data.CollisionBox.GetRectangle()))
                {
                    // В зависимости от типов объектов определяется подходящее решение для устранения коллизий
                    EntityTypeComponent entityType = data.Entity.GetComponent<EntityTypeComponent>();
                    switch (entityType.Type)
                    {
                        case EntityType.Coin:
                            ResolveCoinCollision(_playerData, data);
                            break;
                        case EntityType.RegularEnemy:
                            ResolveRegularEnemyCollision(_playerData, data);
                            break;
                        case EntityType.PortalToNextLevel:
                            ResolveNextLevelCollision(_playerData, data);
                            break;
                        //здесь можно добавить больше кейсов
                        default:
                            break;
                    }

                    //Обновление поля коллизий
                    _playerData.CollisionBox.UpdateBoxPosition(_playerData.Movement.Position.X, _playerData.Movement.Position.Y, _playerData.State.HorizontalDirection);
                    data.CollisionBox.UpdateBoxPosition(data.Movement.Position.X, data.Movement.Position.Y, data.State.HorizontalDirection);
                }
            }
        }

        /// <summary>
        ///Устраняет коллизии между игроком и монетным предметом.
        /// </summary>
        /// <param name="playerData">Данные для объекта player.</param>
        /// <param name="coinData">Данные для объекта coin(фрукты, или предметы).</param>
        private void ResolveCoinCollision(EntityData playerData, EntityData coinData)
        {
            // Mark the coin as dead
            coinData.State.CurrentSuperState = SuperState.IsDead;
            MessageBus.Publish(new EntityDiedMessage(coinData.Entity));
        }

        /// <summary>
        /// Устраняет столкновения между игроком и портальным объектом, которые ведут на следующий уровень.
        /// </summary>
        /// <param name="playerData">Данные для объекта player.</param>
        /// <param name="portalData">Данные для объекта портала.</param>
        private void ResolveNextLevelCollision(EntityData playerData, EntityData portalData)
        {
            // Выводит сообщение о том, что игрок достиг следующего уровня?
            MessageBus.Publish(new NextLevelMessage());
        }

        /// <summary>
        /// Разрешает столкновения между игроком и обычным врагом, который ходит по земле.
        /// </summary>
        /// <param name="player">Данные для объекта player.</param>
        /// <param name="enemy">Данные для объекта enemy.</param>
        private void ResolveRegularEnemyCollision(EntityData player, EntityData enemy)
        {

            // Если игрок падает, убейте врага и переведите его в состояние ожидания
            switch (player.State.CurrentSuperState)
            {
                case SuperState.IsFalling:
                    int direction = player.State.HorizontalDirection;
                    float positionX = player.Movement.Position.X;
                    float positionY = player.Movement.Position.Y;

                    bool properHit = HandleFallCollision(player, player.CollisionBox.GetRectangle(), enemy.CollisionBox.GetRectangle(), ref positionX, ref positionY);
                    if (!properHit)
                    {
                        break;
                    }

                    enemy.Movement.Velocity = new Vector2(0, -20);
                    enemy.Movement.Acceleration = new Vector2(0, 100);
                    enemy.State.CurrentSuperState = SuperState.IsDead;
                    enemy.State.CurrentState = State.Idle;

                    player.Movement.Position = new Vector2(positionX, positionY);
                    player.Movement.Velocity = new Vector2(GameConstants.SpeedXonCollision * direction, player.Movement.Velocity.Y - GameConstants.SpeedYonCollision);
                    player.State.CurrentSuperState = SuperState.IsJumping;
                    player.State.CurrentState = State.Idle;
                    player.State.JumpsPerformed = 1;

                    MessageBus.Publish(new EntityDiedMessage(enemy.Entity));
                    break;

                default:
                    player.State.CurrentSuperState = SuperState.IsDead;
                    player.State.CurrentState = State.Idle;
                    player.Movement.Velocity = new Vector2(0, GameConstants.SpeedY / 2);
                    player.Movement.Acceleration = new Vector2(0, GameConstants.GRAVITY / 2);

                    MessageBus.Publish(new EntityDiedMessage(player.Entity));
                    break;
            }
        }

        //Вспомогательные методы

        /// <summary>
        /// Обрабатывает столкновение, когда объект находится в падающем состоянии.
        /// </summary>
        /// <param name="data">Содержит данные сущности.</param>
        /// <param name="box">Поле коллизии объекта.</param>
        /// <param name="rect">The obstacle's rectangle.</param>
        /// <param name="positionX">Текущая позиция по X.</param>
        /// <param name="positionY">Текущая позиция по Y.</param>
        /// <returns>Возвращает значение true, если объект столкнулся с верхней стороной препятствия.</returns>
        private bool HandleFallCollision(EntityData data, Rectangle box, Rectangle rect, ref float positionX, ref float positionY)
        {
            bool wasAbove = data.Movement.LastPosition.Y + data.CollisionBox.OriginalHeight - data.CollisionBox.VertBottomOffset <= rect.Top + 1;
            if (!wasAbove)
            {
                return false;
            }

            positionY = rect.Top - data.CollisionBox.OriginalHeight + data.CollisionBox.VertBottomOffset - 0.1f;
            data.Movement.Velocity = Vector2.Zero;
            data.Movement.Acceleration = Vector2.Zero;
            return true;
        }

    }
}