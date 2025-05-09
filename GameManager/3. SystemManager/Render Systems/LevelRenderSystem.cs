using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> отвечает за рендеринг объектов в игре.
    /// </summary>
    public class LevelRenderSystem : System
    {
        // Список экземпляров EntityData, в которых хранятся ссылки на связанный объект, StateComponent, AnimatedComponent и MovementComponent.
        private string _levelID;

        /// <summary>
        /// Инициализирует новый экземпляр<see cref="LevelRenderSystem"/> класса.
        /// </summary>
        public LevelRenderSystem(LevelID levelID)
        {
            _levelID = levelID.ToString();
        }

        /// <summary>
        /// Отрисовывает текущий уровень.
        /// </summary>
        /// <param name="spriteBatch">Используется для рисования.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Loader.tiledHandler.Draw(_levelID, spriteBatch);
        }
    }
}
