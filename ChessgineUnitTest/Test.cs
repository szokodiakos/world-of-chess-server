using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chessgine;
using System.Collections.Generic;

namespace ChessgineUnitTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestPawnDoubleStep()
        {
            // arrange
            string expectedResult =  "\n"
                                    +"  A   B   C   D   E   F   G   H\n"
                                    +"  ------------------------------\n"
                                    +"8|BR  BN  BB  BQ  BK  BB  BN  BR\n"
                                    +"7|BP  BP  BP  BP  BP  BP  BP  BP\n"
                                    +"6|                              \n"
                                    +"5|                              \n"
                                    +"4|WP                            \n"
                                    +"3|                              \n"
                                    +"2|    WP  WP  WP  WP  WP  WP  WP\n"
                                    +"1|WR  WN  WB  WQ  WK  WB  WN  WR";

            Game game = new Game(20);

            // act
            // test multiple selection before move
            game.Start();
            game.WhitePlayer.SelectField(Board.B2);
            game.WhitePlayer.SelectField(Board.A2);

            List<string> validSteps = game.Rules.GetValidFields(game.WhitePlayer.GetSelectedFigure());
            if (validSteps.Count > 0)
            {
                game.WhitePlayer.MoveSelectedFigureTo(validSteps[0]);
            }
            string gameBoard = game.Board.ToString();

            // assert
            Assert.AreEqual(expectedResult, gameBoard);
        }

        [TestMethod]
        [ExpectedException(typeof(EnemyFigureSelectedException))]
        public void WhitePlayerSelectsBlackFigure()
        {
            // arrange
            Game game = new Game(20);

            // act
            // select a black rook
            game.Start();
            game.WhitePlayer.SelectField(Board.A8);
        }

        [TestMethod]
        [ExpectedException(typeof(NoFigureOnFieldException))]
        public void WhitePlayerSelectsEmptyField()
        {
            // arrange
            Game game = new Game(20);

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.E5);
            game.WhitePlayer.MoveSelectedFigureTo(Board.E6);
        }

        [TestMethod]
        [ExpectedException(typeof(OtherPlayerTurnsException))]
        public void BlackPlayerTriesToMoveFirst()
        {
            // arrange
            Game game = new Game(20);

            // act
            // select a black rook
            game.Start();
            game.BlackPlayer.SelectField(Board.A8);

        }

        [TestMethod]
        [ExpectedException(typeof(NoSelectedFigureForPlayerException))]
        public void WhitePlayerMovesWithoutSelectingFigure()
        {
            // arrange
            Game game = new Game(20);

            // act
            game.Start();
            game.WhitePlayer.MoveSelectedFigureTo(Board.A5);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFigureMoveException))]
        public void WhitePlayerTriesToMoveInvalid()
        {
            // arrange
            Game game = new Game(20);

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.A2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.A5);
        }

        [TestMethod]
        [ExpectedException(typeof(OtherPlayerTurnsException))]
        public void WhitePlayerTriesToMoveAfterFirstMove()
        {
            // arrange
            Game game = new Game(20);

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.A2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.A4);
            game.WhitePlayer.SelectField(Board.B2);
        }

        [TestMethod]
        public void WhitePawnHitsBlackPawn()
        {
            // arrange
            Game game = new Game(20);
            string expectedResult = "\n"
                                    + "  A   B   C   D   E   F   G   H\n"
                                    + "  ------------------------------\n"
                                    + "8|BR  BN  BB  BQ  BK  BB  BN  BR\n"
                                    + "7|BP      BP  BP  BP  BP  BP  BP\n"
                                    + "6|                              \n"
                                    + "5|    WP                        \n"
                                    + "4|                              \n"
                                    + "3|                              \n"
                                    + "2|    WP  WP  WP  WP  WP  WP  WP\n"
                                    + "1|WR  WN  WB  WQ  WK  WB  WN  WR";

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.A2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.A4);

            game.BlackPlayer.SelectField(Board.B7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.B5);

            game.WhitePlayer.SelectField(Board.A4);
            game.WhitePlayer.MoveSelectedFigureTo(Board.B5);

            string gameBoard = game.Board.ToString();

            // assert
            Assert.AreEqual(expectedResult, gameBoard);
        }

        [TestMethod]
        public void BlackBishopHitsWhiteBishop()
        {

            // arrange
            Game game = new Game(20);
            string expectedResult = "\n"
                                    + "  A   B   C   D   E   F   G   H\n"
                                    + "  ------------------------------\n"
                                    + "8|BR  BN  BB  BQ  BK      BN  BR\n"
                                    + "7|BP  BP  BP  BP  BP  BP      BP\n"
                                    + "6|                        BP  BB\n"
                                    + "5|                              \n"
                                    + "4|                              \n"
                                    + "3|            WP                \n"
                                    + "2|WP  WP  WP      WP  WP  WP  WP\n"
                                    + "1|WR  WN      WQ  WK  WB  WN  WR";

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.D2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.D3);

            game.BlackPlayer.SelectField(Board.G7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.G6);

            game.WhitePlayer.SelectField(Board.C1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.H6);

            game.BlackPlayer.SelectField(Board.F8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H6);

            string gameBoard = game.Board.ToString();

            // assert
            Assert.AreEqual(expectedResult, gameBoard);
        }

        [TestMethod]
        public void BlackPawnHitsWhitePawnEnpassant()
        {
            // arrange
            Game game = new Game(20);
            string expectedResult = "\n"
                                    + "  A   B   C   D   E   F   G   H\n"
                                    + "  ------------------------------\n"
                                    + "8|BR  BN  BB  BQ  BK  BB  BN  BR\n"
                                    + "7|BP      BP  BP  BP  BP  BP  BP\n"
                                    + "6|                              \n"
                                    + "5|                              \n"
                                    + "4|                            WP\n"
                                    + "3|        BP                    \n"
                                    + "2|WP  WP      WP  WP  WP  WP    \n"
                                    + "1|WR  WN  WB  WQ  WK  WB  WN  WR";
            

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.H2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.H3);

            game.BlackPlayer.SelectField(Board.B7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.B5);

            game.WhitePlayer.SelectField(Board.H3);
            game.WhitePlayer.MoveSelectedFigureTo(Board.H4);

            game.BlackPlayer.SelectField(Board.B5);
            game.BlackPlayer.MoveSelectedFigureTo(Board.B4);

            game.WhitePlayer.SelectField(Board.C2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.C4);

            game.BlackPlayer.SelectField(Board.B4);
            game.BlackPlayer.MoveSelectedFigureTo(Board.C3);

            string gameBoard = game.Board.ToString();

            // assert
            Assert.AreEqual(expectedResult, gameBoard);
        }

        [TestMethod]
        public void WhitePawnGetsPromoted()
        {
            // arrange
            Game game = new Game(20);
            Board board = game.Board;
            Figure[,] predefinedBoard = new Figure[8, 8] {
                {Figure.BR(board), null            , Figure.BB(board), Figure.BQ(board), Figure.BK(board), Figure.BB(board), Figure.BN(board), Figure.BR(board)},
                {Figure.BP(board), null            , Figure.BP(board), Figure.BP(board), Figure.BP(board), Figure.BP(board), Figure.BP(board), Figure.BP(board)},
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {Figure.WP(board), Figure.WP(board), Figure.WP(board), Figure.WP(board), Figure.WP(board), Figure.WP(board), Figure.WP(board), Figure.WP(board)},
                {Figure.WR(board), Figure.WN(board), Figure.WB(board), Figure.WQ(board), Figure.WK(board), Figure.WB(board), Figure.WN(board), Figure.WR(board)}
            };
            game.Board.parseBoard(predefinedBoard);
            string expectedResult = "\n"
                                    + "  A   B   C   D   E   F   G   H\n"
                                    + "  ------------------------------\n"
                                    + "8|BR  WQ  BB  BQ  BK  BB  BN  BR\n"
                                    + "7|BP      BP  BP  BP  BP        \n"
                                    + "6|                        BP    \n"
                                    + "5|                              \n"
                                    + "4|                              \n"
                                    + "3|                            BP\n"
                                    + "2|WP      WP  WP  WP  WP  WP  WP\n"
                                    + "1|WR  WN  WB  WQ  WK  WB  WN  WR";

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.B2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.B4);

            game.BlackPlayer.SelectField(Board.H7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H6);

            game.WhitePlayer.SelectField(Board.B4);
            game.WhitePlayer.MoveSelectedFigureTo(Board.B5);

            game.BlackPlayer.SelectField(Board.H6);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H5);

            game.WhitePlayer.SelectField(Board.B5);
            game.WhitePlayer.MoveSelectedFigureTo(Board.B6);

            game.BlackPlayer.SelectField(Board.H5);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H4);

            game.WhitePlayer.SelectField(Board.B6);
            game.WhitePlayer.MoveSelectedFigureTo(Board.B7);

            game.BlackPlayer.SelectField(Board.H4);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H3);

            game.WhitePlayer.PawnPromotionPreference = FigureType.QUEEN;
            game.WhitePlayer.SelectField(Board.B7);
            game.WhitePlayer.MoveSelectedFigureTo(Board.B8);

            game.BlackPlayer.SelectField(Board.G7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.G6);

            string gameBoard = game.Board.ToString();

            // assert
            Assert.AreEqual(expectedResult, gameBoard);
        }

        [TestMethod]
        public void BothPlayersUseCastling()
        {
            // arrange
            Game game = new Game(20);
            Board board = game.Board;
            Figure[,] predefinedBoard = new Figure[8, 8] {
                {Figure.BR(board), Figure.BN(board), Figure.BB(board), Figure.BQ(board), Figure.BK(board), Figure.BB(board), Figure.BN(board), Figure.BR(board)},
                {Figure.BP(board), null            , Figure.BP(board), null            , Figure.BP(board), Figure.BP(board), Figure.BP(board), Figure.BP(board)},
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {Figure.WP(board), Figure.WP(board), Figure.WP(board), Figure.WP(board), Figure.WP(board), Figure.WP(board), null,             Figure.WP(board)},
                {Figure.WR(board), Figure.WN(board), Figure.WB(board), Figure.WQ(board), Figure.WK(board), Figure.WB(board), Figure.WN(board), Figure.WR(board)}
            };
            game.Board.parseBoard(predefinedBoard);
            string expectedResult = "\n"
                                    + "  A   B   C   D   E   F   G   H\n"
                                    + "  ------------------------------\n"
                                    + "8|        BK  BR      BB  BN  BR\n"
                                    + "7|BP      BP  BQ  BP  BP  BP  BP\n"
                                    + "6|BN                            \n"
                                    + "5|                              \n"
                                    + "4|                              \n"
                                    + "3|WP                          WN\n"
                                    + "2|WR  WP  WP  WP  WP  WP      WP\n"
                                    + "1|    WN  WB  WQ      WR  WK    ";

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.A2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.A3);

            game.BlackPlayer.SelectField(Board.B8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.A6);

            game.WhitePlayer.SelectField(Board.F1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.H3);

            game.BlackPlayer.SelectField(Board.C8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H3);

            game.WhitePlayer.SelectField(Board.G1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.H3);

            game.BlackPlayer.SelectField(Board.D8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.D7);

            game.WhitePlayer.SelectField(Board.A1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.A2);

            // black castling (long)
            game.BlackPlayer.SelectField(Board.E8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.C8);

            // white castling (short)
            game.WhitePlayer.SelectField(Board.E1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.G1);

            string gameBoard = game.Board.ToString();

            // assert
            Assert.AreEqual(expectedResult, gameBoard);
        }

        [TestMethod]
        public void BlackPlayerChecksWhite()
        {
            // arrange
            Game game = new Game(20);

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.H2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.H3);

            game.BlackPlayer.SelectField(Board.B8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.A6);

            game.WhitePlayer.SelectField(Board.G2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.G3);

            game.BlackPlayer.SelectField(Board.A6);
            game.BlackPlayer.MoveSelectedFigureTo(Board.B4);

            game.WhitePlayer.SelectField(Board.A2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.A3);

            game.BlackPlayer.SelectField(Board.B4);
            game.BlackPlayer.MoveSelectedFigureTo(Board.D3);

            // assert
            Assert.AreEqual(true, game.Rules.IsCheck(Color.WHITE));
        }

        [TestMethod]
        public void BlackPlayerFoolMatesWhite()
        {
            // arrange
            Game game = new Game(20);

            // act
            game.Start();
            game.WhitePlayer.SelectField(Board.F2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.F3);

            game.BlackPlayer.SelectField(Board.E7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.E5);

            game.WhitePlayer.SelectField(Board.G2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.G4);

            game.BlackPlayer.SelectField(Board.D8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H4);

            // assert
            Assert.AreEqual(true, game.Rules.IsCheckMate(Color.WHITE));
        }

        /**
         * http://sakk.chess.com/forum/view/fun-with-chess/fastest-stalemate
         **/
        [TestMethod]
        public void StaleMateDraw()
        {
            // arrange
            Game game = new Game(20);
            
            // act
            game.Start();

            game.WhitePlayer.SelectField(Board.C2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.C4);

            game.BlackPlayer.SelectField(Board.H7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H5);

            game.WhitePlayer.SelectField(Board.H2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.H4);

            game.BlackPlayer.SelectField(Board.A7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.A5);

            game.WhitePlayer.SelectField(Board.D1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.A4);

            game.BlackPlayer.SelectField(Board.A8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.A6);

            game.WhitePlayer.SelectField(Board.A4);
            game.WhitePlayer.MoveSelectedFigureTo(Board.A5);

            game.BlackPlayer.SelectField(Board.A6);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H6);

            game.WhitePlayer.SelectField(Board.A5);
            game.WhitePlayer.MoveSelectedFigureTo(Board.C7);

            game.BlackPlayer.SelectField(Board.F7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.F6);

            game.WhitePlayer.SelectField(Board.C7);
            game.WhitePlayer.MoveSelectedFigureTo(Board.D7);

            game.BlackPlayer.SelectField(Board.E8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.F7);

            game.WhitePlayer.SelectField(Board.D7);
            game.WhitePlayer.MoveSelectedFigureTo(Board.B7);

            game.BlackPlayer.SelectField(Board.D8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.D3);

            game.WhitePlayer.SelectField(Board.B7);
            game.WhitePlayer.MoveSelectedFigureTo(Board.B8);

            game.BlackPlayer.SelectField(Board.D3);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H7);

            game.WhitePlayer.SelectField(Board.B8);
            game.WhitePlayer.MoveSelectedFigureTo(Board.C8);

            game.BlackPlayer.SelectField(Board.F7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.G6);

            game.WhitePlayer.SelectField(Board.C8);
            game.WhitePlayer.MoveSelectedFigureTo(Board.E6);

            // assert
            Assert.AreEqual(true, game.Rules.IsDraw(Color.BLACK));
        }

        [TestMethod]
        public void KingVsKingDraw()
        {
            // arrange
            Game game = new Game(20);
            Board board = game.Board;
            Figure[,] predefinedBoard = new Figure[8, 8] {
                {null,             null,             null,             null,             Figure.BK(board), null,             null,             null,           },
                {null,             null,             null,             null,             Figure.WP(board), null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             Figure.WK(board), null,             null,             null,           }
            };
            game.Board.parseBoard(predefinedBoard);

            // act
            game.Start();

            game.WhitePlayer.SelectField(Board.E1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.E2);

            game.BlackPlayer.SelectField(Board.E8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.E7);

            // assert
            Assert.AreEqual(true, game.Rules.IsDraw(Color.BLACK));
        }

        [TestMethod]
        public void KingVsKingKnightDraw()
        {
            // arrange
            Game game = new Game(20);
            Board board = game.Board;
            Figure[,] predefinedBoard = new Figure[8, 8] {
                {null,             null,             null,             null,             Figure.BK(board), null,             null,             null,           },
                {null,             null,             null,             null,             Figure.WP(board), null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             Figure.WN(board), null,             null,             null,             null,           },
                {null,             null,             null,             null,             Figure.WK(board), null,             null,             null,           }
            };
            game.Board.parseBoard(predefinedBoard);

            // act
            game.Start();

            game.WhitePlayer.SelectField(Board.E1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.E2);

            game.BlackPlayer.SelectField(Board.E8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.E7);

            // assert
            Assert.AreEqual(true, game.Rules.IsDraw(Color.BLACK));
        }

        [TestMethod]
        public void KingBishopVsKingBishopOnSameColorFieldDraw()
        {
            // arrange
            Game game = new Game(20);
            Board board = game.Board;
            Figure[,] predefinedBoard = new Figure[8, 8] {
                {null,             null,             Figure.BB(board), null,             Figure.BK(board), null,             null,             null,           },
                {null,             null,             null,             null,             Figure.WP(board), null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             null,             null,             null,             null,           },
                {null,             null,             null,             null,             Figure.WK(board), Figure.WB(board), null,             null,           }
            };
            game.Board.parseBoard(predefinedBoard);

            // act
            game.Start();

            game.WhitePlayer.SelectField(Board.E1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.E2);

            game.BlackPlayer.SelectField(Board.E8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.E7);

            // assert
            Assert.AreEqual(true, game.Rules.IsDraw(Color.BLACK));
        }

        [TestMethod]
        public void TestMoves()
        {
            // arrange
            Game game = new Game(20);
            game.Start();

            game.WhitePlayer.SelectField(Board.G2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.G4);
            game.BlackPlayer.SelectField(Board.E7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.E5);

            game.WhitePlayer.SelectField(Board.G1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.F3);
            game.BlackPlayer.SelectField(Board.F8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.C5);

            game.WhitePlayer.SelectField(Board.F3);
            game.WhitePlayer.MoveSelectedFigureTo(Board.E5);
            game.BlackPlayer.SelectField(Board.D8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.F6);

            game.WhitePlayer.SelectField(Board.E5);
            game.WhitePlayer.MoveSelectedFigureTo(Board.C4);
            game.BlackPlayer.SelectField(Board.B8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.C6);

            game.WhitePlayer.SelectField(Board.E2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.E4);
            game.BlackPlayer.SelectField(Board.D7);
            game.BlackPlayer.MoveSelectedFigureTo(Board.D6);

            game.WhitePlayer.SelectField(Board.A2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.A4);
            game.BlackPlayer.SelectField(Board.G8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.H6);

            game.WhitePlayer.SelectField(Board.F1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.D3);
            game.BlackPlayer.SelectField(Board.C8);
            game.BlackPlayer.MoveSelectedFigureTo(Board.G4);

            game.WhitePlayer.SelectField(Board.D1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.G4);
            game.BlackPlayer.SelectField(Board.H6);
            game.BlackPlayer.MoveSelectedFigureTo(Board.G4);

            game.WhitePlayer.SelectField(Board.H1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.G1);
            game.BlackPlayer.SelectField(Board.G4);
            game.BlackPlayer.MoveSelectedFigureTo(Board.F2);

            game.WhitePlayer.SelectField(Board.G1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.F1);
            game.BlackPlayer.SelectField(Board.F2);
            game.BlackPlayer.MoveSelectedFigureTo(Board.D3);

            game.WhitePlayer.SelectField(Board.C2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.D3);
            game.BlackPlayer.SelectField(Board.C6);
            game.BlackPlayer.MoveSelectedFigureTo(Board.D4);

            game.WhitePlayer.SelectField(Board.B2);
            game.WhitePlayer.MoveSelectedFigureTo(Board.B4);
            game.BlackPlayer.SelectField(Board.D4);
            game.BlackPlayer.MoveSelectedFigureTo(Board.C2);

            game.WhitePlayer.SelectField(Board.E1);
            game.WhitePlayer.MoveSelectedFigureTo(Board.E2);

            Assert.AreEqual(false, game.Rules.IsCastlingAvailable(Color.WHITE));
        }
    }
}
