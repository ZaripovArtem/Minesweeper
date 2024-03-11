using Minesweeper.Models;
using Minesweeper.Services;

namespace UnitTests
{
    /// <summary>
    /// Тестирование сервиса игры.
    /// </summary>
    [TestClass]
    public class GameServiceTests
    {
        /// <summary>
        /// Тестирование генерации поля и мин.
        /// </summary>
        /// <param name="mines">Количество мин на игровом поле.</param>
        [TestMethod]
        [DataRow(10)]
        [DataRow(4)]
        [DataRow(7)]
        [DataRow(5)]
        [DataRow(12)]
        [DataRow(99)]
        public void GenerateField_MinesCount_ReturnCurrentCount(int mines)
        {
            var gameService = new GameService();

            gameService.GenerateField(new NewGameRequest
            {
                Width = 10,
                Height = 10,
                Mines_Count = mines
            });

            var minesCount = 0;

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (GameService.GeneratedGameField[i][j] == "X")
                    {
                        minesCount++;
                    }
                }
            }

            Assert.AreEqual(mines, minesCount);
        }

        /// <summary>
        /// Тестирование метода хода в игре, при выйгрыше.
        /// </summary>
        [TestMethod]
        public void MakeMove_Coordinates_WinGame()
        {
            var gameService = new GameService();

            gameService.GenerateField(new NewGameRequest
            {
                Height = 3,
                Width = 3,
                Mines_Count = 1
            });

            GameService.GeneratedGameField = new string[3][]
            {
                new string[] {"1", "X", "1"},
                new string[] {"1", "1", "1"},
                new string[] {"0", "0", "0"}
            };

            gameService.MakeMove(new GameTurnRequest
            {
                Game_Id = GameService.GameId,
                Col = 1,
                Row = 2
            });

            var expectedResult = new string[3][]
            {
                new string[] {" ", " ", " "},
                new string[] {"1", "1", "1"},
                new string[] {"0", "0", "0"}
            };

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    Assert.AreEqual(expectedResult[i][j], GameService.GameField[i][j]);
                }
            }

            Assert.IsFalse(GameService.IsGameCompleted);

            gameService.MakeMove(new GameTurnRequest
            {
                Game_Id = GameService.GameId,
                Col = 0,
                Row = 0
            });

            expectedResult = new string[3][]
            {
                new string[] {"1", " ", " "},
                new string[] {"1", "1", "1"},
                new string[] {"0", "0", "0"}
            };

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    Assert.AreEqual(expectedResult[i][j], GameService.GameField[i][j]);
                }
            }

            Assert.IsFalse(GameService.IsGameCompleted);

            gameService.MakeMove(new GameTurnRequest
            {
                Game_Id = GameService.GameId,
                Col = 2,
                Row = 0
            });

            expectedResult = new string[3][]
            {
                new string[] {"1", "M", "1"},
                new string[] {"1", "1", "1"},
                new string[] {"0", "0", "0"}
            };

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    Assert.AreEqual(expectedResult[i][j], GameService.GameField[i][j]);
                }
            }

            Assert.IsTrue(GameService.IsGameCompleted);
        }

        /// <summary>
        /// Тестирование метода хода в игре, при проигрыше.
        /// </summary>
        [TestMethod]
        public void MakeMove_Coordinates_LoseGame()
        {
            var gameService = new GameService();

            gameService.GenerateField(new NewGameRequest
            {
                Height = 3,
                Width = 3,
                Mines_Count = 1
            });

            GameService.GeneratedGameField = new string[3][]
            {
                new string[] {"1", "X", "1"},
                new string[] {"1", "1", "1"},
                new string[] {"0", "0", "0"}
            };

            gameService.MakeMove(new GameTurnRequest
            {
                Game_Id = GameService.GameId,
                Col = 1,
                Row = 0
            });

            var expectedResult = new string[3][]
            {
                new string[] {"1", "X", "1"},
                new string[] {"1", "1", "1"},
                new string[] {"0", "0", "0"}
            };

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    Assert.AreEqual(expectedResult[i][j], GameService.GameField[i][j]);
                }
            }

            Assert.IsTrue(GameService.IsGameCompleted);
        }
    }
}