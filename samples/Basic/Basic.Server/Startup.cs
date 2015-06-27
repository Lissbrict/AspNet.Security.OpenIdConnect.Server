using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Owin;
using Owin;
using Owin.Security.OpenIdConnect.Server;

namespace Basic.Server {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            var certificate = GetCertificate();
            var credentials = new X509SigningCredentials(certificate);

            app.UseOpenIdConnectServer(new OpenIdConnectServerOptions {
                AccessTokenLifetime = TimeSpan.FromDays(14),
                IdentityTokenLifetime = TimeSpan.FromMinutes(60),

                SigningCredentials = credentials,

                TokenEndpointPath = new PathString("/token"),
                AuthorizationEndpointPath = new PathString("/auth.cshtml"),

                Provider = new AuthorizationProvider(),
                AllowInsecureHttp = true,
                ApplicationCanDisplayErrors = true
            });
        }

        private static X509Certificate2 GetCertificate() {
            // Note: in a real world app, you'd probably prefer storing the X.509 certificate
            // in the user or machine store. To keep this sample easy to use, the certificate
            // is extracted from the Certificate.pfx file embedded in this assembly.
            using (var stream = typeof(Startup).Assembly.GetManifestResourceStream("Basic.Server.Certificate.pfx"))
            using (var buffer = new MemoryStream()) {
                stream.CopyTo(buffer);
                buffer.Flush();

                return new X509Certificate2(
                    rawData: buffer.GetBuffer(),
                    password: "Owin.Security.OpenIdConnect.Server");
            }
        }
    }
}