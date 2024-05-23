namespace Shared.Interfaces.Options {
    public class IUserOptions : IBaseOptions {
        public IDatabaseOptions Database { get; set; }

        public class IUserEndpointsOptions {
            public IEndpointOptions GetUserById { get; set; }
            public IEndpointOptions GetUserByEmail { get; set; }
            public IEndpointOptions CreateUser { get; set; }
            public IEndpointOptions UpdateUser { get; set; }
            public IEndpointOptions RemoveUserById { get; set; }
            public IEndpointOptions RemoveUserByEmail { get; set; }
        }
        public IUserEndpointsOptions Endpoints { get; set; }
    }
}
