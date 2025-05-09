using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="System"/> /// <summary>
    /// Система, которая обновляет состояние сущностей на основе ввода с клавиатуры.
    /// </summary>
    public class PlayerInputSystem : System
    {
        private List<Entity> entities;
        private List<StateComponent> states;
        private List<PlayerInputComponent> inputs;

        /// <summary>
        /// Инициализирует новый экземпляр класса PlayerInputSystem.
        /// </summary>
        public PlayerInputSystem()
        {
            entities = new List<Entity>();
            states = new List<StateComponent>();
            inputs = new List<PlayerInputComponent>();
        }

        /// <summary>
        /// Добавляет сущность в систему.
        /// </summary>
        /// <param name="entity">Сущность, которую нужно добавить.</param>
        public override void AddEntity(Entity entity)
        {
            StateComponent state = entity.GetComponent<StateComponent>();
            PlayerInputComponent input = entity.GetComponent<PlayerInputComponent>();
            if (state == null || input == null)
            {
                return;
            }

            entities.Add(entity);
            states.Add(state);
            inputs.Add(input);
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
            }
        }

        /// <summary>
        /// Обновляет состояние системы на основе ввода с клавиатуры.
        /// </summary>
        /// <param name="gameTime">Текущее игровое время.</param>
        public override void Update(GameTime gameTime)
        {
            int n = inputs.Count;
            for (int i = 0; i < n; i++)
            {
                Entity entity = entities[i]; // Получаем entity здесь
                StateComponent state = states[i];
                PlayerInputComponent input = inputs[i];

                if (!entity.IsActive || state.CurrentSuperState == SuperState.IsDead || state.CurrentSuperState == SuperState.IsAppearing)
                {
                    continue;
                }

                input.Update(gameTime);
                UpdateEntityState(gameTime, input, state, entity); // Передаем entity
            }
        }

        /// <summary>
        /// Обновляет состояние сущности на основе ее ввода и текущего состояния.
        /// </summary>
        /// <param name="gameTime">Текущее игровое время.</param>
        /// <param name="input">Компонент ввода для сущности.</param>
        /// <param name="state">Компонент состояния для сущности.</param>
        private void UpdateEntityState(GameTime gameTime, PlayerInputComponent input, StateComponent state, Entity entity)
        {
            bool bothKeysDown = input.IsLeftKeyDown && input.IsRightKeyDown;
            bool bothKeysUp = !input.IsLeftKeyDown && !input.IsRightKeyDown;

            // Обновляем MovementComponent
            MovementComponent movement = entity.GetComponent<MovementComponent>();
            if (movement != null)
            {
                movement.IsJumpKeyDown = input.IsJumpKeyDown;
                movement.BothKeysDown = bothKeysDown;
            }

            switch (state.CurrentState)
            {
                case State.Slide:
                    if (bothKeysDown || bothKeysUp)
                    {
                        break;
                    }
                    else if (input.IsLeftKeyDown && state.HorizontalDirection == 1)
                    {
                        state.CurrentState = State.WalkLeft;
                    }
                    else if (input.IsRightKeyDown && state.HorizontalDirection == -1)
                    {
                        state.CurrentState = State.WalkRight;
                    }
                    break;

                default:
                    if (bothKeysDown || bothKeysUp)
                    {
                        state.CurrentState = State.Idle;
                    }
                    else if (input.IsLeftKeyDown)
                    {
                        state.CurrentState = State.WalkLeft;
                    }
                    else if (input.IsRightKeyDown)
                    {
                        state.CurrentState = State.WalkRight;
                    }
                    break;
            }

            switch (state.CurrentSuperState)
            {
                case SuperState.IsOnGround:
                    state.JumpsPerformed = 0;
                    if (input.IsJumpKeyDown && !bothKeysDown)
                    {
                        state.JumpsPerformed = 1;
                        state.CurrentState = State.Jump;
                    }
                    break;

                case SuperState.IsFalling:
                    if (state.CurrentState == State.Slide)
                    {
                        state.JumpsPerformed = 1;
                    }
                    if (input.IsJumpKeyDown && !bothKeysDown)
                    {
                        if (state.JumpsPerformed < 1)
                        {
                            state.CurrentState = State.Jump;
                            state.JumpsPerformed = 1;
                        }
                        else if (state.JumpsPerformed < 2)
                        {
                            state.CurrentState = State.DoubleJump;
                            state.JumpsPerformed = 2;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}

