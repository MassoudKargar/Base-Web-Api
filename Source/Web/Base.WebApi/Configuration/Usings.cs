global using Autofac;
global using Autofac.Extensions.DependencyInjection;

global using AutoMapper;

global using Base.Application;
global using Base.Application.Users;
global using Base.Domain;
global using Base.Domain.Configuration;
global using Base.Domain.Users;
global using Base.Infrastructure;
global using Base.Infrastructure.Configuration;
global using Base.Infrastructure.Exceptions;
global using Base.Infrastructure.Utilities;
global using Base.Infrastructure.WebSetting;
global using Base.WebApi.Configuration;
global using Base.WebApi.Configuration.Middleware;
global using Base.WebApi.Configuration.Swagger;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Authorization;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using Pluralize.NET;

global using Serilog;

global using Swashbuckle.AspNetCore.Filters;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using Swashbuckle.AspNetCore.SwaggerUI;

global using System.Data.SqlClient;
global using System.Net;
global using System.Reflection;
global using System.Text;
