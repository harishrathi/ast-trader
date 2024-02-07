using AstTrader.Server.AppServices;

namespace AstTrader.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServices();
            var app = builder.Build();
            app.UseServices();
            app.Run();
        }
    }
}
