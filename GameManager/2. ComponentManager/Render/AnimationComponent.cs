using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/> отвечает за управление анимацией объектов.
    /// </summary>
    /// <remarks>
    /// <summary>
    /// Этот компонент содержит словарь, который сопоставляет действие с его соответствующей анимацией (stateID к ActionAnimation).
    /// Он также содержит текущую анимацию сущности и несколько полезных методов.
    /// </summary>
    /// <remarks>
    /// Используется для хранения и управления анимациями сущности.
    /// </remarks>
    public class AnimationComponent : Component
    {
        /// <summary>
        /// Словарь всех анимаций для этого компонента, индексированных по имени действия.
        /// </summary>
        private Dictionary<AnimationID, ActionAnimation> _animations;

        /// <summary>
        /// Имя текущего действия анимации.
        /// </summary>
        private AnimationID _currentAction;
        private AnimationID _defaultAction;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref ="AnimationComponent"/>.
        /// </summary>
        public AnimationComponent(AnimationID defaultAction = AnimationID.Idle)
        {
            _animations = new Dictionary<AnimationID, ActionAnimation>();
            _defaultAction = defaultAction;
            _currentAction = defaultAction;
        }

        /// <summary>
        /// Добавляет анимацию для заданного действия в список анимаций.
        /// </summary>
        /// <param name="spriteID">Идентификатор спрайт-листа в классе Loader</param>
        /// <param name="action">Имя действия (AnimationID из StateComponent), связанного с анимацией.</param>
        /// <param name="rows">Количество строк в спрайт-листе.</param>
        /// <param name="columns">Количество столбцов в спрайт-листе.</param>
        /// <param name="fps">Количество кадров в секунду для анимации.</param>
        public void AddAnimation(Enum spriteID, AnimationID action, int rows, int columns, float fps)
        {
            _animations[action] = new ActionAnimation(spriteID, rows, columns, fps);
        }

        // <summary>
        /// Возвращает текущую анимацию, связанную с компонентом,
        /// или анимацию по умолчанию "idle", если текущее действие не имеет связанной анимации.
        /// </summary>
        /// <returns>Текущая анимация, связанная с компонентом, или анимация по умолчанию "idle", если текущее действие не имеет связанной анимации.</returns>
        public ActionAnimation GetCurrentAnimation()
        {

            if (!_animations.ContainsKey(_currentAction))
            {
                //Console.WriteLine($"Animation for action '{_currentAction}' does not exist, playing default animation"); // Debug message
                _currentAction = _defaultAction;
            }
            return _animations[_currentAction];
        }

        /// <summary>
        /// <summary>
        /// Обновляет текущую анимацию на основе прошедшего игрового времени.
        /// </summary>
        /// <param name="gameTime">Прошедшее игровое время.</param>
        public void Update(GameTime gameTime)
        {
            ActionAnimation currentAnimation = GetCurrentAnimation();
            currentAnimation.Update(gameTime);
        }

        /// <summary>
        /// Отрисовывает текущую анимацию на экране.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, используемый для отрисовки анимации.</param>
        /// <param name="position">Позиция анимации на экране.</param>
        /// <param name="direction">Горизонтальное направление, в котором смотрит анимация.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, int direction = 1)
        {
            ActionAnimation currentAnimation = GetCurrentAnimation();
            currentAnimation.Draw(spriteBatch, position, direction);
        }

        /// <summary>
        /// Изменяет текущую анимацию при задании нового действия.
        /// </summary>
        /// <param name="action">Имя нового действия.</param>
        public void SetCurrentAction(AnimationID action)
        {
            if (_currentAction != action)
            {
                if (GameConstants.AnimationDebugMessages)
                {
                    Console.WriteLine($"Анимация {_currentAction} изменяется на анимацию {action}");
                }

                ResetCurrentAnimation();
                _currentAction = action;
            }
        }

        /// <summary>
        /// Сбросить текущую анимацию
        /// </summary>
        private void ResetCurrentAnimation()
        {
            ActionAnimation currentAnimation = GetCurrentAnimation();
            currentAnimation.Reset();
        }
    }

    /// <summary>
    /// Вспомогательный класс, который представляет собой отдельную анимацию, состоящую из нескольких кадров, каждый из которых является прямоугольником в спрайт-листе.
    /// </summary>
    public class ActionAnimation
    {
        private Texture2D _texture;         // Текстура, содержащая спрайт-лист.
        private int _rows;                  // Количество строк в спрайт-листе.
        private int _columns;               // Количество столбцов в спрайт-листе.
        private int _currentFrame;          // Индекс текущего отображаемого кадра.
        private int _totalFrames;           // Общее количество кадров в спрайт-листе.
        private float _frameTime;           // Время между каждым кадром в секундах.
        private float _elapsedFrameTime;    // Время, прошедшее с момента последней смены кадра, в секундах.
        private Rectangle[] _frames;        // Массив прямоугольников, определяющих каждый кадр в спрайт-листе.

        /// <summary>
        /// Указывает, завершена ли текущая анимация
        /// </summary>
        public bool IsFinished { get { return _currentFrame >= _totalFrames - 1; } }

        /// <summary>
        /// <summary>
        /// Создает новый экземпляр класса ActionAnimation.
        /// </summary>
        /// <param name="spriteID">Идентификатор спрайт-листа в классе Loader.</param>
        /// <param name="rows">Количество строк в спрайт-листе.</param>
        /// <param name="columns">Количество столбцов в спрайт-листе.</param>
        /// <param name="fps">Частота кадров в кадрах в секунду.</param>
        public ActionAnimation(Enum spriteID, int rows, int columns, float fps)
        {
            _texture = Loader.GetTexture(spriteID);
            _rows = rows;
            _columns = columns;
            _currentFrame = 0;
            _totalFrames = _rows * _columns;
            _frameTime = 1 / fps;
            _elapsedFrameTime = 0;

            _frames = new Rectangle[_totalFrames];
            int frameWidth = _texture.Width / _columns;
            int frameHeight = _texture.Height / _rows;
            for (int i = 0; i < _totalFrames; i++)
            {
                int x = (i % _columns) * frameWidth;
                int y = (i / _columns) * frameHeight; // Исправлено: было (i % _rows)
                _frames[i] = new Rectangle(x, y, frameWidth, frameHeight);
            }
        }

        /// <summary>
        /// Обновляет анимацию на основе прошедшего игрового времени.
        /// </summary>
        /// <param name="gameTime">Прошедшее игровое время.</param>
        public void Update(GameTime gameTime)
        {
            if (_frames.Length == 1)
            {
                return;
            }
            _elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedFrameTime >= _frameTime)
            {
                _currentFrame++;
                if (_currentFrame >= _totalFrames)
                {
                    _currentFrame = 0;
                }
                _elapsedFrameTime = 0;
            }
        }

        /// <summary>
        /// Отрисовывает текущий кадр анимации в указанной позиции с указанным направлением.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, используемый для отрисовки кадра.</param>
        /// <param name="position">Позиция кадра в игровом мире.</param>
        /// <param name="direction">Направление, в котором смотрит кадр (1 для вправо, -1 для влево).</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, int direction = 1)
        {
            bool isFacingLeft = false;
            if (direction == -1)
                isFacingLeft = true;

            Rectangle currentFrame = _frames[_currentFrame];

            //Console.WriteLine($"Drawing frame {_currentFrame}: X = {currentFrame.X}, Y = {currentFrame.Y}, Width = {currentFrame.Width}, Height = {currentFrame.Height}");

            spriteBatch.Draw(_texture,
                     position,
                     sourceRectangle: currentFrame,
                     Color.White,
                     0f,
                     Vector2.Zero,
                     1f,
                     isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                     0f);
        }

        /// <summary>
        /// Возвращает анимацию к первому кадру.
        /// </summary>
        public void Reset()
        {
            _currentFrame = 0;
            _elapsedFrameTime = 0;
        }
    }
}