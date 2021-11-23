// <copyright file="Startup.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Management.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.Management.Batch;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.OpenApi.Models;
    using Microsoft.Rest;
    using Teqniqly.AzBatch.Management.Abstractions;
    using Teqniqly.AzBatch.Management.Infrastructure;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AzBatch.Management.Api", Version = "v1" });
            });

            var azureAdConfiguration = this
                .Configuration
                .GetSection("AzureAd")
                .Get<AzureAdConfiguration>();

            services.AddSingleton<IBatchManagementService, BatchManagementService>();

            services.AddSingleton<BatchManagementServiceConfiguration>(provider => this
                .Configuration
                .GetSection("BatchManagementServiceConfiguration")
                .Get<BatchManagementServiceConfiguration>());

            ConfigureBatchManagementClient(services, azureAdConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AzBatch.Management.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureBatchManagementClient(
            IServiceCollection services,
            AzureAdConfiguration azureAdConfiguration)
        {
            var clientCredential = new ClientCredential(
                azureAdConfiguration.ClientId,
                azureAdConfiguration.ClientSecret);

            var authContext = new AuthenticationContext($"{azureAdConfiguration.Instance}/{azureAdConfiguration.TenantId}");
            var resource = "https://management.core.windows.net/";

            var authResult = authContext.AcquireTokenAsync(
                    resource,
                    clientCredential)
                .Result;

            var token = authResult.AccessToken;

            services.AddSingleton<IBatchManagementClient>(provider =>
            {
                var client = new BatchManagementClient(new TokenCredentials(token))
                {
                   SubscriptionId = azureAdConfiguration.SubscriptionId,
                };

                return client;
            });
        }
    }
}
