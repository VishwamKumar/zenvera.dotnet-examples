
namespace Exp.Todo.RestApi.MvcControllers;

[Route("api/[controller]")]
[ApiController]
public class ToDosController(ILogger<ToDosController> logger, IMapper mapper, ITodoService todoService) : Controller
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoResponse>>> GetToDos()
    {
        var recs = await todoService.GetToDosAsync();
        var items = mapper.Map<List<ToDoResponse>>(recs);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoResponse>> GetToDo([FromRoute] int id)
    {
        var toDo = await todoService.GetToDoByIdAsync(id);

        if (toDo == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<ToDoResponse>(toDo));
    }


    [HttpPost]
    public async Task<ActionResult<ToDoResponse>> PostToDo([FromBody] ToDoRequest req)
    {
        try
        {
            ToDo toDo = new() { ToDoName = req.ToDoName };
            var toDoRec = await todoService.AddToDoAsync(toDo);
            if (toDoRec != null)
            {
                return CreatedAtAction(nameof(GetToDo), new { id = toDoRec.Id }, toDoRec);
            }
            else
            {
                return BadRequest(new { message = "The ToDo item could not be created. Please check the input and try again." });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex?.Message);
            throw;
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutToDo([FromRoute] int id, [FromBody] ToDoRequest req)
    {
        try
        {
            ToDo? toDo = await todoService.GetToDoByIdAsync(id);

            if (toDo == null)
            {
                return NotFound();
            }
            toDo.ToDoName = req.ToDoName;
            var rec = await todoService.UpdateToDoAsync(toDo);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDo([FromRoute] int id)
    {
        try
        {
            ToDo? toDo = await todoService.GetToDoByIdAsync(id);

            if (toDo == null)
            {
                return NotFound();
            }
            _ = await todoService.DeleteToDoByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
        return NoContent();
    }
}

