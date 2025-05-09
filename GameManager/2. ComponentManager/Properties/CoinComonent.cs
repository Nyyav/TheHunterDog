using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonogameExamples
{
    /// <summary>
    /// <see cref="Component"/>, который отслеживает количество монет.
    /// </summary>
    public class CoinComponent : Component
    {
        //Position
        private  int _coins;

        /// <summary>
        /// Монеты сущности.
        /// </summary>
        public int Coins { get => _coins; set => _coins = value; }

        /// <summary>
        /// Инициализирует новый экземпляр класса CoinComponent с указанным начальным количеством монет.
        /// </summary>
        /// <param name="initialCoins">Начальное количество монет.</param>
        public CoinComponent(int initialCoins = 0)
        {
            _coins = 0;
        }
    }
}