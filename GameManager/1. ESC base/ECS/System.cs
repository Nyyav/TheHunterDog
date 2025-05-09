using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameExamples
{
    /// <summary>
    /// Абстрактный базовый класс для систем в игре.
    /// </summary>
    public abstract class System
    {
        /// <summary>
        /// Добавляет сущность в систему.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        public virtual void AddEntity(Entity entity)
        {

        }

        /// <summary>
        /// Удаляет сущность из системы.
        /// </summary>
        /// <param name="entity">Сущность для удаления.</param>
        public virtual void RemoveEntity(Entity entity)
        {

        }

        /// <summary>
        /// Подписывает систему на события MessageBus.
        /// </summary>
        public virtual void Subscribe()
        {

        }

        /// <summary>
        /// Отписывает систему от событий MessageBus.
        /// </summary>
        public virtual void Unsubscribe()
        {

        }

        /// <summary>
        /// Обновляет систему.
        /// </summary>
        /// <param name="gameTime">Текущее игровое время.</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Отрисовывает систему.
        /// </summary>
        /// <param name="spriteBatch">Пакет спрайтов, используемый для отрисовки.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}