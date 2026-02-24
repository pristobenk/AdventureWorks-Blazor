using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace AdventureWorks.WebApi.Extensions;

public static class CompressionProviderExtentions
{
    internal static IServiceCollection AddCompressionProvider(this IServiceCollection services)
    {
        // Response compression for static assets (Brotli + Gzip)
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            // include common static file MIME types and wasm/octet-stream
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
            {
                    "application/wasm",
                    "application/octet-stream"
            });
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(opts => opts.Level = CompressionLevel.Fastest);
        services.Configure<GzipCompressionProviderOptions>(opts => opts.Level = CompressionLevel.Fastest);

        return services;
    }
}
