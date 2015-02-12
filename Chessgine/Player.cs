using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    public class Player
    {
        public FigureType PawnPromotionPreference { get; set; }

        private string selectedField;
        private Color color;
        private Game game;

        public Player(Color color, Game game, FigureType pawnPromotionPreference)
        {
            this.color = color;
            this.game = game;
            this.PawnPromotionPreference = pawnPromotionPreference;
        }

        public void SelectField(string fieldCode)
        {
            Figure figure = this.game.Board.GetFigureAtFieldCode(fieldCode);

            if (figure == null)
            {
                throw new NoFigureOnFieldException();
            }

            if (this.color != figure.Color)
            {
                throw new EnemyFigureSelectedException();
            }

            if (this.color != this.game.CurrentPlayerColor)
            {
                throw new OtherPlayerTurnsException();
            }

            Logger.Log("Player selected figure "+ figure.ToString() + " at "+figure.Board.GetFiguresFieldCode(figure)+".");
            this.game.CurrentTurn.WhichFigure = figure;
            this.selectedField = fieldCode;
        }

        public Figure GetSelectedFigure()
        {
            return this.game.Board.GetFigureAtFieldCode(selectedField);
        }

        public void MoveSelectedFigureTo(string fieldCode)
        {
            if (selectedField == null)
            {
                throw new NoSelectedFigureForPlayerException();
            }
            Figure figure = this.GetSelectedFigure();

            // after move, delete the figure selection
            this.selectedField = null;

            figure.MoveTo(fieldCode);
        }
    }
}
