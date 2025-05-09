using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/> который содержит данные и методы, связанные с полем столкновения объекта в игре.
    /// </summary>
    /// <remarks>
    /// <summary>
    /// Этот компонент содержит свойства для исходной ширины и высоты сущности, ширины и высоты коллизионного бокса сущности,
    /// размеры вертикальных и горизонтальных смещений, а также положение коллизионного бокса.
    /// </summary>
    /// <remarks>
    /// Этот компонент используется для определения границ столкновений сущности с другими объектами в игровом мире.
    /// </remarks>
    public class CollisionBoxComponent : Component
    {
        /// <summary>
        /// Исходная ширина коллизионного бокса.
        /// </summary>
        public int OriginalWidth { get; private set; }

        /// <summary>
        /// Исходная высота коллизионного бокса.
        /// </summary>
        public int OriginalHeight { get; private set; }

        /// <summary>
        /// Текущая ширина коллизионного бокса.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Текущая высота коллизионного бокса.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Вертикальное смещение сверху коллизионного бокса.
        /// </summary>
        public int VertTopOffset { get; private set; }

        /// <summary>
        /// Вертикальное смещение снизу коллизионного бокса.
        /// </summary>
        public int VertBottomOffset { get; private set; }

        /// <summary>
        /// Горизонтальное смещение слева коллизионного бокса.
        /// </summary>
        public int HorLeftOffset { get; private set; }

        /// <summary>
        /// Горизонтальное смещение справа коллизионного бокса.
        /// </summary>
        public int HorRightOffset { get; private set; }

        /// <summary>
        /// Коллизионный бокс для сущности.
        /// </summary>
        private Rectangle _entityCollisionBox;

        // Левая граница платформы, на которой в данный момент стоит сущность.
        private int _groundLeft = 0;

        // Правая граница платформы, на которой в данный момент стоит сущность.
        private int _groundRight = GameConstants.SCREEN_WIDTH;

        // Нижняя граница платформы, на которой в данный момент стоит сущность.
        private int _groundBottom = GameConstants.SCREEN_HEIGHT;


        /// <summary>
        /// Создает новый экземпляр класса CollisionBoxComponent.
        /// </summary>
        /// <param name="position">Позиция коллизионного бокса в игровом мире.</param>
        /// <param name="width">Ширина коллизионного бокса.</param>
        /// <param name="height">Высота коллизионного бокса.</param>
        /// <param name="vertTopOffset">Вертикальное смещение от верха коллизионного бокса.</param>
        /// <param name="vertBottomOffset">Вертикальное смещение от низа коллизионного бокса.</param>
        /// <param name="horLeftOffset">Горизонтальное смещение от левой стороны коллизионного бокса.</param>
        /// <param name="horRightOffset">Горизонтальное смещение от правой стороны коллизионного бокса.</param>
        public CollisionBoxComponent(Vector2 position, int width, int height,
                                int vertTopOffset = 0, int vertBottomOffset = 0, int horLeftOffset = 0, int horRightOffset = 0)
        {
            this.OriginalWidth = width;
            this.OriginalHeight = height;
            this.VertTopOffset = vertTopOffset;
            this.VertBottomOffset = vertBottomOffset;
            this.HorLeftOffset = horLeftOffset;
            this.HorRightOffset = horRightOffset;
            this.Width = width - horLeftOffset - horRightOffset;
            this.Height = height - vertBottomOffset - vertTopOffset;
            this._entityCollisionBox = new Rectangle((int)position.X + horLeftOffset, (int)position.Y + vertTopOffset, this.Width, this.Height);
        }

        /// <summary>
        /// <summary>
        /// Обновляет позицию коллизионного бокса на основе позиции и направления сущности.
        /// </summary>
        /// <param name="positionX">Позиция сущности по оси X в игровом мире.</param>
        /// <param name="positionY">Позиция сущности по оси Y в игровом мире.</param>
        /// <param name="direction">Направление, в котором смотрит сущность (1 для вправо, -1 для влево).</param>
        public void UpdateBoxPosition(float positionX, float positionY, int direction)
        {
            switch (direction)
            {
                case -1:
                    _entityCollisionBox.X = (int)positionX + HorRightOffset;
                    break;

                default:
                    _entityCollisionBox.X = (int)positionX + HorLeftOffset;
                    break;
            }

            _entityCollisionBox.Y = (int)positionY + VertTopOffset;

        }

        /// <summary>
        /// Возвращает коллизионный бокс в виде объекта Rectangle.
        /// </summary>
        /// <returns>Коллизионный бокс в виде объекта Rectangle.</returns>
        public Rectangle GetRectangle()
        {
            return _entityCollisionBox;
        }

        /// <summary>
        /// Устанавливает границы платформы для проверки, находится ли сущность на земле.
        /// </summary>
        /// <param name="left">Левая граница платформы.</param>
        /// <param name="right">Правая граница платформы.</param>
        public void SetGroundLocation(int left, int right)
        {
            _groundLeft = left;
            _groundRight = right;
        }

        /// <summary>
        /// Устанавливает нижнюю границу платформы для проверки, скользит ли сущность.
        /// </summary>
        /// <param name="bottom">Нижняя граница платформы.</param>
        public void SetSlidingLocation(int bottom)
        {
            _groundBottom = bottom;
        }

        /// <summary>
        /// Проверяет, находится ли сущность в воздухе (не на платформе).
        /// </summary>
        /// <param name="position">Левая граница сущности (ось X).</param>
        /// <returns>True, если сущность находится в воздухе, и false в противном случае.</returns>
        public bool CheckIfInAir(float position, int direction)
        {
            float left = position + HorLeftOffset;
            float right = position - HorRightOffset + OriginalWidth;
            if (direction == -1)
            {
                left = position + HorRightOffset;
                right = position - HorLeftOffset + OriginalWidth;
            }
            return right < _groundLeft || left > _groundRight;
        }

        /// <summary>
        /// Проверяет, находится ли объект под платформой при скольжении.
        /// </summary>
        /// <param name="position">Верхняя граница объекта (ось y).</param>
        /// <returns>True, если объект находится в воздухе (не скользит), в противном случае ложно.</returns>
        public bool CheckIfBelow(float position)
        {

            float top = position + VertTopOffset + 5;
            return top > _groundBottom;
        }
    }
}