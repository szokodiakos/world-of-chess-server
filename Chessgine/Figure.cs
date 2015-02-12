using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    public class Figure
    {
        public FigureType FigureType { get; set; }
        public Color Color { get; set; }

        public Board Board { get; set; }

        public Figure(Board board, FigureType figureType, Color color)
        {
            FigureType = figureType;
            Color = color;
            this.Board = board;
        }

        public void MoveTo(string fieldCode)
        {
            this.Board.MoveFigure(fieldCode, this);
        }

        // Transforms direction to both black and white figures
        private string TransformStepFieldCode(string currentFieldCode, Direction direction)
        {
            int modifier = this.Color == Color.WHITE ? 1 : -1;
            int x = Board.FieldCode2ArrayPosition(currentFieldCode).Item1;
            int y = Board.FieldCode2ArrayPosition(currentFieldCode).Item2;
            int xOld = x;
            int yOld = y;

            switch (direction)
            {
                case Direction.FORWARD:
                    y = y - (modifier * 1);
                    break;
                case Direction.BACKWARD:
                    y = y + (modifier * 1);
                    break;
                case Direction.LEFT:
                    x = x - (modifier * 1);
                    break;
                case Direction.RIGHT:
                    x = x + (modifier * 1);
                    break;
                case Direction.DIAGONAL_FORWARD_LEFT:
                    y = y - (modifier * 1);
                    x = x - (modifier * 1);
                    break;
                case Direction.DIAGONAL_FORWARD_RIGHT:
                    y = y - (modifier * 1);
                    x = x + (modifier * 1);
                    break;
                case Direction.DIAGONAL_BACKWARD_LEFT:
                    y = y + (modifier * 1);
                    x = x - (modifier * 1);
                    break;
                case Direction.DIAGONAL_BACKWARD_RIGHT:
                    y = y + (modifier * 1);
                    x = x + (modifier * 1);
                    break;
            }
            return Board.ArrayPosition2FieldCode(x, y);
        }

        public string CalculateStepFieldCode(Direction direction, bool mustHitEnemy, bool intermediateFieldsMustBeEmpty, bool canHitEnemy)
        {
            return CalculateSequenceStepFieldCode(new List<Direction> { direction }, mustHitEnemy, intermediateFieldsMustBeEmpty, canHitEnemy);
        }

        public string CalculateSequenceStepFieldCode(List<Direction> steps, bool mustHitEnemy, bool intermediateFieldsMustBeEmpty, bool canHitEnemy)
        {
            return CalculateSequenceStepFieldCode(this.Board.GetFiguresFieldCode(this), steps, mustHitEnemy, intermediateFieldsMustBeEmpty, canHitEnemy);
        }

        private string CalculateSequenceStepFieldCode(string fromFieldCode, List<Direction> steps, bool mustHitEnemy, bool intermediateFieldsMustBeEmpty, bool canHitEnemy)
        {
            string intermediateFieldCode = fromFieldCode;

            for (int i = 0; i < steps.Count; i++)
            {
                if (intermediateFieldCode == null)
                {
                    return null;
                }

                Direction currentDirection = steps[i];
                intermediateFieldCode = this.TransformStepFieldCode(intermediateFieldCode, currentDirection);

                Figure intermediateFigure = this.Board.GetFigureAtFieldCode(intermediateFieldCode);

                if (intermediateFigure == null)
                {
                    if (mustHitEnemy && i == steps.Count - 1)
                    {
                        return null;
                    }
                }
                else
                {
                    if (intermediateFieldsMustBeEmpty)
                    {
                        if (i < steps.Count - 1)
                        {
                            return null;
                        }
                        else if (i == steps.Count - 1)
                        {
                            if (this.Color == intermediateFigure.Color)
                            {
                                return null;
                            }
                            if (this.Color != intermediateFigure.Color && !canHitEnemy)
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        if (i == steps.Count - 1 && this.Color == intermediateFigure.Color)
                        {
                            return null;
                        }
                        if (i == steps.Count -1 && this.Color != intermediateFigure.Color && !canHitEnemy)
                        {
                            return null;
                        }
                    }
                }
            }
            return intermediateFieldCode;
        }

        public string CalculateKingStepFieldCode(Direction direction, bool hitFieldsOnly)
        {
            string possibleFieldCode = CalculateSequenceStepFieldCode(new List<Direction> { direction }, false, true, true);

            if (!hitFieldsOnly)
            {
                List<string> enemyHitFields = this.Board.Game.Rules.GetEnemyFiguresValidHitFieldsByColor(this.Color);

                // if the enemy hits the desired move, throw the step away
                if (enemyHitFields.Contains(possibleFieldCode))
                {
                    return null;
                }
            }

            return possibleFieldCode;
        }

        public List<string> CalculateIterativeStepFieldCodes(Direction direction)
        {
            List<string> steps = new List<string>();

            string currentStep = "";
            int i = 1;
            // unless there is something in the way, add valid steps
            while (currentStep != null)
            {
                List<Direction> directions = new List<Direction>();
                for (int j = 0; j < i; j++)
                {
                    directions.Add(direction);
                }
                currentStep = CalculateSequenceStepFieldCode(directions, false, true, true);
                steps.Add(currentStep);
                i++;
            }
            return steps;
        }

        public string GetAbbreviation()
        {
            string colorCode = "";
            string figureTypeCode = "";

            switch (this.Color)
            {
                case Color.BLACK:
                    colorCode = "B";
                    break;
                case Color.WHITE:
                    colorCode = "W";
                    break;
            }
            switch (this.FigureType)
            {
                case FigureType.KING:
                    figureTypeCode = "K";
                    break;
                case FigureType.QUEEN:
                    figureTypeCode = "Q";
                    break;
                case FigureType.ROOK:
                    figureTypeCode = "R";
                    break;
                case FigureType.BISHOP:
                    figureTypeCode = "B";
                    break;
                case FigureType.KNIGHT:
                    figureTypeCode = "N";
                    break;
                case FigureType.PAWN:
                    figureTypeCode = "P";
                    break;
            }
            return colorCode + figureTypeCode;
        }

        public override string ToString()
        {
            return "(" + this.Color + ", " + this.FigureType + ")";
        }

        public static Figure BR(Board board)
        {
            return new Figure(board, FigureType.ROOK, Color.BLACK);
        }

        public static Figure BN(Board board)
        {
            return new Figure(board, FigureType.KNIGHT, Color.BLACK);
        }

        public static Figure BB(Board board)
        {
            return new Figure(board, FigureType.BISHOP, Color.BLACK);
        }

        public static Figure BQ(Board board)
        {
            return new Figure(board, FigureType.QUEEN, Color.BLACK);
        }

        public static Figure BK(Board board)
        {
            return new Figure(board, FigureType.KING, Color.BLACK);
        }
        public static Figure BP(Board board)
        {
            return new Figure(board, FigureType.PAWN, Color.BLACK);
        }

        public static Figure WR(Board board)
        {
            return new Figure(board, FigureType.ROOK, Color.WHITE);
        }

        public static Figure WN(Board board)
        {
            return new Figure(board, FigureType.KNIGHT, Color.WHITE);
        }

        public static Figure WB(Board board)
        {
            return new Figure(board, FigureType.BISHOP, Color.WHITE);
        }

        public static Figure WQ(Board board)
        {
            return new Figure(board, FigureType.QUEEN, Color.WHITE);
        }

        public static Figure WK(Board board)
        {
            return new Figure(board, FigureType.KING, Color.WHITE);
        }
        public static Figure WP(Board board)
        {
            return new Figure(board, FigureType.PAWN, Color.WHITE);
        }
    }
}
