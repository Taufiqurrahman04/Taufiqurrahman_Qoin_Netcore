using RabbitMQ.Client;
using Taufiqurrahman_Test_Qoin.EntityFramework.Models;

namespace Taufiqurrahman_Test_Qoin.EntityFramework.Repositories
{
    public interface IRabbitMqs
    {
        bool Connection();
        void Publish<T>(T message, string exchangeName,string exchangeType, string routeKey) where T : class;
        void CreateChanel();
        void CloseChannel();
    }
}
