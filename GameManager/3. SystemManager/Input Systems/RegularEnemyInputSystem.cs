using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> это обновляет состояние обычных врагов.
    /// </summary>
    public class RegularEnemyInputSystem : System
    {
        private List<Entity> entities;
        private List<StateComponent> states;
        private List<RegularEnemyComponent> inputs;
        private List<MovementComponent> movements;


        /// <summary>
        /// <summary>
        /// Инициализирует новый экземпляр класса RegularEnemyInputSystem.
        /// </summary>
        public RegularEnemyInputSystem()
        {
            entities = new List<Entity>();
            states = new List<StateComponent>();
            inputs = new List<RegularEnemyComponent>();
            movements = new List<MovementComponent>();
        }

        /// <summary>
        /// Добавляет сущность в систему.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно добавить.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            RegularEnemyComponent input = entity.GetComponent<RegularEnemyComponent>();
            MovementComponent movement = entity.GetComponent<MovementComponent>();

            if (state == null || input == null || movement == null)
            {
                return;
            }

            entities.Add(entity);
            states.Add(state);
            inputs.Add(input);
            movements.Add(movement);
        }

        /// <summary>
        /// Удаляет сущность из системы.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно удалить.</param>
        public override void RemoveEntity(Entity entity)
        {
            int index = entities.IndexOf(entity);
            if (index != -1)
            {
                entities.RemoveAt(index);
                states.RemoveAt(index);
                inputs.RemoveAt(index);
                movements.RemoveAt(index);
            }
        }

        public override void Update(GameTime gameTime)
        {
            int n = inputs.Count;
            for (int i = 0; i < n; i++)
            {
                StateComponent state = states[i];
                if (!entities[i].IsActive || state.CurrentSuperState == SuperState.IsAppearing || state.CurrentSuperState == SuperState.IsDead)
                {
                    continue;
                }

                inputs[i].Update(movements[i].Position.X);
                UpdateEntityState(gameTime, inputs[i], state);
            }
        }

        private void UpdateEntityState(GameTime gameTime, RegularEnemyComponent input, StateComponent state)
        {
            //Console.WriteLine($"Entity state before update: {state.CurrentState}");

            switch (state.CurrentSuperState)
            {
                case SuperState.IsOnGround:
                    break;
                default:
                    return;
            }

            switch (state.CurrentState)
            {
                case State.WalkLeft:
                    if (input.IsRight || !state.CanMoveLeft)
                    {
                        state.CurrentState = State.WalkRight;
                    }
                    break;

                case State.WalkRight:
                    if (input.IsLeft || !state.CanMoveRight)
                    {
                        state.CurrentState = State.WalkLeft;
                        
                    }
                    break;

                default:
                    state.CurrentState = state.DefaultState;

                    if (!state.CanMoveLeft)
                    {
                        state.CurrentState = State.WalkRight;
                    }
                    else if (!state.CanMoveRight)
                    {
                        state.CurrentState = State.WalkLeft;
                    }
                    break;
            }
        }
    }
}

