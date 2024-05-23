using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Shared.WebSocket.Extensions {
    public static class WebSocketSendExtension {
        private static JsonSerializerOptions jsonSerializerOptions = new() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static async Task SendAsync(this System.Net.WebSockets.WebSocket webSocket, string message) {
            await webSocket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, WebSocketMessageFlags.EndOfMessage, CancellationToken.None);
        }

        public static async Task SendAsync<ValueType>(this System.Net.WebSockets.WebSocket webSocket, ValueType messageObject) {
            var message = JsonSerializer.Serialize(messageObject, jsonSerializerOptions);
            await webSocket.SendAsync(message);
        }
    }
}
