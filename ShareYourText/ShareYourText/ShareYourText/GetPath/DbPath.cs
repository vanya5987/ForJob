namespace ShareYourText.GetPath
{
    internal class DbPath
    {
        public static string GetPath(string type)
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dataDirectory = Path.Combine(projectDirectory, "Data");

            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);

            return Path.Combine(dataDirectory, $"{type}.db");
        }
    }
}
