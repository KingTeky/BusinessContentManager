namespace BlossomTreeManager.Web.Data.Constants;

public static class ApplicationRoles
{
    public const string AppOwner = "AppOwner";
    public const string CompanyStaff = "CompanyStaff";
    public const string SchoolAdmin = "SchoolAdmin";
    public const string SchoolStaff = "SchoolStaff";
    public const string Teacher = "Teacher";
    public const string Parent = "Parent";

    public static readonly string[] All =
    [
        AppOwner,
        CompanyStaff,
        SchoolAdmin,
        SchoolStaff,
        Teacher,
        Parent
    ];
}
