using Exp.Todo.Application.Features.TodoManager.Command.CreateTodo;
using Exp.Todo.Application.Interfaces.CQRS;
using Exp.Todo.Application.Interfaces.Persistence;
using Exp.Todo.Application.Common;

namespace Exp.Todo.Tests.UnitTests.Application.Commands;

public class CreateTodoCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsTodoId()
    {
        // Arrange
        var mockWriteRepo = new Mock<ITodoWriteRepository>();
        mockWriteRepo.Setup(r => r.AddAsync(It.IsAny<TodoEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TodoEntity todo, CancellationToken ct) =>
                {
                    // Simulate DB-generated ID = 1
                    return 1;
                });

        var handler = new CreateTodoCommandHandler(mockWriteRepo.Object);
        var command = new CreateTodoCommand(new CreateTodoDto { TodoName = "Test Task" });

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        mockWriteRepo.Verify(r => r.AddAsync(It.Is<TodoEntity>(t => t.TodoName == "Test Task"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_InvalidTodoName_ThrowsValidationException(string? todoName)
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddValidatorsFromAssembly(typeof(CreateTodoCommand).Assembly);
        services.AddScoped<IDispatcher, Dispatcher>();
        services.AddScoped<ICommandHandler<CreateTodoCommand, int>, CreateTodoCommandHandler>();

        var mockWriteRepo = new Mock<ITodoWriteRepository>();
        services.AddSingleton(mockWriteRepo.Object);

        var serviceProvider = services.BuildServiceProvider();
        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        var command = new CreateTodoCommand(new CreateTodoDto { TodoName = todoName! });

        // Act & Assert - Validation should throw AppValidationException before handler is called
        // If validators aren't found, domain validation will throw DomainException
        var exception = await Assert.ThrowsAnyAsync<Exception>(() => dispatcher.Send(command, CancellationToken.None));
        Assert.True(exception is AppValidationException || exception is DomainException,
            $"Expected AppValidationException or DomainException, but got {exception.GetType().Name}");
        mockWriteRepo.Verify(r => r.AddAsync(It.IsAny<TodoEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

