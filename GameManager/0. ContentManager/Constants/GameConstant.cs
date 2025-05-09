using System.Reflection;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// Набор общих игровых констант.
    /// </summary>
    public static class GameConstants
    {
        // Константы, относящиеся к игроку.
        public static int PLAYER_MAX_HP = 3;

        // Константы, относящиеся к объектам (сущностям) мира.
        public static float GRAVITY = 1700f;
        public static int OTHER_HP = 1;
        public static float SpeedY = -500f;
        public static float SpeedX = 80f;
        public static float SpeedXonCollision = -50f;
        public static float SpeedYonCollision = 300f;

        // Тэги для обозначения препятствий на карте.
        public static HashSet<string> OBSTACLES = new HashSet<string>()
        {
            "solid",
            "float"
        };

        // Тэги для обозначения сущностей на карте.
        public static HashSet<string> ENTITIES = new HashSet<string>()
        {
            "entity"
        };

        // Разные игровые константы.
        public static int SCREEN_WIDTH = 640;
        public static int SCREEN_HEIGHT = 368;
        public static bool FULL_SCREEN = false;
        public static float AnimationFPS = 20f;
        public static float PhysicFPS = 60f;

        // Клавиши управления.
        public const Keys LEFT_KEY = Keys.A;
        public const Keys RIGHT_KEY = Keys.D;
        public const Keys JUMP_KEY = Keys.Space;

        // Флаги отладки.
        public static bool DisplayCollisionBoxes = false;
        public static bool AnimationDebugMessages = false;
        public static bool EntityDebugMessages = false;

        /// <summary>
        /// Обновляет значение указанной игровой константы.
        /// </summary>
        /// <param name="fieldName">Имя константы для изменения.</param>
        /// <param name="value">Новое значение константы.</param>
        public static void UpdateConstant(string fieldName, object value)
        {
            FieldInfo field = typeof(GameConstants).GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
            if (field != null)
            {
                field.SetValue(null, value);
            }
        }
    }
}