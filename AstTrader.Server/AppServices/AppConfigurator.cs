namespace AstTrader.Server.AppServices
{
    public static class AppConfigurator
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            // swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        public static void UseServices(this WebApplication app)
        {
            app.UseDefaultFiles(); // for index.html
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");
        }
    }
}
