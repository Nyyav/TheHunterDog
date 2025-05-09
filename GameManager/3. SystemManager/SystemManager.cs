using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonogameExamples
{

    /// <summary>
    /// <summary>
    /// Управляет коллекцией классов <see cref="System"/> и предоставляет методы для добавления, удаления, обновления и рисования сущностей через них.
    /// </summary>
    public struct SystemManager
    {
        private List<System> _systems = new List<System>();

        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="SystemManager"/>.
        /// </summary>
        /// <param name="LevelID">Идентификатор уровня.</param>
        public SystemManager(LevelID levelID)
        {
            ResetSystems(levelID);
        }

        /// <summary>
        /// Сбрасывает диспетчер систем, используется при каждом изменении/перезагрузке уровня.
        /// </summary>
        /// <param name="LevelID">Идентификатор уровня.</param>
        public void ResetSystems(LevelID levelID)
        {
            _systems = new List<System>();

            // Системы ввода
            _systems.Add(new PlayerInputSystem());
            _systems.Add(new RegularEnemyInputSystem());

            // Системы игровой логики
            _systems.Add(new MovementSystem());
            _systems.Add(new PlayerEntityCollisionSystem());
            _systems.Add(new ObstacleCollisionSystem(levelID));
            _systems.Add(new RespawnSystem());
            _systems.Add(new AppearSystem());

            // Системы рендеринга
            _systems.Add(new LevelRenderSystem(levelID));
            _systems.Add(new TimerSystem());
            _systems.Add(new AnimationRenderSystem());

            // Смерть
            _systems.Add(new DeathSystem());

        }


        /// <summary>
        /// Добавляет сущность во все системы.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно добавить.</param>
        public void Add(Entity entity)
        {
            foreach (System system in _systems)
            {
                system.AddEntity(entity);
            }
        }

        /// <summary>
        /// Удаляет сущность из всех систем.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно удалить.</param>
        public void Remove(Entity entity)
        {
            foreach (System system in _systems)
            {
                system.RemoveEntity(entity);
            }
        }

        /// <summary>
        /// <summary>
        /// Подписывает все системы на их события MessageBus.
        /// </summary>
        public void Subscribe()
        {
            foreach (System system in _systems)
            {
                system.Subscribe();
            }
        }

        /// <summary>
        /// Отписывает все системы от их событий MessageBus.
        /// </summary>
        public void Unsubscribe()
        {
            foreach (System system in _systems)
            {
                system.Unsubscribe();
            }
        }

        /// <summary>
        /// Обновляет все системы с указанным <see cref="GameTime"/>.
        /// </summary>
        /// <param name="gameTime">Время, прошедшее с момента последнего обновления.</param>
        public void Update(GameTime gameTime)
        {
            foreach (System system in _systems)
            {
                system.Update(gameTime);
            }
        }

        /// <summary>
        /// Рисует все сущности, управляемые системами, с указанным <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Пакет спрайтов, используемый для рисования сущностей.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (System system in _systems)
            {
                system.Draw(spriteBatch);
            }
        }
    }
}
