namespace MyDiet_API.Utilities
{
    public static class Constants
    {
        public const string UNAUTHORIZED_ACCESS_RESOURCE = "Access not authorized to this resource";

        public const string NO_AUTHORIZATION_HEADER = "No Authorization header was found in the request";

        public const string INVALID_FORMAT_AUTHORIZATION_HEADER = "Invalid format of Bearer JWT Authorization Header";

        public const string TOKEN_NOT_VALID = "JWT Token not valid";

        public const string INTERNAL_SERVER_ERROR = "Internal Server Error";

        public const string LOGIN = "login";

        public const string REGISTER = "register";

        public const string AUTHORIZATION_HEADER = "Authorization";

        public const string BASIC_AUTHORIZATION_KEYWORD = "Basic";

        public const string BEARER_AUTHORIZATION_KEYWORD = "Bearer";

        public const string APPLICATION_JSON_CONTENT_TYPE = "application/json";
    }
}
