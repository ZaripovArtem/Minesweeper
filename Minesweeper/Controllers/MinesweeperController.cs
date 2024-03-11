using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Minesweeper.Models;
using Minesweeper.Services;
using Minesweeper.Services.Interfaces;

namespace Minesweeper.Controllers
{
    /// <summary>
    /// Контроллер игры "Сапер".
    /// </summary>
    [EnableCors]
    [Route("api")]
    [ApiController]
    public class MinesweeperController : ControllerBase
    {
        /// <summary>
        /// Сервис игры.
        /// </summary>
        private readonly IGameService _gameService;

        /// <summary>
        /// Инициализация контроллера.
        /// </summary>
        /// <param name="gameService">Сервис игры.</param>
        public MinesweeperController(IGameService gameService)
        {
            if (gameService == null) throw new ArgumentNullException(nameof(gameService));

            _gameService = gameService;
        }

        /// <summary>
        /// Новая игра.
        /// </summary>
        /// <param name="newGameRequest">Запрос новой игры.</param>
        /// <returns>Статус код запроса.</returns>
        [HttpPost("new")]
        public async Task<IActionResult> NewGame(NewGameRequest newGameRequest)
        {
            try
            {
                _gameService.GenerateField(newGameRequest);

                GameService.Width = newGameRequest.Width;
                GameService.Height = newGameRequest.Height;
                GameService.MinesCount = newGameRequest.Mines_Count;

                var result = new GameInfoResponse()
                {
                    Game_Id = GameService.GameId,
                    Width = GameService.Width,
                    Height = GameService.Height,
                    Mines_Count = GameService.MinesCount,
                    Completed = GameService.IsGameCompleted,
                    Field = GameService.GameField
                };
                
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse
                {
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// Сделать ход.
        /// </summary>
        /// <param name="gameTurnRequest">Запрос игрового хода.</param>
        /// <returns>Статус код запроса.</returns>
        [HttpPost("turn")]
        public async Task<IActionResult> MakeNextMove(GameTurnRequest gameTurnRequest)
        {
            if (GameService.IsGameCompleted)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse
                {
                    Error = "Игра уже завершена."
                });
            }

            try
            {
                gameTurnRequest.Game_Id = GameService.GameId;

                _gameService.MakeMove(gameTurnRequest);

                var result = new GameInfoResponse()
                {
                    Game_Id = GameService.GameId,
                    Width = GameService.Width,
                    Height = GameService.Height,
                    Mines_Count = GameService.MinesCount,
                    Completed = GameService.IsGameCompleted,
                    Field = GameService.GameField
                };

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse
                {
                    Error = ex.Message
                });
            }
        }
    }
}
