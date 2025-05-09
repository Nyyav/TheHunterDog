using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/>, содержащий данные, связанные с движением сущности в игре.
    /// </summary>
    /// <remarks>
    /// Этот компонент содержит свойства для позиции, предыдущей позиции, скорости, ускорения и флагов, связанных с горизонтальным движением.
    /// </remarks>
    public class MovementComponent : Component
    {
        public bool IsJumpKeyDown { get; set; }
        public bool BothKeysDown { get; set; }
        //Position
        private Vector2 _position;

        //Motion
        private Vector2 _velocity;
        private Vector2 _acceleration;

        /// <summary>
        /// Предыдущая позиция сущности.
        /// </summary>
        public Vector2 LastPosition { get; private set;}

        /// <summary>
        /// Позиция сущности.
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set
            {
                LastPosition = _position;
                _position = value;
            }
        }

        /// <summary>
        /// Скорость сущности.
        /// </summary>
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }

        /// <summary>
        /// Ускорение сущности.
        /// </summary>
        public Vector2 Acceleration { get => _acceleration; set => _acceleration = value; }

        /// <summary>
        /// Инициализирует новый экземпляр класса MovementComponent с указанной начальной позицией.
        /// </summary>
        /// <param name="initialPosition">Начальная позиция сущности.</param>
        public MovementComponent(Vector2 initialPosition)
        {
            _position = initialPosition;
            _velocity = Vector2.Zero;
            _acceleration = Vector2.Zero;
        }
    }
}