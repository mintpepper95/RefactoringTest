namespace Refactoring.LegacyService;

public class DatabaseOptions {
    // Expecting config file to look like 
    // {
    //   "ConnectionStrings" : {
    //      "ApplicationDatabase": "xxx"
    //   }
    // }
    //
    public const string SectionName = "ConnectionStrings";

    public string ApplicationDatabase { get; set; } = string.Empty;
}