using System.IO;
using System.Diagnostics;

namespace ExtremeUltraDeepCleaner.Services
{
    /// <summary>
    /// Helper class for safe file system operations
    /// </summary>
    public static class FileSystemHelper
    {
        /// <summary>
        /// Checks if the application has write access to a path
        /// </summary>
        public static bool HasWriteAccess(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    return false;

                string testFile = Path.Combine(path, $"_test_{Guid.NewGuid()}.tmp");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates the total size of a directory (in bytes)
        /// </summary>
        public static long GetDirectorySize(string path)
        {
            if (!Directory.Exists(path))
                return 0;

            try
            {
                return Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
                    .Sum(file =>
                    {
                        try
                        {
                            return new FileInfo(file).Length;
                        }
                        catch
                        {
                            return 0;
                        }
                    });
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Safely deletes a file and returns the size freed
        /// </summary>
        public static async Task<long> SafeDeleteFileAsync(string filePath)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (!File.Exists(filePath))
                        return 0;

                    long size = new FileInfo(filePath).Length;
                    File.Delete(filePath);
                    return size;
                }
                catch
                {
                    return 0;
                }
            });

            return 0;
        }

        /// <summary>
        /// Safely deletes a directory and returns the size freed
        /// </summary>
        public static async Task<long> SafeDeleteDirectoryAsync(string directoryPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(directoryPath))
                        return 0;

                    long size = GetDirectorySize(directoryPath);
                    Directory.Delete(directoryPath, recursive: true);
                    return size;
                }
                catch
                {
                    return 0;
                }
            });
        }

        /// <summary>
        /// Kills Windows Explorer process
        /// </summary>
        public static async Task KillExplorerAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    var processes = Process.GetProcessesByName("explorer");
                    foreach (var process in processes)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                catch
                {
                    // Ignore errors
                }
            });
        }

        /// <summary>
        /// Starts Windows Explorer process
        /// </summary>
        public static async Task StartExplorerAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    Process.Start("explorer.exe");
                }
                catch
                {
                    // Ignore errors
                }
            });
        }

        /// <summary>
        /// Quickly checks if a directory exists (optimized)
        /// </summary>
        public static bool DirectoryExistsFast(string path)
        {
            try
            {
                return Directory.Exists(path);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Safely empties a directory without deleting it
        /// </summary>
        public static async Task<long> SafeEmptyDirectoryAsync(string directoryPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(directoryPath))
                        return 0;

                    long totalFreed = 0;

                    // Delete all files
                    foreach (var file in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            long size = new FileInfo(file).Length;
                            File.Delete(file);
                            totalFreed += size;
                        }
                        catch
                        {
                            // Silent skip
                        }
                    }

                    // Delete all subdirectories
                    foreach (var dir in Directory.EnumerateDirectories(directoryPath))
                    {
                        try
                        {
                            Directory.Delete(dir, recursive: true);
                        }
                        catch
                        {
                            // Silent skip
                        }
                    }

                    return totalFreed;
                }
                catch
                {
                    return 0;
                }
            });
        }
    }
}
