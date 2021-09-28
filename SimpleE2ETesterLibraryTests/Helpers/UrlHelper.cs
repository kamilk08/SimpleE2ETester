namespace SimpleE2ETesterLibraryTests.Helpers
{
    public class UrlHelper
    {
        public static string Delete() => @"http://localhost:5000/api/delete/{dto}";
        public static string Get(int id) => $"http://localhost:5000/api/get/{id}";
        public static string Put() => @"http://localhost:5000/api/put/{dto}";
        public static string Post() => @"http://localhost:5000/api/post/{dto}";
    }
}