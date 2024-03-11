using Minesweeper.Models;
using Minesweeper.Services.Interfaces;

namespace Minesweeper.Services
{
    /// <summary>
    /// Сервис игры.
    /// </summary>
    public class GameService : IGameService
    {
        /// <summary>
        /// Идентификатор игры.
        /// </summary>
        public static Guid GameId;

        /// <summary>
        /// Ширина поля.
        /// </summary>
        public static int Width;

        /// <summary>
        /// Высота поля.
        /// </summary>
        public static int Height;

        /// <summary>
        /// Количество мин.
        /// </summary>
        public static int MinesCount;

        /// <summary>
        /// Завершена ли игра.
        /// </summary>
        public static bool IsGameCompleted;

        /// <summary>
        /// Игровое поле, представляемое игроку.
        /// </summary>
        public static string[][] GameField;

        /// <summary>
        /// Сгенерированное игровое поле.
        /// </summary>
        public static string[][] GeneratedGameField;

        /// <summary>
        /// Генерация поля.
        /// </summary>
        /// <param name="newGameRequest">Запрос новой игры.</param>
        public void GenerateField(NewGameRequest newGameRequest) 
        {
            ValidationService.NewGameRequestValidator(
                newGameRequest.Width,
                newGameRequest.Height,
                newGameRequest.Mines_Count);

            GameId = Guid.NewGuid();

            GameField = new string[newGameRequest.Height][];

            GeneratedGameField = new string[newGameRequest.Height][];

            for (var i = 0; i < newGameRequest.Height; i++)
            {
                GameField[i] = new string[newGameRequest.Width];

                GeneratedGameField[i] = new string[newGameRequest.Width];

                for (int j = 0; j < newGameRequest.Width; j++)
                {
                    GameField[i][j] = " ";
                }
            }

            GenerateMines(newGameRequest);

            FillOpenCells();

            IsGameCompleted = false;

            MinesCount = newGameRequest.Mines_Count;
        }

        /// <summary>
        /// Генерация мин.
        /// </summary>
        /// <param name="newGameRequest">Запрос новой игры.</param>
        public void GenerateMines(NewGameRequest newGameRequest)
        {
            ValidationService.NewGameRequestValidator(
                newGameRequest.Width,
                newGameRequest.Height,
                newGameRequest.Mines_Count);

            var random = new Random();
            var minesCount = newGameRequest.Mines_Count;
            var height = newGameRequest.Height;
            var width = newGameRequest.Width;

            var allCoordinates = new List<(int, int)>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    allCoordinates.Add((i, j));
                }
            }
            
            allCoordinates = allCoordinates.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < minesCount; i++)
            {
                int row = allCoordinates[i].Item1;
                int col = allCoordinates[i].Item2;
                GeneratedGameField[row][col] = "X";
            }
        }
      
        /// <summary>
        /// Заполнение ячеек без мин.
        /// </summary>
        public void FillOpenCells()
        {
            if (GeneratedGameField == null)
            {
                throw new ArgumentNullException("Передана ссылка на пустое поле.");
            }

            int height = GeneratedGameField.Length;
            int width = GeneratedGameField[0].Length;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (GeneratedGameField[i][j] != "X")
                    {
                        int minesCount = 0;

                        for (int di = -1; di <= 1; di++)
                        {
                            for (int dj = -1; dj <= 1; dj++)
                            {
                                int ni = i + di;
                                int nj = j + dj;

                                if (ni >= 0 && ni < height && nj >= 0 && nj < width && GeneratedGameField[ni][nj] == "X")
                                {
                                    minesCount++;
                                }
                            }
                        }
                        GeneratedGameField[i][j] = minesCount.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Сделать ход в игре.
        /// </summary>
        /// <param name="gameTurnRequest">Запрос игрового хода.</param>
        public void MakeMove(GameTurnRequest gameTurnRequest)
        {
            ValidationService.GameTurnRequestValidator(
                gameTurnRequest.Game_Id,
                gameTurnRequest.Col,
                gameTurnRequest.Row);

            int width = gameTurnRequest.Col;
            int height = gameTurnRequest.Row;

            if (GameField[height][width] != " ")
            {
                throw new ArgumentException("Поле уже открыто.");
            }

            if (GeneratedGameField[height][width] == "X" )
            {
                GameField = GeneratedGameField;
                IsGameCompleted = true;
            }
            else
            {
                if(GeneratedGameField[height][width] != "0")
                {
                    GameField[height][width] = GeneratedGameField[height][width];

                    var result = IsGameWin();

                    if (result)
                    {
                        IsGameCompleted = IsGameWin();
                        WinGameField();
                    }
                }
                else
                {
                    OpenZeroMinesField(height, width, GameField.Length - 1, GameField[0].Length - 1);

                    var result = IsGameWin();

                    if (result)
                    {
                        IsGameCompleted = IsGameWin();
                        WinGameField();
                    }

                }
            }
        }

        /// <summary>
        /// Открытие ячеек, рядом с которыми нет ни одной мины. 
        /// </summary>
        /// <param name="row">Ряд.</param>
        /// <param name="col">Колонка.</param>
        /// <param name="maxRow">Максимальный индекс ряда.</param>
        /// <param name="maxCol">Максимальный индекс колонки.</param>
        private void OpenZeroMinesField(int row, int col, int maxRow, int maxCol)
        {
            for (var i = row - 1; i < row + 2; i++)
            {
                if (i < 0 || i > maxRow)
                {
                    continue;
                }

                for (var j = col - 1; j < col + 2; j++)
                {
                    if (j < 0 || j > maxCol)
                    {
                        continue;
                    }

                    if (GameField[i][j] != " ")
                    {
                        continue;
                    }

                    GameField[i][j] = GeneratedGameField[i][j];

                    if (GameField[i][j] == "0")
                    {
                        OpenZeroMinesField(i, j, maxRow, maxCol);
                    }
                }   
            }
        }

        /// <summary>
        /// Проверка на то закончилась ли игра победой.
        /// </summary>
        /// <returns>True - да, false - нет.</returns>
        private bool IsGameWin()
        {
            int height = GameField.Length;
            int width = GameField[0].Length;

            var emptyFieldCount = 0;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (GameField[i][j] == " ")
                    {
                        emptyFieldCount++;
                    }
                }
            }

            if(emptyFieldCount == MinesCount)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Заполнение игрового поля в случае победы.
        /// </summary>
        private void WinGameField()
        {
            int height = GameField.Length;
            int width = GameField[0].Length;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (GameField[i][j] == " ")
                    {
                        GameField[i][j] = "M";
                    }
                }
            }
        }
    }
}
