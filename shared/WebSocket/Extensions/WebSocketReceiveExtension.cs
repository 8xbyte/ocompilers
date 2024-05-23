using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Shared.WebSocket.Extensions {
    public static class WebSocketReceiveExtension {
        private static JsonSerializerOptions jsonSerializerOptions = new() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static async Task<string> ReceiveAsync(this System.Net.WebSockets.WebSocket webSocket) {
            var buffer = new byte[4096];
            var response = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            return Encoding.UTF8.GetString(buffer, 0, response.Count);
        }

        public static async Task<ResultType?> ReceiveAsync<ResultType>(this System.Net.WebSockets.WebSocket webSocket) {
            var buffer = new byte[4096];
            var response = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            return JsonSerializer.Deserialize<ResultType>(Encoding.UTF8.GetString(buffer, 0, response.Count), jsonSerializerOptions);
        }
    }
}
