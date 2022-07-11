namespace Base.WebApi.Configuration;
public class JobFactory : SimpleJobFactory
{
    private IServiceProvider ServiceProvider { get; }

    public JobFactory(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        try
        {
            return (IJob)this.ServiceProvider.GetService(bundle.JobDetail.JobType);
        }
        catch (Exception e)
        {
            throw new TaskSchedulerException($"");
        }
    }
}