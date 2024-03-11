namespace Minesweeper.Models
{
    /// <summary>
    /// Отклик об информации по игре.
    /// </summary>
    public class GameInfoResponse
    {
        /// <summary>
        /// Идентификатор игры.
        /// </summary>
        public Guid Game_Id { get; set; }
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

        /// <summary>
        /// Завершена ли игра.
        /// </summary>
        public Boolean Completed { get; set; }

        /// <summary>
        /// Поля.
        /// </summary>
        public string[][] Field { get; set; }
    }
}
