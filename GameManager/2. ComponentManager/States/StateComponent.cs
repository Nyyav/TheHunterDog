using Microsoft.Xna.Framework;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/> /// <summary> /// Хранит информацию о состояниях сущности.
    /// </summary>
    public class StateComponent : Component
    {
        // Состояние объекта и предыдущее состояние.
        private State _currentState;
        public State previousState { get; private set; }

        // Супер-состояние и предыдущее супер-состояние.
        private SuperState _currentSuperState;
        public SuperState previousSuperState { get; private set; }

        // Флаги для ограничений движения.
        private bool _canMoveLeft, _canMoveRight;

        // Горизонтальное направление, 1 - вправо, -1 - влево.
        private int _horizontalDirection = 1;

        /// <summary>
        /// Идентификатор действия, используемый для определения текущей анимации.
        /// </summary>
        public AnimationID AnimationState { get; private set; }

        /// <summary>
        /// Супер-состояние по умолчанию, в которое сущность должна переходить после определенных действий (появление и т. д.).
        /// </summary>
        public SuperState DefaultSuperState { get; private set; }

        /// <summary>
        /// Состояние по умолчанию, в которое сущность должна переходить после определенных действий (появление и т. д.).
        /// </summary>
        public State DefaultState { get; private set; }

        /// <summary>
        /// Горизонтальное направление по умолчанию, -1 - влево, 1 - вправо.
        /// </summary>
        public int DefaultHorizontalDirection { get; private set; }

        /// <summary>
        /// Прыжки, выполненные данной сущностью.
        /// </summary>
        public int JumpsPerformed = 0;

        /// <summary>
        /// Инициализирует новый экземпляр класса StateComponent с состоянием и супер-состоянием по умолчанию.
        /// </summary>
        public StateComponent(State defaultState = State.Idle, SuperState defaultSuperState = SuperState.IsFalling, State currentState = State.Idle, SuperState currentSuperState = SuperState.IsAppearing)
        {
            _currentState = currentState;
            _currentSuperState = currentSuperState;
            DefaultSuperState = defaultSuperState;
            DefaultState = defaultState;
            DefaultHorizontalDirection = 1;
            if(defaultState == State.WalkLeft)
            {
                DefaultHorizontalDirection = -1;
            }
            _canMoveRight = true;
            _canMoveLeft = true;

            UpdateStateID();
        }

        /// <summary>
        /// Обновляет идентификатор состояния на основе текущего состояния и супер-состояния.
        /// Используется для определения текущей анимации.
        /// </summary>
        public void UpdateStateID()
        {
            switch (_currentSuperState)
            {
                case SuperState.IsOnGround:
                    AnimationState = AnimationID.Idle;
                    if (_currentState == State.WalkLeft || _currentState == State.WalkRight)
                    {
                        AnimationState = AnimationID.Walk;
                    }
                    break;

                case SuperState.IsFalling:
                    AnimationState = AnimationID.Fall;
                    if (_currentState == State.Slide)
                    {
                        AnimationState = AnimationID.Slide;
                    }
                    break;

                case SuperState.IsJumping:
                    AnimationState = AnimationID.Jump;
                    break;

                case SuperState.IsDoubleJumping:
                    AnimationState = AnimationID.DoubleJump;
                    break;

                case SuperState.IsDead:
                    AnimationState = AnimationID.Death;
                    break;

                case SuperState.IsAppearing:
                    AnimationState = AnimationID.Appear;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Указывает, может ли сущность двигаться влево.
        /// </summary>
        public bool CanMoveLeft
        {
            get { return _canMoveLeft; }
            set
            {
                _canMoveLeft = value;

                // Связано с механикой скольжения по стенам
                if (!_canMoveLeft)
                {
                    CurrentState = State.Slide;
                    HorizontalDirection = -1;
                }
            }
        }

        /// <summary>
        /// Указывает, может ли сущность двигаться вправо.
        /// </summary>
        public bool CanMoveRight
        {
            get { return _canMoveRight; }
            set
            {
                _canMoveRight = value;
                
                //if don't want a slide mechanic, delete this
                if (!_canMoveRight)
                {
                    CurrentState = State.Slide;
                    HorizontalDirection = 1;
                }
            }
        }

        /// <summary>
        /// Горизонтальное направление сущности.
        /// </summary>
        public int HorizontalDirection { get => _horizontalDirection; set => _horizontalDirection = value; }

        /// <summary>
        /// Взаимодействующее состояние сущности.
        /// </summary>
        public State CurrentState
        {
            get => _currentState;
            set
            {
                previousState = _currentState;
                _currentState = value;
                UpdateStateID();
            }
        }

        /// <summary>
        /// Непрерывное состояние сущности.
        /// </summary>
        public SuperState CurrentSuperState
        {
            get => _currentSuperState;
            set
            {
                previousSuperState = _currentSuperState;
                _currentSuperState = value;
                UpdateStateID();
            }
        }
    }
}
