using Microsoft.IdentityModel.Protocols;

namespace WebApiBrowler.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }

        public static class Roles
        {
            public const string Admin = "Admin"; // @
            public const string Voice = "Voice"; // +
            public const string User = "User"; // regular Joe

        }

        public static class Permission
        {
            public const string View = "View";
            public const string Create = "Create";
            public const string Update = "Update";
            public const string Delete = "Delete";
        }

        public static class CustomClaimTypes
        {
            public const string Permission = "Permission";
        }
    }
}
