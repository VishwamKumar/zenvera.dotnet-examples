global using AutoMapper;
global using FluentValidation;
global using FluentValidation.Results;
global using System.Reflection;
global using Microsoft.Extensions.DependencyInjection;

global using Exp.Todo.Domain.Entities;
global using Exp.Todo.Domain.Common;
global using Exp.Todo.Application.Features.TodoManager.Dtos;
global using Exp.Todo.Application.Features.TodoManager.Command.CreateTodo;
global using Exp.Todo.Application.Features.TodoManager.Command.UpdateTodo;
global using Exp.Todo.Application.Features.TodoManager.Command.DeleteTodo;
global using Exp.Todo.Application.Interfaces.Persistence;
global using Exp.Todo.Application.Interfaces.CQRS;
global using Exp.Todo.Application.Common;
global using Exp.Todo.Application.Common.Exceptions;
global using TodoEntity = Exp.Todo.Domain.Entities.Todo;
