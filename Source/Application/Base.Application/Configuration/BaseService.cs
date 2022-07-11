namespace Base.Application.Configuration;
public class BaseService<T> : IScopedDependency
{
    #region Constructor
    public BaseService(
        IMapper mapper,
        ISqlConnection sqlConnection,
        ILogger<T> logger)
    {
        Mapper = mapper;
        SqlConnection = sqlConnection;
        Logger = logger;
    }
    public readonly IMapper Mapper;
    public ISqlConnection SqlConnection { get; }
    public ILogger<T> Logger { get; }
    #endregion
}

