namespace Base.WebApi.Controllers;

[ApiController]
[ApiResultFilter]
[Route("api/v{version:apiVersion}/[controller]/[action]")]// api/v1/[controller]
public class BaseController<T, I> : ControllerBase where T : ControllerBase where I : class
{
    public BaseController(ILogger<T> logger, I baseInterface)
    {
        BaseInterface = baseInterface;
        Logger = logger;
    }
    public I BaseInterface { get; }
    public ILogger<T> Logger { get; }
}
