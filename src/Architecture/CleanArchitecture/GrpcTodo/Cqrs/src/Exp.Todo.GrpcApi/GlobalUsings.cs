global using System.Data;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.OpenApi.Models;
global using Google.Protobuf.WellKnownTypes;
global using AutoMapper;
global using Grpc.Core;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;

global using Exp.Todo.GrpcApi.Helpers;
global using Exp.Todo.GrpcApi.Services;
global using Exp.Todo.GrpcApi.Extensions;
global using Exp.Todo.GrpcApi.Middleware;
global using Exp.Todo.GrpcApi.Configuration;

global using Exp.Todo.Application.Extensions;
global using Exp.Todo.Application.Interfaces.CQRS;
global using Exp.Todo.Application.Common.Exceptions;
global using Exp.Todo.Application.Features.TodoManager.Dtos;
global using Exp.Todo.Application.Features.TodoManager.Queries.GetAll;
global using Exp.Todo.Application.Features.TodoManager.Queries.GetById;
global using Exp.Todo.Application.Features.TodoManager.Command.CreateTodo;
global using Exp.Todo.Application.Features.TodoManager.Command.UpdateTodo;
global using Exp.Todo.Application.Features.TodoManager.Command.DeleteTodo;

global using Exp.Todo.Domain.Common;
global using Exp.Todo.Infrastructure.Extensions;
global using Exp.Todo.Infrastructure.Persistence;
global using TodoEntity = Exp.Todo.Domain.Entities.Todo;
