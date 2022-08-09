namespace permissions.Constant
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string module)
        {
            return new List<string>
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Update",
                $"Permissions.{module}.Delete",
            };
        }

        public static List<string> GenerateAllPermissions()
        {
            var allPermission = new List<string>();

            var modules = Enum.GetValues(typeof(Modules));

            foreach (var module in modules)
                allPermission.AddRange(GeneratePermissionsList(module.ToString()));

            return allPermission;
        }

        public static class AddEmployee
        {
            public const string View = "Permissions.addUsers.View";
            public const string Create = "Permissions.addUsers.Create";
            public const string Update = "Permissions.addUsers.Update";
            public const string Delete = "Permissions.addUsers.Delete";
        }
    }
}
