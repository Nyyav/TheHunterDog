using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// TimerSystem отвечает за отслеживание объектов с помощью таймеров и управление ими.
    /// </summary>
    public class TimerSystem : System
    {
        private Entity _entity;
        private float _timer;
        private Vector2 _position;

        /// <summary>
        /// Инициализирует новый экземпляр<see cref="TimerSystem"/> класса.
        /// </summary>
        public TimerSystem()
        {
            _entity = null;
            _timer = 10;
        }

        /// <summary>
        /// Удаляет объект из системы.
        /// </summary>
        /// <param name="entity">Объект, подлежащий удалению.</param>
        public override void RemoveEntity(Entity entity)
        {
            if (_entity == entity)
            {
                _entity = null;
            }
        }

        /// <summary>
        /// Подписывается на рассылку GameTimerMessage.
        /// </summary>
        public override void Subscribe()
        {
            MessageBus.Subscribe<GameTimerMessage>(TimerStarted);
        }

        /// <summary>
        /// Отписывается от рассылки GameTimerMessage.
        /// </summary>
        public override void Unsubscribe()
        {
            MessageBus.Unsubscribe<GameTimerMessage>(TimerStarted);
        }

        /// <summary>
        /// Вызывается при запуске таймера, устанавливает объект и таймер на основе полученного сообщения.
        /// </summary>
        /// <param name="message">Сообщение Gametimermessage, содержащее информацию о таймере.</param>
        public void TimerStarted(GameTimerMessage message)
        {
            _entity = message.Entity;
            _timer = message.Timer;
            _position = message.Position;
        }

        /// <summary>
        /// Обновляет таймер в зависимости от затраченного игрового времени.
        /// </summary>
        /// <param name="gameTime">Текущее время игры.</param>
        public override void Update(GameTime gameTime)
        {
            if (_entity == null || !_entity.IsActive)
            {
                return;
            }

            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer <= 0)
            {
                //Kогда таймер достигнет нуля, выполнится действие
                _timer = 0;

                MessageBus.Publish(new NextLevelMessage());
            }
        }

        /// <summary>
        /// Метод для отображения таймера на экране.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_entity != null && _entity.IsActive)
            {
                var font = Loader.GetFont("GameFont");
                int minutes = (int)_timer / 60;
                int seconds = (int)_timer % 60;
                string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);

                spriteBatch.DrawString(font, timerText, _position, Color.Black);
            }
        }

    }
}
