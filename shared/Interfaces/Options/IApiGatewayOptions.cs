namespace Shared.Interfaces.Options {
    public class IApiGatewayOptions : IBaseOptions {
        public class IHttpOptions {
            public class ICookiesOptions {
                public string AuthToken { get; set; }
            }
            public ICookiesOptions Cookies { get; set; }

            public class IHeadersOptions {
                public string UserId { get; set; }
            }
            public IHeadersOptions Headers { get; set; }
        }
        public IHttpOptions Http { get; set; }
    }
}
