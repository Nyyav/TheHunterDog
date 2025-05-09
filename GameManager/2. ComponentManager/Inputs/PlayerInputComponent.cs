using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/>, представляющий состояние ввода для сущности игрока.
    /// </summary>
    public class PlayerInputComponent : Component
    {
        /// <summary>
        /// Получает значение, указывающее, нажата ли в данный момент клавиша для перемещения игрока влево.
        /// </summary>
        public bool IsLeftKeyDown { get; private set; }

        /// <summary>
        /// Получает значение, указывающее, нажата ли в данный момент клавиша для перемещения игрока вправо.
        /// </summary>
        public bool IsRightKeyDown { get; private set; }

        /// <summary>
        /// Получает значение, указывающее, нажата ли в данный момент клавиша для прыжка игрока.
        /// </summary>
        public bool IsJumpKeyDown { get; private set; }

        /// <summary>
        /// Обновляет состояние ввода компонента на основе текущего состояния клавиатуры.
        /// </summary>
        /// <param name="gameTime">Текущее игровое время.</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            IsLeftKeyDown = keyboardState.IsKeyDown(GameConstants.LEFT_KEY);
            IsRightKeyDown = keyboardState.IsKeyDown(GameConstants.RIGHT_KEY);
            IsJumpKeyDown = keyboardState.IsKeyDown(GameConstants.JUMP_KEY);
        }
    }
}