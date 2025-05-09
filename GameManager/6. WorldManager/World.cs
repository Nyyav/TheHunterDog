using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace MonogameExamples
{
    /// <summary>
    /// <summary>
    /// Представляет игровой мир и управляет загрузкой и обновлением уровней.
    /// </summary>
    public class World
    {
        private LevelID _currentLevel;
        private SystemManager _systems;
        private int _totalLevels = Enum.GetValues(typeof(LevelID)).Length;

        private readonly Queue<Entity> _entitiesToDestroy;
        private readonly Queue<Entity> _entitiesToAdd;
        private bool _levelNeedsChanging;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="World">.
        /// </summary>
        public World()
        {
            _systems = new SystemManager(CurrentLevel);
            _entitiesToDestroy = new Queue<Entity>();
            _entitiesToAdd  = new Queue<Entity>();

            MessageBus.Subscribe<AddEntityMessage>(OnCreateEntity);
            MessageBus.Subscribe<DestroyEntityMessage>(OnDestroyEntity);
            MessageBus.Subscribe<NextLevelMessage>(NextLevel);
            MessageBus.Subscribe<PreviousLevelMessage>(PreviousLevel);
            MessageBus.Subscribe<ReloadLevelMessage>(ResetCurrentLevel);

            ChangeLevel(LevelID.Level1);
        }

        /// <summary>
        /// Возвращает текущий уровень.
        /// </summary>
        public LevelID CurrentLevel
        {
            get { return _currentLevel; }
        }

        /// <summary>
        /// Изменяет текущий уровень на указанный.
        /// </summary>
        /// <param name="level">Уровень, на который нужно перейти.</param>
        private void ChangeLevel(LevelID level)
        {
            _currentLevel = level;
            _levelNeedsChanging = true;
        }

        /// <summary>
        /// Загружает текущий уровень и его сущности в игровой мир.
        /// </summary>
        private void LoadLevel()
        {
            MediaPlayer.Stop();
            _systems.Unsubscribe();
            _systems.ResetSystems(CurrentLevel);
            _systems.Subscribe();
            LevelLoader.GetObjects(CurrentLevel);
            //Loader.PlayMusic(BackgroundMusic.Default, true);
        }

        /// <summary>
        /// Перезагружает текущий уровень.
        /// </summary>
        /// <param name="message">Необязательный параметр сообщения.</param>
        public void ResetCurrentLevel(ReloadLevelMessage message = null)
        {
            _levelNeedsChanging = true; 
        }

        /// <summary>
        /// Переходит на следующий уровень.
        /// </summary>
        /// <param name="message">Необязательный параметр сообщения.</param>
        public void NextLevel(NextLevelMessage message = null)
        {
            _currentLevel = (LevelID)(((int)_currentLevel + 1 + _totalLevels) % _totalLevels);
            _levelNeedsChanging = true;
        }

        /// <summary>
        /// Возвращается к предыдущему уровню.
        /// </summary>
        public void PreviousLevel(PreviousLevelMessage message = null)
        {
            _currentLevel = (LevelID)(((int)_currentLevel - 1 + _totalLevels) % _totalLevels);
            _levelNeedsChanging = true;
        }

        /// <summary>
        /// <summary>
        /// Обрабатывает добавление сущностей в системы.
        /// </summary>
        /// <param name="message">Сообщение, содержащее сущность для добавления.</param>
        private void OnCreateEntity(AddEntityMessage message) // Добавляем этот метод
        {
            _entitiesToAdd.Enqueue(message.Entity);
        }

        /// <summary>
        /// Обрабатывает уничтожение сущностей.
        /// </summary>
        /// <param name="message">Сообщение, содержащее сущность для уничтожения.</param>
        private void OnDestroyEntity(DestroyEntityMessage message)
        {
            _entitiesToDestroy.Enqueue(message.Entity);
        }

        /// <summary>
        /// Обновляет игровой мир.
        /// </summary>
        /// <param name="gameTime">Предоставляет снимок значений времени.</param>
        public void Update(GameTime gameTime)
        {
            if(_levelNeedsChanging)
            {
                LoadLevel();
                _levelNeedsChanging = false;
            }

            while (_entitiesToAdd.Count > 0)
            {
                Entity entity = _entitiesToAdd.Dequeue();
                _systems.Add(entity);
            }

            _systems.Update(gameTime);

            while (_entitiesToDestroy.Count > 0)
            {
                Entity entity = _entitiesToDestroy.Dequeue();
                _systems.Remove(entity);
            }
        }

        /// <summary>
        /// Рисует игровой мир.
        /// </summary>
        /// <param name="spriteBatch">Используется для рисования.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            _systems.Draw(spriteBatch);
        }
    }
}
