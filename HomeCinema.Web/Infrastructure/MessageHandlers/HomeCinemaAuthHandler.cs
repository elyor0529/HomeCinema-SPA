using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HomeCinema.Web.Infrastructure.Extensions;

namespace HomeCinema.Web.Infrastructure.MessageHandlers
{
    public class HomeCinemaAuthHandler : DelegatingHandler
    {
        private IEnumerable<string> _authHeaderValues;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                request.Headers.TryGetValues("Authorization", out _authHeaderValues);

                if (_authHeaderValues == null)
                    return base.SendAsync(request, cancellationToken);

                var tokens = _authHeaderValues.FirstOrDefault()?.Replace("Basic", "").Trim();

                if (!string.IsNullOrEmpty(tokens))
                {
                    var data = Convert.FromBase64String(tokens);
                    var decodedString = Encoding.UTF8.GetString(data);
                    var tokensValues = decodedString.Split(':');
                    var membershipService = request.GetMembershipService();
                    var membershipCtx = membershipService.ValidateUser(tokensValues[0], tokensValues[1]);

                    if (membershipCtx.User != null)
                    {
                        var principal = membershipCtx.Principal;
                        Thread.CurrentPrincipal = principal;
                        HttpContext.Current.User = principal;
                    }
                    else
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        var tsc = new TaskCompletionSource<HttpResponseMessage>();
                        tsc.SetResult(response);
                        return tsc.Task;
                    }
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(response);
                    return tsc.Task;
                }
                return base.SendAsync(request, cancellationToken);
            }
            catch
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
        }
    }
}