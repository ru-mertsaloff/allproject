using Api.Models;

namespace Api.Interfaces
{
    public interface IObservable
    {
        /// <summary>
        /// Обработка сообщения
        /// </summary>
        /// <returns></returns>
        public void HandlerLog(Message message);

    }
}