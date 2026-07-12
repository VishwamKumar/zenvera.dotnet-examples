global using Moq;
global using FluentAssertions;
global using Microsoft.EntityFrameworkCore;
global using AutoMapper;
global using FluentValidation;
global using Microsoft.Extensions.DependencyInjection;

global using Exp.Todo.Application.Dtos;
global using Exp.Todo.Application.Interfaces.Persistence;
global using Exp.Todo.Application.Services;
global using Exp.Todo.Application.Common.Exceptions;
global using Exp.Todo.Infrastructure.Persistence;
global using Exp.Todo.Domain.Entities;
global using Exp.Todo.Domain.Common;
global using TodoEntity = Exp.Todo.Domain.Entities.Todo;
