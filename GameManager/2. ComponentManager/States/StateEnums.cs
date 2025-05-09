namespace MonogameExamples
{
    /// <summary>
    /// Возможные взаимодействующие состояния, в которых может находиться объект.
    /// </summary>
    public enum State
    {
        Idle,
        WalkLeft,
        WalkRight,
        Jump,
        DoubleJump,
        Slide,
    }

    /// <summary>
    /// Возможные непрерывные сверхсостояния, в которых может находиться объект.
    /// </summary>
    public enum SuperState
    {
        IsOnGround,
        IsFalling,
        IsJumping,
        IsDoubleJumping,
        IsDead,
        IsSliding,
        IsAppearing,
    }

    /// <summary>
    /// Возможные состояния анимации, в которых может находиться объект.
    /// </summary>
    public enum AnimationID
    {
        Idle,
        Walk,
        Jump,
        DoubleJump,
        Slide,
        Fall,
        Death,
        Appear,
    }
}