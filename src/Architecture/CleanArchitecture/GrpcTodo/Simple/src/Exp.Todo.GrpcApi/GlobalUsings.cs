global using System.Data;
global using Microsoft.Extensions.Logging;
global using Microsoft.OpenApi.Models;
global using Serilog;
global using Grpc.Core.Interceptors;

global using Exp.Todo.GrpcApi.Helpers;
global using Exp.Todo.GrpcApi.Services;
global using Exp.Todo.GrpcApi.Extensions;
global using Exp.Todo.GrpcApi.Middleware;
global using Exp.Todo.GrpcApi.Interceptors;

global using Exp.Todo.Application.Extensions;
global using Exp.Todo.Application.Common.Exceptions;
global using Exp.Todo.Application.Dtos;
global using Exp.Todo.Application.Services;

global using Exp.Todo.Infrastructure.Extensions;
global using Exp.Todo.Domain.Common;

global using Google.Protobuf.WellKnownTypes;
global using AutoMapper;
global using Grpc.Core;
global using TodoEntity = Exp.Todo.Domain.Entities.Todo;
