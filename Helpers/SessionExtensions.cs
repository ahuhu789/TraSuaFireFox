using System.Text.Json;

namespace TraSuaFireFox.Helpers
{
    public static class SessionExtensions
    {
        // Lưu object vào Session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Lấy object từ Session
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}