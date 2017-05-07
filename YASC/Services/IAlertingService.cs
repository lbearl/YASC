namespace YASC.Services
{
    public interface IAlertingService
    {
        void AddJob(string url, string emailAddress);
    }
}
