using Microsoft.Xna.Framework;
namespace MonogameExamples
{
    /// <summary>
    /// Структура, содержащая наиболее часто используемые компоненты.
    /// </summary>
    struct EntityData
    {
        public Entity Entity;
        public MovementComponent Movement;
        public StateComponent State;
        public CollisionBoxComponent CollisionBox;
        public AnimationComponent Animations;
    }
}