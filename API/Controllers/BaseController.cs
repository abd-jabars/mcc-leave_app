using API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public ActionResult<Entity> Insert(Entity entity)
        {
            try
            {
                var result = repository.Insert(entity);
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "Input success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.ToString() });
            }
        }

        [HttpGet]
        public ActionResult<Entity> Get()
        {
            try
            {
                var result = repository.Get();
                if (result.Count() > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok(new { status = HttpStatusCode.NoContent, result = result, message = "There is no data" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.ToString() });
            }
        }

        [HttpGet("{key}")]
        public ActionResult<Entity> Get(Key key)
        {
            try
            {
                var result = repository.Get(key);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok(new { status = HttpStatusCode.NoContent, result = result, message = $"Data with id {key} not found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.ToString() });
            }
        }

        [HttpPut]
        public ActionResult<Entity> Update(Entity entity)
        {
            try
            {
                var result = repository.Update(entity);
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "Data updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.ToString() });
            }
        }

        [HttpDelete]
        public ActionResult<Entity> Delete(Entity entity)
        {
            try
            {
                var result = repository.Delete(entity);
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "Data deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.ToString() });
            }
        }
    }
}
