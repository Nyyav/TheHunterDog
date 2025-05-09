using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/>, который хранит идентификатор типа сущности для целей классификации и управления.
    /// Может быть заменен компонентом Tag, если этого требует игра.
    /// </summary>
    public class EntityTypeComponent : Component
    {
        /// <summary>
        /// Получает тип сущности, к которой принадлежит этот компонент.
        /// </summary>
        public EntityType Type { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EntityTypeComponent"/> с указанным типом сущности.
        /// </summary>
        /// <param name="type">Тип сущности, к которой принадлежит этот компонент.</param>
        public EntityTypeComponent(EntityType type)
        {
            Type = type;
        }
    }
}