namespace Shared.Interfaces.Options {
    public class IAuthOptions : IBaseOptions {
        public class IJwtOptions {
            public string SecurityKey { get; set; }
            public string EncryptionAlgorithm { get; set; }
            public class ITokenOptions {
                public double Expiration { get; set; }
                public class IFieldsOptions {
                    public string UserId { get; set; }
                    public string Expiration { get; set; }
                }
                public IFieldsOptions Fields { get; set; }
            }
            public ITokenOptions Token { get; set; }
        }
        public IJwtOptions Jwt { get; set; }

        public class IAuthEndpointsOptions {
            public IEndpointOptions VerifyToken { get; set; }
        }
        public IAuthEndpointsOptions Endpoints { get; set; }
    }
}
