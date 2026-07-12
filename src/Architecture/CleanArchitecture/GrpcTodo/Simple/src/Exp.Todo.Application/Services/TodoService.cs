namespace Exp.Todo.Application.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTodoDto> _createValidator;
    private readonly IValidator<UpdateTodoDto> _updateValidator;

    public TodoService(
        ITodoRepository repository,
        IMapper mapper,
        IValidator<CreateTodoDto> createValidator,
        IValidator<UpdateTodoDto> updateValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<List<TodoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var todos = await _repository.GetAllAsync(cancellationToken);
        return todos != null 
            ? _mapper.Map<List<TodoDto>>(todos) 
            : new List<TodoDto>();
    }

    public async Task<TodoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var todo = await _repository.GetByIdAsync(id, cancellationToken);
        return todo is null ? null : _mapper.Map<TodoDto>(todo);
    }

    public async Task<int> CreateAsync(CreateTodoDto dto, CancellationToken cancellationToken = default)
    {
        // Validate the DTO
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new AppValidationException(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        // Create domain entity
        var todo = TodoEntity.Create(dto.TodoName);
        var id = await _repository.AddAsync(todo, cancellationToken);
        return id;
    }

    public async Task<bool> UpdateAsync(UpdateTodoDto dto, CancellationToken cancellationToken = default)
    {
        // Validate the DTO
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new AppValidationException(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        // Get existing entity
        var existingTodo = await _repository.GetByIdAsync(dto.Id, cancellationToken);
        if (existingTodo == null)
        {
            return false;
        }

        // Update domain entity
        TodoEntity.Update(existingTodo, dto.TodoName);
        var result = await _repository.UpdateAsync(existingTodo, cancellationToken);
        return result;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var todo = await _repository.GetByIdAsync(id, cancellationToken);
        if (todo == null)
        {
            return false;
        }

        var result = await _repository.DeleteAsync(id, cancellationToken);
        return result;
    }
}

