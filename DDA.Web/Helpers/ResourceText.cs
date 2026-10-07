using DDA.Resources;

namespace DDA.Web.Helpers
{
    /// <summary>Translates a resource key (returned by the business layer) using the current UI culture.</summary>
    public static class ResourceText
    {
        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            return Resource.ResourceManager.GetString(key, Resource.Culture) ?? key;
        }

        public static string Format(string key, params object[] args)
        {
            return string.Format(Get(key), args);
        }
    }
}