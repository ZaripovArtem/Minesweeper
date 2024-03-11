namespace Minesweeper.Services
{
    /// <summary>
    /// Сервис валидации.
    /// </summary>
    public static class ValidationService
    {
        /// <summary>
        /// Максимальная ширина поля.
        /// </summary>
        private const int MaxHeight = 30;

        /// <summary>
        /// Минимальная ширина поля.
        /// </summary>
        private const int MinHeight = 2;

        /// <summary>
        /// Максимальная высота поля.
        /// </summary>
        private const int MaxWidth = 30;

        /// <summary>
        /// Минимальная высота поля.
        /// </summary>
        private const int MinWidth = 2;

        /// <summary>
        /// Минимальное число мин.
        /// </summary>
        private const int MinMinesCount = 1;

        /// <summary>
        /// Минимальный индекс колонки.
        /// </summary>
        private const int MinCol = 0;

        /// <summary>
        /// Минимальный ряд колонки.
        /// </summary>
        private const int MinRow = 0;

        /// <summary>
        /// Валидатор запуска новой игры.
        /// </summary>
        /// <param name="width">Ширина игрового поля.</param>
        /// <param name="height">Высота игрового поля.</param>
        /// <param name="minesCount">Число мин.</param>
        public static void NewGameRequestValidator(int width, int height, int minesCount)
        {
            if (height < MinHeight)
            {
                throw new ArgumentException("Ширина поля должна быть не менее 2.");
            }

            if (height > MaxHeight)
            {
                throw new ArgumentException("Ширина поля должна быть не более 30.");
            }

            if (width < MinWidth)
            {
                throw new ArgumentException("Высота поля должна быть не менее 2.");
            }

            if (width > MaxWidth)
            {
                throw new ArgumentException("Высота поля должна быть не более 30.");
            }

            if (minesCount < MinMinesCount)
            {
                throw new ArgumentException("Количество мин должно быть не менее 1.");
            }

            if (minesCount > (width * height) - 1)
            {
                throw new ArgumentException($"Количество мин должно быть не более {(width * height) - 1}.");
            }
        }

        /// <summary>
        /// Валидатор запроса игрового хода.
        /// </summary>
        /// <param name="gameId">Идентификатор игры.</param>
        /// <param name="col">Колонка проверяемой ячейки.</param>
        /// <param name="row">Ряд проверяемой ячейки.</param>
        public static void GameTurnRequestValidator(Guid gameId, int col, int row)
        {
            if (gameId == Guid.Empty)
            {
                throw new ArgumentException("Передан пустой идентификатор игры.");
            }

            if (col < MinCol)
            {
                throw new ArgumentException($"Минимальный индекс колонки ячейки: {MinCol}");
            }

            if (row < MinRow)
            {
                throw new ArgumentException($"Минимальный индекс колонки ячейки: {MinRow}");
            }
        }
    }
}
