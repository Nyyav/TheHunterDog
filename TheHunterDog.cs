using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// Основной игровой класс.
    /// </summary>
    public class TheHunterDog : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private Rectangle _destinationRenderRectangle = new Rectangle();
        private KeyboardState _previousKeyboardState;
        private bool _isPaused = false;

        public World world;

        /// <summary>
        /// Инициализация класса TheHunterDog.
        /// </summary>
        public TheHunterDog()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = false;
        }

        /// <summary>
        /// Инициализация игрового окна и установка желаемого разрешение.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "The Hunter Dog";

            // Изменить разрешение
            _graphics.PreferredBackBufferWidth = GameConstants.SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = GameConstants.SCREEN_HEIGHT;

            // Установить полноэкранный режим
            _graphics.IsFullScreen = GameConstants.FULL_SCREEN;


            // false чтобы использовать полноэкранный оконный режим без рамки, который масштабирует разрешение игры.
            // true чтобы использовать эксклюзивный полноэкранный режим, который изменяет разрешение экрана в соответствии с разрешением игры.
            _graphics.HardwareModeSwitch = false; 

            _graphics.ApplyChanges();

            // Ограничение FPS (кадров в секунду)
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / GameConstants.PhysicFPS);

            base.Initialize();
        }

        /// <summary>
        /// Загрузка игрового контента.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _renderTarget = new RenderTarget2D(GraphicsDevice, GameConstants.SCREEN_WIDTH, GameConstants.SCREEN_HEIGHT);

            // Загрузка ресурсов
            Loader.LoadContent(GraphicsDevice);

            // Инициализация мира
            world = new World();
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="gameTime">Текущее время игры.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            // Переключение полноэкранного режима по нажатию F11
            if (currentKeyboardState.IsKeyDown(Keys.F11) && _previousKeyboardState.IsKeyUp(Keys.F11))
            {
                GameConstants.FULL_SCREEN = !GameConstants.FULL_SCREEN; // Обновляем константу

                _graphics.IsFullScreen = GameConstants.FULL_SCREEN;    // Устанавливаем полноэкранный режим

                // Применяем изменения
                _graphics.ApplyChanges();
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && _previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                if (_isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }

            if (!_isPaused)
            {
                // Обрабатываем ввод игрока и обновляем игровую логику
                if (currentKeyboardState.IsKeyDown(Keys.R) && _previousKeyboardState.IsKeyUp(Keys.R))
                {
                    MessageBus.Publish(new ReloadLevelMessage());
                }
                else if (currentKeyboardState.IsKeyDown(Keys.P) && _previousKeyboardState.IsKeyUp(Keys.P))
                {
                    world.PreviousLevel();
                }
                else if (currentKeyboardState.IsKeyDown(Keys.N) && _previousKeyboardState.IsKeyUp(Keys.N))
                {
                    MessageBus.Publish(new NextLevelMessage());
                }

                world.Update(gameTime);
            }

            _previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        /// <summary>
        /// Рисует игру.
        /// </summary>
        /// <param name="gameTime">Текущее игровое время.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (GameConstants.FULL_SCREEN)
            {
                // Устанавливаем цель рендеринга для рисования игрового контента
                GraphicsDevice.SetRenderTarget(_renderTarget);

                // Рисуем игровой контент
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                world.Draw(_spriteBatch);
                _spriteBatch.End();

                // Возвращаем цель рендеринга обратно к значению по умолчанию
                GraphicsDevice.SetRenderTarget(null);
                // Масштабируем окно
                UpdateScaling();

                // Рисуем текстуру цели рендеринга по центру и масштабированно по экрану
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                _spriteBatch.Draw(_renderTarget, _destinationRenderRectangle, Color.White);
                _spriteBatch.End();
            }
            else
            {
                // Рисуем игровой контент
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                world.Draw(_spriteBatch);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void UpdateScaling()
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / GameConstants.SCREEN_WIDTH;
            float scaleY = (float)GraphicsDevice.Viewport.Height / GameConstants.SCREEN_HEIGHT;
            float scaleFactor = Math.Min(scaleX, scaleY);
            int screenWidth = (int)(GameConstants.SCREEN_WIDTH * scaleFactor);
            int screenHeight = (int)(GameConstants.SCREEN_HEIGHT * scaleFactor);
            int offsetX = (GraphicsDevice.Viewport.Width - screenWidth) / 2;
            int offsetY = (GraphicsDevice.Viewport.Height - screenHeight) / 2;
            _destinationRenderRectangle = new Rectangle(offsetX, offsetY, screenWidth, screenHeight);
        }

        private void PauseGame()
        {
            _isPaused = true;
            MediaPlayer.Pause();
        }

        private void ResumeGame()
        {
            _isPaused = false;
            MediaPlayer.Resume();
        }
    }
}