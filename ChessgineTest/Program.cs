using Chessgine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessgineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(20);
            game.OnGameEvent += new Game.GameEventHandler(on);

            game.Start();
            game.WhitePlayer.SelectField(Board.A2);
            List<string> validSteps = game.Rules.GetValidFields(game.WhitePlayer.GetSelectedFigure());
            if (validSteps.Count > 0)
            {
                game.WhitePlayer.MoveSelectedFigureTo(validSteps[0]);
            }
            string gameBoard = game.Board.ToString();

            Logger.Log(game.Board.ToString());
        }

        public static void on(object a, GameArgs b)
        {
            Logger.Log("New event " + b.Message);
        }
    }
}
