namespace MockyProducts.UnitTests.Common
{
    internal static class CommonUnitTests
    {
        public static string GetTestDataFolder(string testDataFolder)
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(startupPath, testDataFolder);
            //var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            //var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            //string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            //return Path.Combine(projectPath, "Test_Data", testDataFolder);
            return path;
        }

        public static string ReadSampleJsonFile(string Folder, string filename)
        {
            var fullFileName = Path.Combine(CommonUnitTests.GetTestDataFolder(Folder), filename) + ".json";

            var jsonText = File.ReadAllText(fullFileName);

            return jsonText;
        }
    }
}
