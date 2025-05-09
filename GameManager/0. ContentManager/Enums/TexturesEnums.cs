namespace MonogameExamples
{
    /// <summary>
    /// Перечисление, представляющее различные состояния текстур анимации игрока.
    /// </summary>
    public enum PlayerTexture
    {
        Idle,        // Состояние покоя
        Walking,     // Состояние ходьбы
        Jump,        // Состояние прыжка
        DoubleJump,  // Состояние двойного прыжка
        Fall,        // Состояние падения
        Slide,       // Состояние скольжения
        Hit          // Состояние получения урона
    }

    /// <summary>
    /// Перечисление, представляющее различные состояния текстур анимации врага в маске.
    /// </summary>
    public enum MaskedEnemyTexture
    {
        Idle,        // Состояние покоя
        Walking,     // Состояние ходьбы
        Jump,        // Состояние прыжка
        DoubleJump,  // Состояние двойного прыжка
        Fall,        // Состояние падения
        Slide,       // Состояние скольжения
        Hit          // Состояние получения урона
    }

    public enum BackgroundTexture
    {
        //добавить текстуры
        // Здесь нужно добавить элементы перечисления для разных текстур фона
    }

    /// <summary>
    /// Перечисление, представляющее различные типы текстур фруктов.
    /// </summary>
    public enum FruitTexture
    {
        Apple,        // Яблоко
        Orange,       // Апельсин
        Collected     // Собранный (фрукт)
    }

    /// <summary>
    /// Перечисление, представляющее различные типы текстур Tiled.
    /// </summary>
    public enum TiledTexture
    {
        /// <summary>
        /// Текстура Tiled для местности (ландшафта).
        /// </summary>
        Terrain,

        /// <summary>
        /// Текстура Tiled для элементов пользовательского интерфейса.
        /// </summary>
        UI,

        /// <summary>
        /// Текстура Tiled для фонового окружения.
        /// </summary>
        BackgroundEnvironment,

        /// <summary>
        /// Текстура Tiled для облаков.
        /// </summary>
        Cloud,
        AutumnForest   // Текстуры осеннего леса
    }
}