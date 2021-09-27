namespace SimpleE2ETesterLibrary.Models
{
    public static class StatusCodes
    {
        public static bool IsSuccessfulResponse(int statusCode)
        {
            return statusCode >= 200 && statusCode < 300;
        }

        public static bool IsClientErrorResponse(int statusCode)
        {
            return statusCode >= 400 && statusCode < 500;
        }

        public static bool IsServerErrorResponse(int statusCode)
        {
            return statusCode >= 500 && statusCode < 600;
        }
    }
}