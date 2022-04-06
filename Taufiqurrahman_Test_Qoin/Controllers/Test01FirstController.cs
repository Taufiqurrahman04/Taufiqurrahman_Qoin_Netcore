using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Taufiqurrahman_Test_Qoin.EntityFramework.Models;
using Taufiqurrahman_Test_Qoin.EntityFramework.Repositories;
using Taufiqurrahman_Test_Qoin.Services;

namespace Taufiqurrahman_Test_Qoin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test01FirstController : ControllerBase
    {
        private readonly ITest01s _test01s;
        public Test01FirstController(ITest01s test01s)
        {
            _test01s = test01s;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test01>>> Get([FromQuery]int pageNumber)
        {
            try
            {
                
                return await Task.Run(()=>
                _test01s.CheckDbConnection()?
                Ok(_test01s.GetTest01s(new Test01Parameters
                {
                    pageNumber = pageNumber,
                    pageSize = 20
                })): StatusCode(StatusCodes.Status500InternalServerError,"No connection to database."));
            }
            catch(Exception ex)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, ex.GetMessageException().Message));
            }
           
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Test01>> GetDetail(int id)
        {
            try
            {
                if (_test01s.CheckDbConnection())
                {
                    if (_test01s.CheckDataExist(id))
                    {
                        return await Task.Run(() => Ok(_test01s.GetDetailsTest01(id)));
                    }
                    else return await Task.Run(() => StatusCode(StatusCodes.Status204NoContent, null));
                }
                else return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "No connection to database."));


            }
            catch (Exception ex)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, ex.GetMessageException().Message));
            }
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Test01 data)
        {
            try
            {
                if (data != null)
                {
                    if (_test01s.CheckDbConnection())
                    {


                        if (_test01s.CheckDataExist(data.Nama))
                        {
                            return await Task.Run(() => StatusCode(StatusCodes.Status409Conflict, "Data with same name already exist."));
                        }
                        else
                            return await Task.Run(() => _test01s.InsertTest01(data) ? Ok("Data inserted.") : StatusCode(StatusCodes.Status304NotModified, "No data inserted."));

                    }
                    else return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "No connection to database."));
                }
                else return await Task.Run(() => StatusCode(StatusCodes.Status204NoContent, "Data is empty."));
            }
            catch (Exception ex)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, ex.GetMessageException().Message));
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,[FromBody]Test01 data)
        {
            try
            {
                if (data != null)
                {
                    data.Id = id;
                    if (_test01s.CheckDbConnection())
                    {


                        if (_test01s.CheckDataExist(data.Id))
                            return await Task.Run(() => _test01s.UpdateTest01(data) ? Ok("Data updated.") : StatusCode(StatusCodes.Status304NotModified, "No data updated."));
                        else return await Task.Run(() => StatusCode(StatusCodes.Status404NotFound, "Data not found on database."));
                    }
                    else return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "No connection to database."));
                }
                else return await Task.Run(() => StatusCode(StatusCodes.Status204NoContent, "Data is empty."));


            }
            catch (Exception ex)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, ex.GetMessageException().Message));
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (_test01s.CheckDbConnection())
                {


                    if (_test01s.CheckDataExist(id))
                        return await Task.Run(() => _test01s.DeleteTest01(id) ? Ok("Data deleted.") : StatusCode(StatusCodes.Status304NotModified, "No data deleted."));
                    else return await Task.Run(() => StatusCode(StatusCodes.Status404NotFound, "Data not found on database."));
                }
                else
                    return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, "No connection to database."));
            }
            catch (Exception ex)
            {
                return await Task.Run(() => StatusCode(StatusCodes.Status500InternalServerError, ex.GetMessageException().Message));
            }
        }

    }
}
