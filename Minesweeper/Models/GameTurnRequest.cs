namespace Minesweeper.Models
{
    /// <summary>
    /// Запрос игрового хода.
    /// </summary>
    public class GameTurnRequest
    {
        /// <summary>
        /// Идентификатор игры.
        /// </summary>
        public Guid Game_Id { get; set; }
        /// <summary>
        /// Колонка проверяемой ячейки (нумерация с нуля).
        /// </summary>
        public int Col { get; set; }

        /// <summary>
        /// Ряд проверяемой ячейки (нумерация с нуля).
        /// </summary>
        public int Row { get; set; }
    }
}
