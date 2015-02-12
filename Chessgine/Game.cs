using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    public class Game
    {
        private ChessClock chessClock;
        private History history;

        public delegate void GameEventHandler(object myObject, GameArgs myArgs);
        public event GameEventHandler OnGameEvent;

        public Turn CurrentTurn { get; set; }
        public Color CurrentPlayerColor { get; set; }
        public Board Board { get; set; }
        public Player WhitePlayer { get; set; }
        public Player BlackPlayer { get; set; }
        public Rules Rules { get; set; }
        public bool GameStatus { get; set; }

        public Game(int minutes)
        {
            CurrentTurn = new Turn();
            history = new History(this);
            Board = new Board(this, history);
            Rules = new Rules(Board, history);
            chessClock = new ChessClock(minutes, this);
            WhitePlayer = new Player(Color.WHITE, this, FigureType.QUEEN);
            BlackPlayer = new Player(Color.BLACK, this, FigureType.QUEEN);
            CurrentPlayerColor = Color.WHITE;
            CurrentTurn.WhichPlayer = Color.WHITE;
        }

        public void Start()
        {
            Logger.Log("Game started, White player's turn.");
            chessClock.Start();
            GameStatus = true;
            if (OnGameEvent != null)
            {
                OnGameEvent(this, new GameArgs("GameStarted", "NONE"));
                OnGameEvent(this, new GameArgs("TurnStarted", "WHITE"));
            }
        }

        public void endTurn()
        {
            Logger.Log("Turn has ended, adding turn to history: " + CurrentTurn.ToString());
            if (OnGameEvent != null)
            {
                OnGameEvent(this, new GameArgs("TurnEnded", this.CurrentPlayerColor.ToString()));
            }

            Turn turn = new Turn
            {
                FromFieldCode = CurrentTurn.FromFieldCode,
                ToFieldCode = CurrentTurn.ToFieldCode,
                WhichFigure = CurrentTurn.WhichFigure,
                WhichPlayer = CurrentTurn.WhichPlayer
            };
            history.Turns.Add(turn);

            switch (this.CurrentPlayerColor)
            {
                case Color.BLACK:
                    CurrentPlayerColor = Color.WHITE;
                    break;
                case Color.WHITE:
                    CurrentPlayerColor = Color.BLACK;
                    break;
            }

            CurrentTurn.WhichPlayer = CurrentPlayerColor;
            if (OnGameEvent != null)
            {
                OnGameEvent(this, new GameArgs("TurnStarted", this.CurrentPlayerColor.ToString()));
            }
            chessClock.Hit();
        }

        public void gameFinished(Color winner)
        {
            chessClock.Stop();
            if (winner == Color.NONE)
            {
                Logger.Log("Game finished. It's a draw!");
            }
            GameStatus = false;

            Logger.Log("Game finished. The winner is Player " + winner + "!");
        }
    }
}
