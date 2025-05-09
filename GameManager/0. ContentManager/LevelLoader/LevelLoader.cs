using System.Collections.Generic;
using TiledCS;

namespace MonogameExamples
{
    /// <summary>
    /// Предоставляет статический метод для загрузки сущностей из карты Tiled.
    /// </summary>
    public class LevelLoader
    {
        /// <summary>
        /// Загружает сущности из карты Tiled для указанного уровня.
        /// </summary>
        /// <param name="level">Уровень, для которого загружаются сущности.</param>
        public static void GetObjects(LevelID level)
        {
            // Получаем карту Tiled для заданного уровня
            TiledMap map = Loader.tiledHandler.GetMap(level);

            // Перебираем каждый слой на карте
            foreach (var layer in map.Layers)
            {
                // Пропускаем слои, которые не являются слоями сущностей
                if (!GameConstants.ENTITIES.Contains(layer.name))
                {
                    continue;
                }

                // Перебираем каждый объект на слое
                foreach (var obj in layer.objects)
                {
                    // Создаем вектор позиции из координат объекта
                    Vector2 position = new Vector2((float)obj.x, (float)obj.y);

                    // Обрабатываем создание объекта на основе его имени
                    switch (obj.name)
                    {
                        /*case "bg":
                            Background(obj);
                            break;*/

                        case "player":
                            EntityFactory.CreatePlayer(position);
                            break;

                        case "regularEnemy":
                            RegularEnemy(obj, position);
                            break;

                        case "fruit":
                            Fruit(obj, position);
                            break;

                        case "timer":
                            Timer(obj, position);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Создает сущность обычного врага на основе TiledObject и его позиции.
        /// </summary>
        /// <param name="obj">TiledObject, представляющий врага.</param>
        /// <param name="position">Позиция врага в игровом мире.</param>
        private static void RegularEnemy(TiledObject obj, Vector2 position)
        {
            // Устанавливаем значения по умолчанию для диапазона движения врага и начального направления
            int leftRange = 40;
            int rightRange = 40;
            State direction = State.WalkLeft;

            // Перебираем свойства объекта и обновляем значения, если необходимо
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "left":
                        if (int.TryParse(property.value, out int tempLeftRange))
                        {
                            leftRange = tempLeftRange;
                        }
                        break;

                    case "right":
                        if (int.TryParse(property.value, out int tempRightRange))
                        {
                            rightRange = tempRightRange;
                        }
                        break;

                    case "direction":
                        // Если значение "direction" равно 1, враг начинает движение вправо
                        if (int.TryParse(property.value, out int result) && result == 1)
                        {
                            direction = State.WalkRight;
                        }
                        break;
                }
            }

            // Создаем обычного врага с разобранными свойствами
            EntityFactory.CreateRegularEnemy(position, leftRange, rightRange, direction);
        }

        /// <summary>
        /// Создает сущность фрукта на основе заданного TiledObject и позиции.
        /// </summary>
        /// <param name="obj">TiledObject, содержащий свойства фрукта.</param>
        /// <param name="position">Позиция фрукта.</param>
        private static void Fruit(TiledObject obj, Vector2 position)
        {
            // Устанавливаем тип фрукта по умолчанию
            string fruitType = "apple";

            // Перебираем свойства объекта и обновляем значения, если необходимо
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "fruitType":
                        fruitType = property.value;
                        break;
                }
            }

            // Создание сущности фрукта
            EntityFactory.CreateFruit(position, fruitType);
        }

        /// <summary>
        /// Создает сущность таймера на основе заданного TiledObject и позиции.
        /// </summary>
        /// <param name="obj">TiledObject, содержащий свойства таймера.</param>
        /// <param name="position">Позиция таймера.</param>
        private static void Timer(TiledObject obj, Vector2 position)
        {
            // Устанавливаем длительность таймера по умолчанию, в секундах
            int timer = 188;
            bool isActive = true;

            // Перебираем свойства объекта и обновляем значения, если необходимо
            foreach (var property in obj.properties)
            {
                switch (property.name)
                {
                    case "time":
                        if (int.TryParse(property.value, out int tempTimer))
                        {
                            timer = tempTimer;
                        }
                        break;

                    case "active":
                        if (bool.TryParse(property.value, out bool temp))
                        {
                            isActive = temp;
                        }
                        break;

                    default:
                        break;
                }
            }

            // Создаем сущность таймера с разобранными свойствами
            EntityFactory.CreateTimer(position, timer, isActive);
        }
    }
}
