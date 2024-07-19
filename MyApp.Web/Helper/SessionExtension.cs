namespace MyApp.Web.Helper
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var val = System.Text.Json.JsonSerializer.Serialize(value);
            session.SetString(key, val);
        }
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
                return default;
            else
                return System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
        public static void Remove(this ISession session, string key)
        {
            session.Remove(key);
        }


    }
}
