using System;
using System.Threading.Tasks;
using AspNetCore.UnitOfWork.Example.Data;
using AspNetCore.UnitOfWork.Example.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.UnitOfWork.Example.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodosController : ControllerBase
    {

        private readonly IUnitOfWork<TodoDbContext> _unitOfWork;
        private readonly IRepository<Todo> _todoRepository;

        public TodosController(IUnitOfWork<TodoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _todoRepository = _unitOfWork.GetRepository<Todo>();
        }


        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _todoRepository.AsQueryable().ToListAsync();
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var todo = await _todoRepository.FirstOrDefaultAsync(x => x.Id == id);
            if(todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Todo todo)
        {

            var newTodo = new Todo
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Description = todo.Description,
                Done = false,
                Title = todo.Title
            };

            await _todoRepository.AddAsync(newTodo);
            var result = await _unitOfWork.SaveChangesAsync();
            if(result > 0)
            {
                return Created("ddd", newTodo);
            }
            return BadRequest("can not create todo");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Todo todo)
        {
            var oldTodo = await _todoRepository.FirstOrDefaultAsync(x => x.Id == id);
            if(oldTodo == null)
            {
                return NotFound("todo not found");
            }

            oldTodo.Title = todo.Title;
            oldTodo.Description = todo.Description;
            oldTodo.Done = todo.Done;
            oldTodo.UpdatedAt = DateTime.UtcNow;

            var result = await _unitOfWork.SaveChangesAsync();

            if(result <= 0)
            {
                return BadRequest("can not update");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {

            var todo = await _todoRepository.FirstOrDefaultAsync(x => x.Id == id);
            if(todo == null)
            {
                return NotFound("item not found");
            }

            _todoRepository.Remove(todo);
            var result = await _unitOfWork.SaveChangesAsync();

            if(result <= 0)
            {
                return BadRequest("can not delete");
            }
            return NoContent();
        }
    }
}
