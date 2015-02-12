using Chessgine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameSession
    {
        public Game Game { get; set; }
        public Player WhitePlayer { get; set; }
        public Player BlackPlayer { get; set; }

        public GameSession(Game game, Player whitePlayer, Player blackPlayer)
        {
            this.Game = game;
            this.WhitePlayer = whitePlayer;
            this.BlackPlayer = blackPlayer;
        }

        public Player GetEnemy(Player currentPlayer)
        {
            if (this.WhitePlayer == currentPlayer)
            {
                return this.BlackPlayer;
            }
            return this.WhitePlayer;
        }
    }
}
