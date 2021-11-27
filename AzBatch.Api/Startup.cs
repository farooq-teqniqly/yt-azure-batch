// <copyright file="Startup.cs" company="Teqniqly">
// Copyright (c) Teqniqly. All rights reserved.
// </copyright>

namespace Teqniqly.AzBatch.Api
{
    using System;
    using Azure.Storage;
    using Azure.Storage.Blobs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.Batch;
    using Microsoft.Azure.Batch.Auth;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Teqniqly.AzBatch.Abstractions;
    using Teqniqly.AzBatch.Infrastructure;

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AzBatch.Api", Version = "v1" });
            });

            services.AddSingleton<IBatchService, BatchService>();

            var batchServiceConfiguration = this
                .Configuration
                .GetSection("BatchServiceConfiguration")
                .Get<BatchServiceConfiguration>();

            services.AddSingleton<BatchClient>(provider =>
            {
                var credentials = new BatchSharedKeyCredentials(
                    batchServiceConfiguration.ConnectionConfiguration.EndpointUri,
                    batchServiceConfiguration.ConnectionConfiguration.AccountName,
                    batchServiceConfiguration.ConnectionConfiguration.AccountKey);

                return BatchClient.Open(credentials);
            });

            services.AddSingleton<BlobServiceClient>(provider => new BlobServiceClient(
                new Uri(
                    $"https://{batchServiceConfiguration.StorageConfiguration.AccountName}.blob.core.windows.net"),
                new StorageSharedKeyCredential(
                    batchServiceConfiguration.StorageConfiguration.AccountName,
                    batchServiceConfiguration.StorageConfiguration.AccountKey)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AzBatch.Api v1"));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
