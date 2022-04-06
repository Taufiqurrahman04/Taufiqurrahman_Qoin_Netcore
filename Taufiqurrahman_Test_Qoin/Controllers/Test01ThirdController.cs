using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taufiqurrahman_Test_Qoin.EntityFramework.Models;
using Taufiqurrahman_Test_Qoin.EntityFramework.Repositories;
using Taufiqurrahman_Test_Qoin.Services;

namespace Taufiqurrahman_Test_Qoin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test01ThirdController : ControllerBase
    {
        private IRabbitMqs _manager;
        private const string _exchangeName = "etest1";
        private const string _exchangeType = "direct";
        private const string _routeKey = "test1";
        private const string _queueName = "qtest1";

        public Test01ThirdController(IRabbitMqs manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Test01 data)
        {
            try
            {
                if (data != null)
                {
                  if(_manager.Connection())
                    {
                        _manager.CreateChanel();
                        _manager.Publish(new
                        {
                            command = "create",
                            data = new
                            {
                                Nama = data.Nama,
                                Status = data.Status
                            }
                        }, _exchangeName,_exchangeType,_routeKey);

                        return await Task.Run(() => StatusCode(StatusCodes.Status200OK, "Success Publish Data To Rabbit Mq."));
                    }
                    else return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "No Connection To Rabbit Mq."));

                }
                else return await Task.Run(() => StatusCode(StatusCodes.Status204NoContent, "Data is empty."));
            }
            catch (Exception ex)
            {

                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, ex.GetMessageException().Message));
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put( [FromBody] Test01 data)
        {
            try
            {
                if (data != null)
                {
                    if (_manager.Connection())
                    {
                        _manager.CreateChanel();
                        _manager.Publish(new
                    {
                        command = "update",
                        data = new
                        {
                            Nama = data.Nama,
                            Status = data.Status
                        }
                    }, _exchangeName, _exchangeType, _routeKey);

                    return await Task.Run(() => StatusCode(StatusCodes.Status200OK, "Success Publish Data To Rabbit Mq."));
                    }
                    else return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "No Connection To Rabbit Mq."));
                }
                else return await Task.Run(() => StatusCode(StatusCodes.Status204NoContent, "Data is empty."));


            }
            catch (Exception ex)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, ex.GetMessageException().Message));
            }
        }
        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] Test01 data)
        {
            try
            {
                if (data != null)
                {
                    if (_manager.Connection())
                    {
                        _manager.CreateChanel();
                        _manager.Publish(new
                    {
                        command = "delete",
                        data = new
                        {
                            Nama = data.Nama,
                            Status = data.Status
                        }
                    }, _exchangeName, _exchangeType, _routeKey);

                    return await Task.Run(() => StatusCode(StatusCodes.Status200OK, "Success Publish Data To Rabbit Mq."));
                    }
                    else return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "No Connection To Rabbit Mq."));
                }
                else return await Task.Run(() => StatusCode(StatusCodes.Status204NoContent, "Data is empty."));
            }
            catch (Exception ex)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, ex.GetMessageException().Message));
            }
        }
    }
}
