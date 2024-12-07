namespace ImageDetector.Services
{
    public class ApiKeyService
    {
        private readonly HashSet<string> _validApiKeys;

        public ApiKeyService()
        {
            _validApiKeys = new HashSet<string> { "LkPdXc7QhdvRpOg36V5aMYlZz2tf81WsAqBhD49G", };
        }

        public bool IsValidApiKey(string apiKey)
        {
            return _validApiKeys.Contains(apiKey);
        }
    }
}
