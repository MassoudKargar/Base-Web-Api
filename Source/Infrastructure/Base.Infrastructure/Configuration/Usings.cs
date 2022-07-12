global using Autofac;

global using Base.Infrastructure.Configuration.Filters;
global using Base.Infrastructure.Exceptions;
global using Base.Infrastructure.UserSettings;
global using Base.Infrastructure.Utilities;
global using Base.Infrastructure.WebSetting;

global using Dapper;

global using DNTPersianUtils.Core;

global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Primitives;
global using Microsoft.IdentityModel.Tokens;

global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;

global using System.Collections;
global using System.ComponentModel.DataAnnotations;
global using System.Data;
global using System.Data.SqlClient;
global using System.Globalization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net;
global using System.Reflection;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Security.Principal;
global using System.Text;


#region Assembly
namespace Base.Infrastructure;
public record InfrastructureAssembly();
#endregion
