namespace Base.Application.Generics;

public class Repository : BaseService<Repository>, IGenericsRepository
{
    public Repository(
       IMapper mapper,
       ISqlConnection sqlConnection,
       ILogger<Repository> logger) :
       base(mapper, sqlConnection, logger)
    {
    }
    async Task<IEnumerable<dynamic>> IGenericsRepository.GetServiceDynamic(GenericsServiceDbDynamicEntry dto,CancellationToken cancellationToken)
    {
        if (dto is null)
        {
            throw new BadRequestException(ApiResultStatusCode.BadRequest);
        }
        var dbDynamic = dto.AutoCleanString().ToEntity(Mapper);
        IDbConnection connectionGetSpName = SqlConnection.GetDbConnectionAsync();
        string? spName = null;
        try
        {
            connectionGetSpName.Open();
            var parametersSpName = new DynamicParameters(new { Id = dbDynamic.ControllerId });
            //ارسال درخواست به دیتابیس و دریافت جواب از دیتابیس
            var data = await connectionGetSpName.QueryFirstAsync<Controller>(
                new CommandDefinition(
                    SystemStoredProcedure.GetController,
                    parametersSpName,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken));

            spName = data.SPName;

            if (spName is null)
            {
                throw new DatabaseExceptions(ApiResultStatusCode.ServiceUnavailable)
                {
                    HttpStatusCode = HttpStatusCode.ServiceUnavailable
                };
            }
            var dynamic = dbDynamic.DynamicProperty;
            var parameters = new DynamicParameters();
            parameters.Add("@Result", null, DbType.Int64, ParameterDirection.ReturnValue);
            foreach ((string key, dynamic value) in dynamic)
            {
                parameters.Add($"@{key}", value, null, ParameterDirection.Input);
            }
            //ارسال درخواست به دیتابیس و دریافت جواب از دیتابیس
            var dataMultiple = await connectionGetSpName.QueryMultipleAsync(
                new CommandDefinition(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken));
            //دریافت خطای خرروجی دیتابیس
            List<dynamic> result = new();
            while (true)
            {
                try
                {
                    result.Add(await dataMultiple.ReadAsync<dynamic>());
                }
                catch
                {
                    return result;
                }
            }
        }
        catch (DatabaseExceptions e)//درصورت بروز خطا سیستم به سمت ارور هندلر هدایت میشود
        {
            throw new DatabaseExceptions(ApiResultStatusCode.ServiceUnavailable)
            {
                HttpStatusCode = HttpStatusCode.ServiceUnavailable
            };
        }
        finally
        {
            connectionGetSpName.Close();
            connectionGetSpName.Dispose();
        }
    }
}
