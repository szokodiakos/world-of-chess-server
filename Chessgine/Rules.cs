using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    /*
     * http://www.sakk.hu/help/sakk_szabalyok.html
     */
    public class Rules
    {
        private Board board;
        private History history;

        private bool castlingAvailableForWhite;
        private bool castlingAvailableForBlack;

        public Rules(Board board, History history)
        {
            this.board = board;
            this.history = history;
        }

        public List<string> GetValidFields(Figure figure)
        {
            return GetValidOrValidHitFields(figure, false);
        }

        public bool IsCastlingAvailable(Color color)
        {
            bool available = false;

            switch (color)
            {
                case Color.BLACK:
                    available = this.castlingAvailableForBlack;
                    break;
                case Color.WHITE:
                    available = this.castlingAvailableForWhite;
                    break;
            }

            return available;
        }

        public void SetCastlingAvailable(Color color, bool value)
        {
            switch (color)
            {
                case Color.BLACK:
                    this.castlingAvailableForBlack = value;
                    break;
                case Color.WHITE:
                    this.castlingAvailableForWhite = value;
                    break;
            }
        }

        public bool IsDraw(Color upcomingPlayerColor)
        {
            bool isDraw = false;

            if (!IsCheck(upcomingPlayerColor))
            {
                List<Figure> upcomingPlayerFigures = this.board.GetFiguresByColor(upcomingPlayerColor);
                List<string> upcomingPlayerValidFields = new List<string>();

                foreach (var figure in upcomingPlayerFigures)
                {
                    string currentField = board.GetFiguresFieldCode(figure);
                    upcomingPlayerValidFields.AddIfNotNull(this.GetValidFields(figure));
                }

                // if the enemy king isn't in check but he/she can't make a valid step with any of their figures
                if (upcomingPlayerValidFields.Count == 0)
                {
                    isDraw = true;
                }

                List<Figure> previousPlayerFigures = this.board.GetFiguresByColor(Util.GetOppositeColor(upcomingPlayerColor));

                // if only the kings are in game
                if (previousPlayerFigures.Count == 1 && upcomingPlayerFigures.Count == 1)
                {
                    isDraw = true;
                }

                else
                {
                    // if it's a king vs king and (knight || bishop) situation
                    if ((previousPlayerFigures.Count == 2 && upcomingPlayerFigures.Count == 1 &&
                        (previousPlayerFigures.Any(x => x.FigureType == FigureType.KNIGHT) ||
                         previousPlayerFigures.Any(y => y.FigureType == FigureType.BISHOP)))
                        ||
                        (upcomingPlayerFigures.Count == 2 && previousPlayerFigures.Count == 1 &&
                        (upcomingPlayerFigures.Any(x => x.FigureType == FigureType.KNIGHT) ||
                         upcomingPlayerFigures.Any(y => y.FigureType == FigureType.BISHOP))))
                    {
                        isDraw = true;
                    }

                    else
                    {
                        // if it's a king bishop vs king bishop situation and the bishops move on the same colored field
                        if (previousPlayerFigures.Count == 2 && upcomingPlayerFigures.Count == 2 &&
                            previousPlayerFigures.Any(x => x.FigureType == FigureType.BISHOP) &&
                            upcomingPlayerFigures.Any(x => x.FigureType == FigureType.BISHOP))
                        {
                            Figure whiteBishop = this.board.GetFiguresByColorAndType(Color.WHITE, FigureType.BISHOP)[0];
                            Figure blackBishop = this.board.GetFiguresByColorAndType(Color.BLACK, FigureType.BISHOP)[0];

                            List<string> whiteBishopValidHits = this.GetValidHitFields(whiteBishop);
                            List<string> blackBishopValidHits = this.GetValidHitFields(blackBishop);

                            if (whiteBishopValidHits.SectionWith(blackBishopValidHits).Count != 0)
                            {
                                isDraw = true;
                            }
                        }
                    }
                }
            }
            return isDraw;
        }

        public bool IsCheckMate(Color color)
        {
            bool isCheckMate = true;

            if (IsCheck(color))
            {
                Figure king = this.board.GetFiguresByColorAndType(color, FigureType.KING)[0];

                // if the king has valid steps
                if (this.board.Game.Rules.GetValidFields(king).Count > 0)
                {
                    isCheckMate = false;
                }
                else{

                    // if one of our figures can prevent the check situation
                    List<Figure> ourFigures = this.board.GetFiguresByColor(color);

                    // save the current state of fields
                    Dictionary<string, Figure> currentFields = this.board.GetHypotheticalFieldsAfterMove("", "");

                    foreach (var figure in ourFigures)
                    {
                        List<string> validFields = this.GetValidFields(figure);
                        string figuresFieldCode = this.board.GetFiguresFieldCode(figure);

                        foreach (var validField in validFields)
                        {
                            Dictionary<string, Figure> hypotheticalFields =
                                this.board.GetHypotheticalFieldsAfterMove(figuresFieldCode, validField);
                            
                            // swapping the board's fields with a hypothetical one (what if...)
                            this.board.Fields = hypotheticalFields;

                            // if this field change prevents check
                            if (!IsCheck(color))
                            {
                                isCheckMate = false;
                            }

                            // revert fields back to normal
                            this.board.Fields = currentFields;
                        }
                    }
                }
            }
            return isCheckMate;
        }

        public bool IsCheck(Color color)
        {
            bool isCheck = false;
            List<string> enemyHitFields = this.GetEnemyFiguresValidHitFieldsByColor(color);
            Figure kingFigure = board.GetFiguresByColorAndType(color, FigureType.KING)[0];
            string kingFigureFieldCode = board.GetFiguresFieldCode(kingFigure);

            foreach (var item in enemyHitFields)
            {
                if (item == kingFigureFieldCode)
                {
                    isCheck = true;
                }
            }
            return isCheck;
        }

        public List<string> GetEnemyFiguresValidHitFieldsByColor(Color color)
        {
            List<string> validHitFields = new List<string>();
            Color oppositeColor = color == Color.WHITE ? Color.BLACK : Color.WHITE;

            List<Figure> enemyFigures = this.board.GetFiguresByColor(oppositeColor);
            foreach (var enemyFigure in enemyFigures)
            {
                string fieldCode = this.board.GetFiguresFieldCode(enemyFigure);
                validHitFields.AddIfNotNull(this.GetValidHitFields(enemyFigure));
            }

            return validHitFields;
        }

        public List<string> GetValidHitFields(Figure figure)
        {
            List<string> hitFields = GetValidOrValidHitFields(figure, true);

            return hitFields;
        }

        public List<string> GetValidOrValidHitFields(Figure figure, bool hitFieldsOnly)
        {
            List<string> validSteps = new List<string>();
            string currentField = board.GetFiguresFieldCode(figure);
            Tuple<int, int> arrayPosition = Board.FieldCode2ArrayPosition(currentField);
            int x = arrayPosition.Item1;
            int y = arrayPosition.Item2;

            SetCastlingAvailable(figure.Color, false);

            switch (figure.FigureType)
            {
                case FigureType.KING:

                    // 8 directional one-steps (special king method to prevent recursion)
                    validSteps.AddIfNotNull(figure.CalculateKingStepFieldCode(Direction.FORWARD, hitFieldsOnly));
                    validSteps.AddIfNotNull(figure.CalculateKingStepFieldCode(Direction.BACKWARD, hitFieldsOnly));
                    validSteps.AddIfNotNull(figure.CalculateKingStepFieldCode(Direction.LEFT, hitFieldsOnly));
                    validSteps.AddIfNotNull(figure.CalculateKingStepFieldCode(Direction.RIGHT, hitFieldsOnly));
                    validSteps.AddIfNotNull(figure.CalculateKingStepFieldCode(Direction.DIAGONAL_FORWARD_LEFT, hitFieldsOnly));
                    validSteps.AddIfNotNull(figure.CalculateKingStepFieldCode(Direction.DIAGONAL_FORWARD_RIGHT, hitFieldsOnly));
                    validSteps.AddIfNotNull(figure.CalculateKingStepFieldCode(Direction.DIAGONAL_BACKWARD_LEFT, hitFieldsOnly));
                    validSteps.AddIfNotNull(figure.CalculateKingStepFieldCode(Direction.DIAGONAL_BACKWARD_RIGHT, hitFieldsOnly));

                    if (!hitFieldsOnly)
                    {
                        // castling - if available
                        // 1) if the king hasn't moved yet and isn't in check
                        if (!history.HasFigureMovedYet(figure) && !IsCheck(figure.Color))
                        {
                            List<Figure> friendlyRooks = this.board.GetFiguresByColorAndType(figure.Color, FigureType.ROOK);

                            foreach (var rookFigure in friendlyRooks)
                            {

                                // 2) if the rook hasn't moved yet
                                if (!history.HasFigureMovedYet(rookFigure))
                                {
                                    // get fields between king and rook
                                    List<string> intermediateFieldCodes = board.GetHorizontalIntermediateFields(figure, rookFigure);

                                    // check if the fields between the rook and the king isn't in hit by an enemy figure
                                    List<string> validHitFields = this.GetEnemyFiguresValidHitFieldsByColor(rookFigure.Color);

                                    // 3) if no fields are in hit between the rook and the king
                                    if (intermediateFieldCodes.SectionWith(validHitFields).Count == 0)
                                    {
                                        bool emptyIntermediateFields = true;

                                        // 4) if every intermediate fields are empty
                                        foreach (var field in intermediateFieldCodes)
                                        {
                                            if (this.board.GetFigureAtFieldCode(field) != null)
                                            {
                                                emptyIntermediateFields = false;
                                            }
                                        }
                                        if (emptyIntermediateFields)
                                        {
                                            // we can use castling
                                            string rookFigureFieldCode = this.board.GetFiguresFieldCode(rookFigure);

                                            switch (rookFigureFieldCode)
                                            {
                                                case Board.A8:
                                                    // right black rook
                                                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                                                    {
                                                        Direction.RIGHT,
                                                        Direction.RIGHT
                                                    }, false, false, false));
                                                    this.SetCastlingAvailable(Color.BLACK, true);
                                                    break;
                                                case Board.H8:
                                                    // left black rook
                                                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                                                    {
                                                        Direction.LEFT,
                                                        Direction.LEFT
                                                    }, false, false, false));
                                                    this.SetCastlingAvailable(Color.BLACK, true);
                                                    break;
                                                case Board.A1:
                                                    // left white rook
                                                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                                                    {
                                                        Direction.LEFT,
                                                        Direction.LEFT
                                                    }, false, false, false));
                                                    this.SetCastlingAvailable(Color.WHITE, true);
                                                    break;
                                                case Board.H1:
                                                    // right white rook
                                                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                                                    {
                                                        Direction.RIGHT,
                                                        Direction.RIGHT
                                                    }, false, false, false));
                                                    this.SetCastlingAvailable(Color.WHITE, true);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case FigureType.QUEEN:

                    // 8 directional iterative steps
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.FORWARD));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.BACKWARD));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.LEFT));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.RIGHT));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.DIAGONAL_FORWARD_LEFT));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.DIAGONAL_FORWARD_RIGHT));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.DIAGONAL_BACKWARD_LEFT));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.DIAGONAL_BACKWARD_RIGHT));
                    break;
                case FigureType.ROOK:

                    // 4 directional iterative steps
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.FORWARD));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.BACKWARD));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.LEFT));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.RIGHT));
                    break;
                case FigureType.BISHOP:

                    // 4 directional iterative steps
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.DIAGONAL_FORWARD_LEFT));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.DIAGONAL_FORWARD_RIGHT));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.DIAGONAL_BACKWARD_LEFT));
                    validSteps.AddIfNotNull(figure.CalculateIterativeStepFieldCodes(Direction.DIAGONAL_BACKWARD_RIGHT));
                    break;
                case FigureType.KNIGHT:

                    // Eight different "L" moves
                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                        {
                            Direction.FORWARD,
                            Direction.FORWARD,
                            Direction.LEFT
                        }, false, false, true));
                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                        {
                            Direction.FORWARD,
                            Direction.FORWARD,
                            Direction.RIGHT
                        }, false, false, true));
                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                        {
                            Direction.LEFT,
                            Direction.LEFT,
                            Direction.FORWARD
                        }, false, false, true));
                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                        {
                            Direction.LEFT,
                            Direction.LEFT,
                            Direction.BACKWARD
                        }, false, false, true));
                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                        {
                            Direction.BACKWARD,
                            Direction.BACKWARD,
                            Direction.LEFT
                        }, false, false, true));
                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                        {
                            Direction.BACKWARD,
                            Direction.BACKWARD,
                            Direction.RIGHT
                        }, false, false, true));
                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                        {
                            Direction.RIGHT,
                            Direction.RIGHT,
                            Direction.FORWARD
                        }, false, false, true));
                    validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                        {
                            Direction.RIGHT,
                            Direction.RIGHT,
                            Direction.BACKWARD
                        }, false, false, true));
                    break;
                case FigureType.PAWN:
                    if (!hitFieldsOnly)
                    {
                        if (!history.HasFigureMovedYet(figure))
                        {
                            // can take two steps forward
                            validSteps.AddIfNotNull(figure.CalculateSequenceStepFieldCode(new List<Direction>
                        {
                            Direction.FORWARD,
                            Direction.FORWARD
                        }, false, true, false));
                        }

                        // can take one step forward
                        validSteps.AddIfNotNull(figure.CalculateStepFieldCode(Direction.FORWARD, false, true, false));

                        // or hit enemy figure diagonally if exists
                        validSteps.AddIfNotNull(figure.CalculateStepFieldCode(Direction.DIAGONAL_FORWARD_LEFT, true, true, true));
                        validSteps.AddIfNotNull(figure.CalculateStepFieldCode(Direction.DIAGONAL_FORWARD_RIGHT, true, true, true));
                    
                    }
                    else
                    {
                        // or hit enemy figure diagonally if exists
                        validSteps.AddIfNotNull(figure.CalculateStepFieldCode(Direction.DIAGONAL_FORWARD_LEFT, false, true, true));
                        validSteps.AddIfNotNull(figure.CalculateStepFieldCode(Direction.DIAGONAL_FORWARD_RIGHT, false, true, true));
                    }

                    // or hit enemy pawn with enpassant (if available)
                    validSteps.AddIfNotNull(history.GetEnpassantFieldCode(figure));
                    break;
            }

            if (!hitFieldsOnly)
            {
                Logger.Log("Gathered valid steps for figure " + figure.ToString() + " at " + figure.Board.GetFiguresFieldCode(figure) + " are: (" + string.Join(",", validSteps.ToArray()) + ")");
            }

            // save the current state of fields
            Dictionary<string, Figure> originalFields = this.board.GetHypotheticalFieldsAfterMove("", "");

            if (!hitFieldsOnly)
            {
                // remove steps which would check our king
                var index = 0;
                while (index < validSteps.Count)
                {
                    string validStep = validSteps[index];
                    Dictionary<string, Figure> hypotheticalFields = board.GetHypotheticalFieldsAfterMove(currentField, validStep);
                    this.board.Fields = hypotheticalFields;
                    if (IsCheck(figure.Color))
                    {
                        validSteps.RemoveAt(index);
                    }
                    else
                    {
                        index++;
                    }
                    this.board.Fields = originalFields;
                }
            }
            return validSteps;
        }
    }
}
