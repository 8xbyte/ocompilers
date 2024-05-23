namespace AuthMicroservice.Interfaces {
    public class ILoginHttpRequest {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ILoginHttpResponse {
        public string Status { get; set; }
    }
}
