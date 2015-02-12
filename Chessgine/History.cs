using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    public class History
    {
        public List<Turn> Turns { get; set; }
        private Game game;

        public History(Game game)
        {
            Turns = new List<Turn>();
            this.game = game;
        }

        public bool HasFigureMovedYet(Figure figure)
        {
            bool hasMoved = false;

            foreach (var turn in Turns)
            {
                if (turn.WhichFigure == figure)
                {
                    hasMoved = true;
                }
            }

            Logger.Log("Checked if figure "+ figure.ToString() + " currently at " + figure.Board.GetFiguresFieldCode(figure) + " has moved yet: " + hasMoved + ".");

            return hasMoved;
        }

        public string GetEnpassantFieldCode(Figure figure)
        {
            List<Turn> turns = this.Turns;
            Turn previousTurn = getPreviousTurn();
            if (previousTurn == null)
            {
                return null;
            }

            // in the previous turn the enemy (not the player's color) did not select the pawn we can't use enpassant
            if ((previousTurn.WhichFigure.FigureType != FigureType.PAWN) || (previousTurn.WhichPlayer == figure.Color))
            {
                return null;
            }

            string fromFieldCode = previousTurn.FromFieldCode;
            string toFieldCode = previousTurn.ToFieldCode;

            Tuple<int, int> fromArrayPosition = Board.FieldCode2ArrayPosition(fromFieldCode);
            Tuple<int, int> toArrayPosition = Board.FieldCode2ArrayPosition(toFieldCode);

            int yFromArrayPosition = fromArrayPosition.Item2;
            int yToArrayPosition = toArrayPosition.Item2;

            // in the previous turn the enemy did not use double forward step with its pawn we can't use enpassant
            if (Math.Abs(yFromArrayPosition - yToArrayPosition) != 2)
            {
                return null;
            }

            int enemyXToArrayPosition = toArrayPosition.Item1;
            int ourXToArrayPosition = Board.FieldCode2ArrayPosition(this.game.Board.GetFiguresFieldCode(figure)).Item1;

            int enemyYToArrayPosition = toArrayPosition.Item2;
            int ourYToArrayPosition = Board.FieldCode2ArrayPosition(this.game.Board.GetFiguresFieldCode(figure)).Item2;

            // if the pawns aren't standing next to eachother
            if ((Math.Abs(enemyXToArrayPosition - ourXToArrayPosition) != 1) ||
                (Math.Abs(enemyYToArrayPosition - ourYToArrayPosition) != 0))
            {
                return null;
            }
            else
            {
                if (figure.Color == Color.BLACK){
                    if (enemyXToArrayPosition < ourXToArrayPosition)
                    {
                        return figure.CalculateStepFieldCode(Direction.DIAGONAL_FORWARD_RIGHT, false, false, true);
                    }
                    else
                    {
                        return figure.CalculateStepFieldCode(Direction.DIAGONAL_FORWARD_LEFT, false, false, true);
                    }
                }
     
                else
                {
                    if (enemyXToArrayPosition < ourXToArrayPosition)
                    {
                        return figure.CalculateStepFieldCode(Direction.DIAGONAL_FORWARD_LEFT, false, false, true);
                    }
                    else
                    {
                        return figure.CalculateStepFieldCode(Direction.DIAGONAL_FORWARD_RIGHT, false, false, true);
                    }
                }
            }
        }

        private Turn getPreviousTurn()
        {
            if (Turns.Count >= 2)
            {
                return Turns[Turns.Count - 1];
            }
            return null;
        }
    }
}
