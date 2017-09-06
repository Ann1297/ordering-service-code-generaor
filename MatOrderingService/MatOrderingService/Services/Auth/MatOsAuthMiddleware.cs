using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MatOrderingService.Services.Auth
{
    public class MatOsAuthMiddleware : AuthenticationMiddleware<MatOsAuthOptions>
    {
        public MatOsAuthMiddleware(RequestDelegate next, IOptions<MatOsAuthOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder)
            : base (next, options, loggerFactory, encoder)
        {
            if (next == null)
            {
                throw new ArgumentException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentException(nameof(loggerFactory));
            }

            if (encoder == null)
            {
                throw new ArgumentException(nameof(encoder));
            }
        }
        protected override AuthenticationHandler<MatOsAuthOptions> CreateHandler()
        {
            return new MatOsAuthHandler();
        }
    }
}
