global using AutoMapper;

global using Base.Application.Configuration;
global using Base.Application.Jwt;
global using Base.Domain.Configuration;
global using Base.Domain.Generics;
global using Base.Domain.Roles;
global using Base.Domain.Users;
global using Base.Infrastructure;
global using Base.Infrastructure.Databases.Connections;
global using Base.Infrastructure.Databases.StoredProcedures;
global using Base.Infrastructure.Exceptions;
global using Base.Infrastructure.UserSettings;
global using Base.Infrastructure.WebSetting;

global using Dapper;

global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;

global using System.Data;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net;
global using System.Security.Claims;
global using Base.Infrastructure.Utilities;
global using System.Text;

#region Assembly
namespace Base.Application;
/// <summary>
/// برای دسترسی راحت به به آدرس این لایه از این کلاس استفاده میکنیم
/// </summary>
public record ApplicationAssembly;
#endregion