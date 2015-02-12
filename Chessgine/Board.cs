using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessgine
{
    /*
     *      R - Rook
     *      N - kNight
     *      B - Bishop
     *      Q - Queen
     *      K - King
     *      P - Pawn
     * 
     * 
     *      B - Black
     *      W - White
     * 
     *       a   b   c   d   e   f   g   h 
     *       ------------------------------
     *   8 | BR  BN  BB  BQ  BK  BB  BN  BR | 8
     *   7 | BP  BP  BP  BP  BP  BP  BP  BP | 7
     *   6 |                                | 6
     *   5 |                                | 5
     *   4 |                                | 4
     *   3 |                                | 3
     *   2 | WP  WP  WP  WP  WP  WP  WP  WP | 2
     *   1 | WR  WN  WB  WQ  WK  WB  WN  WR | 1
     *       ------------------------------
     *       a   b   c   d   e   f   g   h
     */
    public class Board
    {
        public const string A1 = "A1";
        public const string A2 = "A2";
        public const string A3 = "A3";
        public const string A4 = "A4";
        public const string A5 = "A5";
        public const string A6 = "A6";
        public const string A7 = "A7";
        public const string A8 = "A8";
        public const string B1 = "B1";
        public const string B2 = "B2";
        public const string B3 = "B3";
        public const string B4 = "B4";
        public const string B5 = "B5";
        public const string B6 = "B6";
        public const string B7 = "B7";
        public const string B8 = "B8";
        public const string C1 = "C1";
        public const string C2 = "C2";
        public const string C3 = "C3";
        public const string C4 = "C4";
        public const string C5 = "C5";
        public const string C6 = "C6";
        public const string C7 = "C7";
        public const string C8 = "C8";
        public const string D1 = "D1";
        public const string D2 = "D2";
        public const string D3 = "D3";
        public const string D4 = "D4";
        public const string D5 = "D5";
        public const string D6 = "D6";
        public const string D7 = "D7";
        public const string D8 = "D8";
        public const string E1 = "E1";
        public const string E2 = "E2";
        public const string E3 = "E3";
        public const string E4 = "E4";
        public const string E5 = "E5";
        public const string E6 = "E6";
        public const string E7 = "E7";
        public const string E8 = "E8";
        public const string F1 = "F1";
        public const string F2 = "F2";
        public const string F3 = "F3";
        public const string F4 = "F4";
        public const string F5 = "F5";
        public const string F6 = "F6";
        public const string F7 = "F7";
        public const string F8 = "F8";
        public const string G1 = "G1";
        public const string G2 = "G2";
        public const string G3 = "G3";
        public const string G4 = "G4";
        public const string G5 = "G5";
        public const string G6 = "G6";
        public const string G7 = "G7";
        public const string G8 = "G8";
        public const string H1 = "H1";
        public const string H2 = "H2";
        public const string H3 = "H3";
        public const string H4 = "H4";
        public const string H5 = "H5";
        public const string H6 = "H6";
        public const string H7 = "H7";
        public const string H8 = "H8";


        private Dictionary<string, Figure> fields;
        public Dictionary<string, Figure> Fields
        {
            get
            {
                return fields;
            }
            set
            {
                this.fields = value;
            }
        }
        public Game Game { get; set; }
        public History History { get; set; }

        public delegate void GameEventHandler(object myObject, GameArgs myArgs);
        public event GameEventHandler OnGameEvent;

        public Board(Game game, History history)
        {
            fields = new Dictionary<string, Figure>();

            fields.Add(Board.A8, new Figure(this, FigureType.ROOK, Color.BLACK));
            fields.Add(Board.H8, new Figure(this, FigureType.ROOK, Color.BLACK));
            fields.Add(Board.A1, new Figure(this, FigureType.ROOK, Color.WHITE));
            fields.Add(Board.H1, new Figure(this, FigureType.ROOK, Color.WHITE));
                                            
            fields.Add(Board.B8, new Figure(this, FigureType.KNIGHT, Color.BLACK));
            fields.Add(Board.G8, new Figure(this, FigureType.KNIGHT, Color.BLACK));
            fields.Add(Board.B1, new Figure(this, FigureType.KNIGHT, Color.WHITE));
            fields.Add(Board.G1, new Figure(this, FigureType.KNIGHT, Color.WHITE));
                                            
            fields.Add(Board.C8, new Figure(this, FigureType.BISHOP, Color.BLACK));
            fields.Add(Board.F8, new Figure(this, FigureType.BISHOP, Color.BLACK));
            fields.Add(Board.C1, new Figure(this, FigureType.BISHOP, Color.WHITE));
            fields.Add(Board.F1, new Figure(this, FigureType.BISHOP, Color.WHITE));
                                             
            fields.Add(Board.D8, new Figure(this, FigureType.QUEEN, Color.BLACK));
            fields.Add(Board.D1, new Figure(this, FigureType.QUEEN, Color.WHITE));
                                           
            fields.Add(Board.E8, new Figure(this, FigureType.KING, Color.BLACK));
            fields.Add(Board.E1, new Figure(this, FigureType.KING, Color.WHITE));
                                             
            fields.Add(Board.A7, new Figure(this, FigureType.PAWN, Color.BLACK));
            fields.Add(Board.B7, new Figure(this, FigureType.PAWN, Color.BLACK));
            fields.Add(Board.C7, new Figure(this, FigureType.PAWN, Color.BLACK));
            fields.Add(Board.D7, new Figure(this, FigureType.PAWN, Color.BLACK));
            fields.Add(Board.E7, new Figure(this, FigureType.PAWN, Color.BLACK));
            fields.Add(Board.F7, new Figure(this, FigureType.PAWN, Color.BLACK));
            fields.Add(Board.G7, new Figure(this, FigureType.PAWN, Color.BLACK));
            fields.Add(Board.H7, new Figure(this, FigureType.PAWN, Color.BLACK));
            fields.Add(Board.A2, new Figure(this, FigureType.PAWN, Color.WHITE));
            fields.Add(Board.B2, new Figure(this, FigureType.PAWN, Color.WHITE));
            fields.Add(Board.C2, new Figure(this, FigureType.PAWN, Color.WHITE));
            fields.Add(Board.D2, new Figure(this, FigureType.PAWN, Color.WHITE));
            fields.Add(Board.E2, new Figure(this, FigureType.PAWN, Color.WHITE));
            fields.Add(Board.F2, new Figure(this, FigureType.PAWN, Color.WHITE));
            fields.Add(Board.G2, new Figure(this, FigureType.PAWN, Color.WHITE));
            fields.Add(Board.H2, new Figure(this, FigureType.PAWN, Color.WHITE));

            this.Game = game;
            this.History = history;
        }

        public Figure GetFigureAtFieldCode(string fieldCode)
        {
            if (fieldCode == null || !fields.ContainsKey(fieldCode))
            {
                return null;
            }
            return fields[fieldCode];
        }

        public void MoveFigure(string toFieldCode, Figure whichFigure)
        {
            if (Game.GameStatus == false)
            {
                throw new GameIsNotStartedException();
            }

            List<string> validSteps = this.Game.Rules.GetValidFields(whichFigure);

            if (!validSteps.Contains(toFieldCode))
            {
                throw new InvalidFigureMoveException();
            }

            Logger.Log("Moving figure "+ whichFigure.ToString() +" from " + whichFigure.Board.GetFiguresFieldCode(whichFigure) + " to " + toFieldCode + ".");

            this.Game.CurrentTurn.FromFieldCode = GetFiguresFieldCode(whichFigure);
            this.Game.CurrentTurn.ToFieldCode = toFieldCode; 

            // check if castling move was chosen
            if (whichFigure.FigureType == FigureType.KING && this.Game.Rules.IsCastlingAvailable(whichFigure.Color))
            {

                Logger.Log("Castling for " + whichFigure.Color);

                this.Game.Rules.SetCastlingAvailable(whichFigure.Color, false);
                string removeRookFrom = "";
                string addRookTo = "";

                // move the respective rook to its correct position
                switch (toFieldCode)
                {
                    case Board.G8:
                        removeRookFrom = Board.H8;
                        addRookTo = Board.F8;
                        break;
                
                    case Board.C8:
                        removeRookFrom = Board.A8;
                        addRookTo = Board.D8;
                        break;
                
                    case Board.G1:
                        removeRookFrom = Board.H1;
                        addRookTo = Board.F1;
                        break;
                
                    case Board.C1:
                        removeRookFrom = Board.A1;
                        addRookTo = Board.D1;
                        break;
                    default:
                        break;
                }
                fields.Remove(removeRookFrom);
                fields[addRookTo] = new Figure(this, FigureType.ROOK, whichFigure.Color);

                JObject message = new JObject();
                message["fromx"] = 7 - Board.FieldCode2ArrayPosition(removeRookFrom).Item1;
                message["fromy"] = 7 - Board.FieldCode2ArrayPosition(removeRookFrom).Item2;
                message["tox"] = 7 - Board.FieldCode2ArrayPosition(addRookTo).Item1;
                message["toy"] = 7 - Board.FieldCode2ArrayPosition(addRookTo).Item2;
                if (OnGameEvent != null)
                {
                    OnGameEvent(this.Game, new GameArgs("Castling", message.ToString()));
                }
            }

            // our figure hits enemy
            if (fields.ContainsKey(toFieldCode) && fields[toFieldCode] != null)
            {
                Logger.Log("Figure " + whichFigure.ToString() + " hits figure " + fields[toFieldCode].ToString() + ".");

                fields.Remove(toFieldCode);
            }
            else
            {
                // enpassant move happened
                if (this.History.GetEnpassantFieldCode(whichFigure) == toFieldCode)
                {
                    int x = Board.FieldCode2ArrayPosition(toFieldCode).Item1;
                    int y = Board.FieldCode2ArrayPosition(toFieldCode).Item2;

                    switch (whichFigure.Color)
                    {
                        case Color.BLACK:

                            // the killed enemy figure is behind (== -1 index in the 2D array) our figure
                            y--;

                            break;
                        case Color.WHITE:
                            
                            // the killed enemy figure is behind (== +1 index in the 2D array) our figure
                            y++;

                            break;
                        default:
                            break;
                    }

                    string hitEnemyFigureFieldCode = Board.ArrayPosition2FieldCode(x, y);

                    JObject message = new JObject();
                    message["x"] = 7 - Board.FieldCode2ArrayPosition(hitEnemyFigureFieldCode).Item1;
                    message["y"] = 7 - Board.FieldCode2ArrayPosition(hitEnemyFigureFieldCode).Item2;
                    if (OnGameEvent != null)
                    {
                        OnGameEvent(this.Game, new GameArgs("Enpassant", message.ToString()));
                    }
                    Logger.Log("Enpassant hit: figure " + whichFigure.ToString() + " hits figure " + fields[hitEnemyFigureFieldCode].ToString() + ".");

                    fields.Remove(hitEnemyFigureFieldCode);
                }
            }

            string oldFieldCode = GetFiguresFieldCode(whichFigure);
            fields.Remove(GetFiguresFieldCode(whichFigure));
            fields[toFieldCode] = whichFigure;

            // checking if pawn promotion is available
            if (whichFigure.FigureType == FigureType.PAWN)
            {
                int y = Board.FieldCode2ArrayPosition(toFieldCode).Item2;
                FigureType figureTypePreference = FigureType.QUEEN;

                switch (whichFigure.Color)
                {
                    case Color.BLACK:

                        // black pawn promotion
                        if (y == 7)
                        {
                            figureTypePreference = this.Game.BlackPlayer.PawnPromotionPreference;
                            fields[toFieldCode] = new Figure(this.Game.Board, figureTypePreference, Color.BLACK);

                            JObject message = new JObject();
                            message["x"] = 7 - Board.FieldCode2ArrayPosition(oldFieldCode).Item1;
                            message["y"] = 7 - Board.FieldCode2ArrayPosition(oldFieldCode).Item2;
                            message["figurePreference"] = figureTypePreference.ToString();
                            OnGameEvent(this.Game, new GameArgs("PawnPromotion", message.ToString()));
                            Logger.Log(whichFigure.Color + " pawn at " + toFieldCode + " promoted to figure: " + figureTypePreference + ".");
                        }
                        break;
                    case Color.WHITE:

                        // white pawn promotion
                        if (y == 0)
                        {
                            figureTypePreference = this.Game.WhitePlayer.PawnPromotionPreference;
                            fields[toFieldCode] = new Figure(this.Game.Board, figureTypePreference, Color.WHITE);

                            JObject message = new JObject();
                            message["x"] = 7 - Board.FieldCode2ArrayPosition(oldFieldCode).Item1;
                            message["y"] = 7 - Board.FieldCode2ArrayPosition(oldFieldCode).Item2;
                            message["figurePreference"] = figureTypePreference.ToString();
                            if (OnGameEvent != null)
                            {
                                OnGameEvent(this.Game, new GameArgs("PawnPromotion", message.ToString()));
                            }
                            Logger.Log(whichFigure.Color + " pawn at " + toFieldCode + " promoted to figure: " + figureTypePreference + ".");
                        }
                        break;
                    default:
                        break;
                }
            }

            Color enemyColor = Util.GetOppositeColor(whichFigure.Color);

            // check if this move induced a check
            if (this.Game.Rules.IsCheck(enemyColor))
            {

                // check if this move induced a checkmate
                if (this.Game.Rules.IsCheckMate(enemyColor))
                {
                    if (OnGameEvent != null)
                    {
                        OnGameEvent(this.Game, new GameArgs("CheckMate", enemyColor.ToString()));
                    }
                    this.Game.gameFinished(whichFigure.Color);
                }
                else
                {
                    // if the king can move from check, it's only check
                    Logger.Log("Check for " + enemyColor + "!");
                    if (OnGameEvent != null)
                    {
                        OnGameEvent(this.Game, new GameArgs("Check", enemyColor.ToString()));
                    }
                }

            }
            else
            {
                if(this.Game.Rules.IsDraw(enemyColor)){
                    this.Game.gameFinished(Color.NONE);
                    if (OnGameEvent != null)
                    {
                        OnGameEvent(this.Game, new GameArgs("Draw", ""));
                    }
                };
            }

            this.Game.endTurn();
        }

        public string GetFiguresFieldCode(Figure figure)
        {
            string fieldCode = this.fields.FirstOrDefault(x => x.Value.Equals(figure)).Key;
            return fieldCode;
        }

        public Figure[,] Board2Array()
        {
            Figure[,] fieldArray = new Figure[8, 8];
            foreach (var key in this.fields.Keys)
            {
                Tuple<int,int> arrayPosition = FieldCode2ArrayPosition(key);
                fieldArray[arrayPosition.Item1, arrayPosition.Item2] = this.fields[key];
            }
            return fieldArray;
        }

        public static string ArrayPosition2FieldCode(int x, int y)
        {
            return ArrayPosition2FieldCode(new Tuple<int, int>(x, y));
        }

        /*
         *     x -->
         *  y
         *  |
         *  V
         * 
         */
        public static string ArrayPosition2FieldCode(Tuple<int, int> arrayPosition)
        {
            string fieldCode = "";
            int x = arrayPosition.Item1;
            int y = arrayPosition.Item2;

            if (0 <= x && x <= 7 && 0 <= y && y <= 7)
            {
                switch (x)
                {
                    case 0:
                        fieldCode += "A";
                        break;
                    case 1:
                        fieldCode += "B";
                        break;
                    case 2:
                        fieldCode += "C";
                        break;
                    case 3:
                        fieldCode += "D";
                        break;
                    case 4:
                        fieldCode += "E";
                        break;
                    case 5:
                        fieldCode += "F";
                        break;
                    case 6:
                        fieldCode += "G";
                        break;
                    case 7:
                        fieldCode += "H";
                        break;
                }
                switch (y)
                {
                    case 0:
                        fieldCode += "8";
                        break;
                    case 1:
                        fieldCode += "7";
                        break;
                    case 2:
                        fieldCode += "6";
                        break;
                    case 3:
                        fieldCode += "5";
                        break;
                    case 4:
                        fieldCode += "4";
                        break;
                    case 5:
                        fieldCode += "3";
                        break;
                    case 6:
                        fieldCode += "2";
                        break;
                    case 7:
                        fieldCode += "1";
                        break;
                }
            }
            else
            {
                return null;
            }

            // Logger.Log("Converted arrayPosition: (" + x + ", " + y + ") to fieldCode: " + fieldCode + ".");

            return fieldCode;
        }
        public static Tuple<int,int> FieldCode2ArrayPosition(string fieldCode)
        {
            int x = 0;
            int y = 0;

            if (fieldCode.Length == 2 && 'A' <= fieldCode[0] && fieldCode[0] <= 'H' && '1' <= fieldCode[1] && fieldCode[1] <= '8')
            {
                switch (fieldCode[0])
                {
                    case 'A':
                        x = 0;
                        break;
                    case 'B':
                        x = 1;
                        break;
                    case 'C':
                        x = 2;
                        break;
                    case 'D':
                        x = 3;
                        break;
                    case 'E':
                        x = 4;
                        break;
                    case 'F':
                        x = 5;
                        break;
                    case 'G':
                        x = 6;
                        break;
                    case 'H':
                        x = 7;
                        break;
                }
                switch (fieldCode[1])
                {
                    case '8':
                        y = 0;
                        break;
                    case '7':
                        y = 1;
                        break;
                    case '6':
                        y = 2;
                        break;
                    case '5':
                        y = 3;
                        break;
                    case '4':
                        y = 4;
                        break;
                    case '3':
                        y = 5;
                        break;
                    case '2':
                        y = 6;
                        break;
                    case '1':
                        y = 7;
                        break;
                }
            }
            else
            {
                return null;
            }

            // Logger.Log("Converted fieldCode: " + fieldCode + " to arrayPosition: (" + x + ", " + y + ").");

            return new Tuple<int, int>(x, y);
        }
        public override string ToString()
        {
            string output = "\n";
            output += "  A   B   C   D   E   F   G   H\n";
            output += "  ------------------------------\n";
            Figure[,] figureArray = Board2Array();

            for (int i = 0; i < figureArray.GetLength(0); i++)
            {
                output += figureArray.GetLength(0) - i + "|";
                for (int j = 0; j < figureArray.GetLength(1); j++)
                {
                    Figure currentFigure = figureArray[j, i];
                    if (currentFigure != null)
                    {
                        output += currentFigure.GetAbbreviation();

                        if (j < figureArray.GetLength(1) - 1)
                        {
                            output += "  ";
                        }
                    }
                    else
                    {
                        if (j < figureArray.GetLength(1) - 1)
                        {
                            output += "    ";
                        }
                        else
                        {
                            output += "  ";
                        }
                    }
                }
                if (i < figureArray.GetLength(0) - 1)
                {
                    output += "\n";
                }
            }
            return output;
        }

        public List<Figure> GetFiguresByColorAndType(Color color, FigureType figureType)
        {
            List<Figure> selectedFigures = new List<Figure>();

            foreach (var key in fields.Keys)
            {
                if (fields[key] != null && fields[key].Color == color && fields[key].FigureType == figureType)
                {
                    selectedFigures.Add(fields[key]);
                }
            }

            return selectedFigures;
        }

        public List<Figure> GetFiguresByColor(Color color)
        {
            List<Figure> selectedFigures = new List<Figure>();

            foreach (var key in fields.Keys)
            {
                if (fields[key] != null && fields[key].Color == color)
                {
                    selectedFigures.Add(fields[key]);
                }
            }

            return selectedFigures;
        }

        public List<string> GetHorizontalIntermediateFields(Figure figureStart, Figure figureEnd)
        {
            List<string> intermediateFieldCodes = new List<string>();
            string fieldStart = this.GetFiguresFieldCode(figureStart);
            string fieldEnd = this.GetFiguresFieldCode(figureEnd);

            Tuple<int, int> fieldStartArrayPosition = Board.FieldCode2ArrayPosition(fieldStart);
            Tuple<int, int> fieldEndArrayPosition = Board.FieldCode2ArrayPosition(fieldEnd);

            // if they aren't horizontal throw it away
            if (fieldStartArrayPosition.Item2 != fieldEndArrayPosition.Item2)
            {
                return null;
            }

            // needed for the for-cycle
            int smallerXPosition = Math.Min(fieldStartArrayPosition.Item1, fieldEndArrayPosition.Item1);
            int biggerXPosition = Math.Max(fieldStartArrayPosition.Item1, fieldEndArrayPosition.Item1);

            for (int i = smallerXPosition + 1; i < biggerXPosition; i++)
            {
                string intermediateFieldCode = Board.ArrayPosition2FieldCode(i, fieldStartArrayPosition.Item2);
                intermediateFieldCodes.Add(intermediateFieldCode);
            }

            return intermediateFieldCodes;
        }

        public void parseBoard(Figure[,] array)
        {
            Dictionary<string, Figure> newFields = new Dictionary<string, Figure>();

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Figure currentFigure = array[j, i];
                    String currentFieldCode = Board.ArrayPosition2FieldCode(i, j);

                    if (currentFigure != null)
                    {
                        Logger.Log("Adding figure " + currentFigure.ToString() + " to field " + currentFieldCode);
                        newFields[currentFieldCode] = currentFigure;
                    }
                }
            }

            this.fields = newFields;
        }

        public Dictionary<string, Figure> GetHypotheticalFieldsAfterMove(string fromFieldCode, string toFieldCode)
        {
            var hypotheticalFields = fields.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

            if (fromFieldCode != "" && toFieldCode != "")
            {
                hypotheticalFields[toFieldCode] = this.GetFigureAtFieldCode(fromFieldCode);
                hypotheticalFields.Remove(fromFieldCode);
            }
            return hypotheticalFields;
        }
    }
}
