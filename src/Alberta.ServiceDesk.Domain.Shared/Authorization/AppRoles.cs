using System;

namespace Alberta.ServiceDesk.Authorization;

public static class AppRoles
{
    public const string Student = "Student";
    public const string Teacher = "Teacher";
    public const string Admin = "Admin";
    
    // Deprecated: Use Admin instead
    [Obsolete("Use AppRoles.Admin instead")]
    public const string DepartmentAdmin = "DepartmentAdmin";
    
    [Obsolete("Use AppRoles.Admin instead")]
    public const string SchoolAdmin = "SchoolAdmin";
}
