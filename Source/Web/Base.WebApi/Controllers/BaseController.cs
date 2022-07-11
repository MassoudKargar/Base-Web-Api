

using Base.Application.Jwt;

namespace Base.WebApi.Controllers;

[ApiController]
[ApiResultFilter]
[ReadableBodyStream]
[Route("api/v{version:apiVersion}/[controller]/[action]")]// api/v1/[controller]
public class BaseController<T, I> : ControllerBase where T : ControllerBase where I : class
{
    public BaseController(IJwtInterface jwtInterface, IMapper mapper, ILogger<T> logger, I baseInterface)
    {
        BaseInterface = baseInterface;
        JwtSetting = jwtInterface;
        Mapper = mapper;
        Logger = logger;
    }
    public I BaseInterface { get; }
    public IJwtInterface JwtSetting { get; }
    public IMapper Mapper { get; }
    public ILogger<T> Logger { get; }
}
