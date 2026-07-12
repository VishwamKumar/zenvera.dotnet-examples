global using HotChocolate;
global using HotChocolate.Resolvers;
global using HotChocolate.Authorization;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
//global using Microsoft.AspNetCore.Authorization;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Authentication.JwtBearer;


global using System.Text;
global using System.Security.Claims;
global using System.IdentityModel.Tokens.Jwt;
global using System.ComponentModel.DataAnnotations;

global using Exp.Auth.GraphQLApi.JwtBearer.Configs;
global using Exp.Auth.GraphQLApi.JwtBearer.Dtos;
global using Exp.Auth.GraphQLApi.JwtBearer.Schemas.Queries;

global using Exp.Auth.GraphQLApi.JwtBearer.Controllers;
global using Exp.Auth.GraphQLApi.JwtBearer.Middlewares;
