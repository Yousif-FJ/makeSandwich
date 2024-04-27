namespace server_a.Helpers
{
    public static class DbStringCreator
    {
        public static string CreateDbString(this IConfiguration configuration)
        {
            var serverHost = configuration.GetValue<string>("mssqlHost");
            var password = configuration.GetValue<string>("mssqlPassword");
            Console.WriteLine($"Server host: {serverHost}");
            return $"Server={serverHost};Database=sandwichUsersDb;User Id=sa;Password={password};TrustServerCertificate=True;";
        }
    }
}