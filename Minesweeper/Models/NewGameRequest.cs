namespace Minesweeper.Models
{
    /// <summary>
    /// Запрос новой игры.
    /// </summary>
    public class NewGameRequest
    {
        /// <summary>
        /// Ширина игрового поля.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота игрового поля.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Количество мин на поле.
        /// </summary>
        public int Mines_Count { get; set; }
    }
}
