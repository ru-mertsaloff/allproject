using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Commands
{
    [ApiController]
    [Route("[controller]")]
    public class LoggingController : ControllerBase
    {
        private readonly IObservable _observable;
        public LoggingController(IObservable observable)
        {
            _observable = observable;
        }
        /// <summary>
        /// Проверка работы
        /// </summary>
        /// <returns>200</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        /// <summary>
        /// Обработка сообщения обработчиком по названию сервиса
        /// </summary>
        /// <param name="service">Название сервиса</param>
        /// <param name="message">Сообщение лога</param>
        /// <returns>200</returns>
        [HttpPost]
        [Route("Send/{service}")]
        public async Task<IActionResult> Send([FromRoute] string service, [FromBody] Message message)
        {
            _observable.HandlerLog(message);
            return Ok();
        }
    }
}
