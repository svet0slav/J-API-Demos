namespace RockPapSci.UnitTests.Common
{
    internal static class CommonUnitTests
    {
        public static string GetTestDataFolder(string testDataFolder)
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(startupPath, testDataFolder);
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
