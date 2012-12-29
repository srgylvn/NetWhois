using System.Net;

namespace NetWhois.Imp.Settings
{
    public interface ISettings
    {
        EndPoint Bind { get; } 
    }
}