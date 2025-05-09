using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// <summary>
    /// Класс для создания сущностей.
    /// </summary>
    static class EntityFactory
    {
        /// <summary>
        /// Создает сущность игрока.
        /// </summary>
        /// <param name="position">Начальная позиция игрока.</param>
        /// <returns>Сущность игрока.</returns>
        public static void CreatePlayer(Vector2 position)
        {
            // Пустой игрок
            Entity player = new Entity();
            player.AddComponent(new EntityTypeComponent(EntityType.Player));

            // Анимации
            AnimationComponent animation = new AnimationComponent();
            animation.AddAnimation(PlayerTexture.Idle, AnimationID.Idle, 1, 11, 20);
            animation.AddAnimation(PlayerTexture.Walking, AnimationID.Walk, 1, 5, 20);
            animation.AddAnimation(PlayerTexture.Jump, AnimationID.Jump, 1, 1, 20);
            animation.AddAnimation(PlayerTexture.DoubleJump, AnimationID.DoubleJump, 1, 1, 20);
            animation.AddAnimation(PlayerTexture.Fall, AnimationID.Fall, 1, 1, 20);
            animation.AddAnimation(PlayerTexture.Slide, AnimationID.Slide, 1, 1, 20);
            animation.AddAnimation(PlayerTexture.Hit, AnimationID.Death, 1, 4, 20);
            animation.AddAnimation(FruitTexture.Collected, AnimationID.Appear, 1, 6, 20);
            player.AddComponent(animation);

            // Состояния
            player.AddComponent(new StateComponent());
            player.AddComponent(new PlayerInputComponent());

            // Позиция и преобразования
            player.AddComponent(new MovementComponent(position));

            // Коллизии
            player.AddComponent(new CollisionBoxComponent(
                    position: position,
                    width: 32,
                    height: 16,
                    vertTopOffset: 8,
                    vertBottomOffset: -6,
                    horLeftOffset: 4,
                    horRightOffset: 6));

            MessageBus.Publish(new AddEntityMessage(player));
        }

        public static void CreateFruit(Vector2 position, string texture, float respawnTime  = 5)
        {
            // Создаем пустую сущность Coin
            Entity coin = new Entity();
            coin.AddComponent(new EntityTypeComponent(EntityType.Coin));

            // Добавляем анимации для фрукта
            AnimationComponent animation = new AnimationComponent();
            Enum.TryParse(texture, out FruitTexture textureKey);
            animation.AddAnimation(textureKey, AnimationID.Idle, 1, 17, 20);
            animation.AddAnimation(FruitTexture.Collected, AnimationID.Death, 1, 6, 20);
            animation.AddAnimation(FruitTexture.Collected, AnimationID.Appear, 1, 6, 20);
            coin.AddComponent(animation);

            /* Добавляем текущее состояние и 
            супер-состояние (оно описывает общую ситуацию, в которой находится 
            сущность (например, на земле, в воздухе, мертв, в процессе появления) для фрукта*/

            coin.AddComponent(new StateComponent(State.Idle, SuperState.IsOnGround));

            // Добавляем компонент движения для установки начальной позиции
            coin.AddComponent(new MovementComponent(position));

            // Добавляем компонент коллизионного бокса для обработки столкновений
            coin.AddComponent(new CollisionBoxComponent(
                    position: position,
                    width: 32,
                    height: 32,
                    vertTopOffset: 7,
                    vertBottomOffset: 11,
                    horLeftOffset: 10,
                    horRightOffset: 10));

            // Добавляем компонент возрождения
            coin.AddComponent(new RespawnComponent(position, respawnTime));

            MessageBus.Publish(new AddEntityMessage(coin));
            }

            public static void CreateRegularEnemy(Vector2 position, float leftRange, float rightRange, State initialDirection, float respawnTime  = 5)
            {
                // Создаем пустую сущность врага
                Entity enemy = new Entity();
                enemy.AddComponent(new EntityTypeComponent(EntityType.RegularEnemy));

                // Добавляем анимации для врага
                AnimationComponent animation = new AnimationComponent(AnimationID.Walk);
                animation.AddAnimation(MaskedEnemyTexture.Walking, AnimationID.Walk, 1, 9, 20);
                animation.AddAnimation(MaskedEnemyTexture.Hit, AnimationID.Death, 1, 4, 20);
                animation.AddAnimation(FruitTexture.Collected, AnimationID.Appear, 1, 6, 20);
                enemy.AddComponent(animation);

                // Добавляем текущее состояние и супер-состояние для врага
                enemy.AddComponent(new StateComponent(initialDirection));
                enemy.AddComponent(new RegularEnemyComponent(position.X, leftRange, rightRange));

                // Добавляем компонент движения для установки начальной позиции
                enemy.AddComponent(new MovementComponent(position));

                // Добавляем компонент бокса колизии для обработки столкновений
                enemy.AddComponent(new CollisionBoxComponent(
                        position: position,
                        width: 32,
                        height: 16,
                        vertTopOffset: 8,
                        vertBottomOffset: -8,
                        horLeftOffset: 4,
                        horRightOffset: 6));

                // Добавляем компонент возрождения
                enemy.AddComponent(new RespawnComponent(position, respawnTime));
                MessageBus.Publish(new AddEntityMessage(enemy));
            }

            public static void CreateTimer(Vector2 position, int duration, bool isActive)
            {
                // Создаем пустую сущность таймера
            Entity timer = new Entity(isActive);
            MessageBus.Publish(new GameTimerMessage(timer, duration, position));
        }
    }
}
