using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/>, отвечающий за управление движением обычного врага в пределах указанного диапазона.
    /// Он определяет направление движения на основе текущей позиции врага.
    /// </summary>
    public class RegularEnemyComponent : Component
    {
        /// <summary>
        /// Получает значение, указывающее, должен ли враг двигаться влево.
        /// </summary>
        public bool IsLeft { get; private set; }

        /// <summary>
        /// Получает значение, указывающее, должен ли враг двигаться вправо.
        /// </summary>
        public bool IsRight { get; private set; }

        private float _left;
        private float _right;

        /// <summary>
        /// Инициализирует новый экземпляр класса RegularEnemyComponent с указанной начальной позицией и диапазоном движения.
        /// </summary>
        /// <param name="start">Начальная позиция врага по оси X.</param>
        /// <param name="leftRange">Расстояние, на которое врагу разрешено двигаться влево от начальной позиции.</param>
        /// <param name="rightRange">Расстояние, на которое врагу разрешено двигаться вправо от начальной позиции.</param>
        public RegularEnemyComponent(float start, float leftRange, float rightRange)
        {
            _left = start - leftRange;
            _right = start + rightRange;
        }

        /// <summary>
        /// Обновляет направление движения врага на основе его текущей позиции.
        /// </summary>
        /// <param name="positionX">Текущая позиция врага по оси X.</param>
        public void Update(float positionX)
        {
            IsLeft = false;
            IsRight = false;

            if (positionX <= _left)
            {
                IsRight = true;
            }
            else if (positionX >= _right)
            {
                IsLeft = true;
            }
        }
    }
}