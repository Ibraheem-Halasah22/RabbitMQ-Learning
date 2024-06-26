﻿namespace DotEnv

{
    using System;
    using System.IO;

    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }

        public static string GetDotEnvPathInTheSolutionDirectory()
        {
            var root = Directory.GetCurrentDirectory();
            while (root != null && !Directory.GetFiles(root, "*.sln").Any())
            {
                root = Directory.GetParent(root).FullName;
            }
            var dotenv = Path.Combine(root, ".env");
            return dotenv;
        }
        public static void LoadDotEnvPathFromTheSolutionDirectory()
        {
            var path = GetDotEnvPathInTheSolutionDirectory();
            Load(path);
        }
    }
}