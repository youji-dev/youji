using Quartz;

namespace Application.WebApi.Jobs
{
    public abstract class JobBase
    {
        protected T? GetJobData<T>(IJobExecutionContext context, string key)
        {
            return context.JobDetail.JobDataMap[key] switch
            {
                T value => value,
                _ => default,
            };
        }

        protected void SetJobData<T>(IJobExecutionContext context, string key, T? value)
        {
            context.JobDetail.JobDataMap[key] = value!;
        }
    }
}
