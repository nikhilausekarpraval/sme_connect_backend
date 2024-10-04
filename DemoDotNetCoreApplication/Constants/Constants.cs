namespace DemoDotNetCoreApplication.Constatns
{
    public class Constants
    {
        public static class ApiResponseType
        {
            public const string Success = "Success";

            public const string Failure = "Failure";

            public const string NotFound = "NotFound";
        }

        public static string[] Claims =  { "DeleteTask", "ViewReport", "ManageSystem" };


    }
}
