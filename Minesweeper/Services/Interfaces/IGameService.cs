using Minesweeper.Models;

namespace Minesweeper.Services.Interfaces
{
    /// <summary>
    /// Сервис игры.
    /// </summary>
    public interface IGameService
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
        /// Игровое поле.
        /// </summary>
        public static string[][] GameField;

        /// <summary>
        /// Сгенерировать поле.
        /// </summary>
        /// <param name="newGameRequest">Запрос новой игры.</param>
        public void GenerateField(NewGameRequest newGameRequest);

        /// <summary>
        /// Сделать ход.
        /// </summary>
        /// <param name="gameTurnRequest">Запрос игрового хода.</param>
        public void MakeMove(GameTurnRequest gameTurnRequest);
    }
}
