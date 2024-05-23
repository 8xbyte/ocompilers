namespace AuthMicroservice.Interfaces {
    public class IRegisterHttpRequest {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class IRegisterHttpResponse {
        public string Status { get; set; }
    }
}
