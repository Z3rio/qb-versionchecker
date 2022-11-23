using static VersionChecker.Utils.Github;
using static VersionChecker.Models.ResourceModel;
using static VersionChecker.Utils.Sorting;

namespace VersionChecker.Commands
{
    internal class ListCmd
    {
        public static async void Run()
        {
            Console.WriteLine("Input your server path (Main server folder or resources folder)");
            string? path = Console.ReadLine();

            if (path != null)
            {
                if (Directory.Exists(path))
                {
                    DirectoryInfo? resourceFolder = new DirectoryInfo(path);

                    if (resourceFolder != null)
                    {
                        if (resourceFolder.Name != "resources")
                        {
                            path += "\\resources";

                            resourceFolder = new DirectoryInfo(path);
                        }

                        if (Directory.Exists(path))
                        {

                            List<string> dirs = new List<string> { };

                            try
                            {
                                dirs = Directory.GetDirectories(path, "*qb-*", System.IO.SearchOption.AllDirectories).ToList();
                            }
                            catch (UnauthorizedAccessException)
                            {
                                dirs = new List<string> { };
                            }

                            List<Resource> resources = new List<Resource> { };

                            foreach (var dir in dirs)
                            {
                                DirectoryInfo resource = new DirectoryInfo(dir);

                                bool valid = await IsValid($"https://github.com/qbcore-framework/{resource.Name}");

                                Console.WriteLine($"Fetching data for \"{resource.Name}\"");

                                if (valid == true)
                                {
                                    HttpResponseMessage res = await client.GetAsync($"https://raw.githubusercontent.com/qbcore-framework/{resource.Name}/main/fxmanifest.lua");

                                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        string? localVersionNumber = null;

                                        string currentFxmanifest = await res.Content.ReadAsStringAsync();
                                        string? currentVersionNumber = GetVersionNumber(currentFxmanifest);

                                        string? localFxmanifest = System.IO.File.ReadAllText($"{resource.FullName}\\fxmanifest.lua");

                                        if (localFxmanifest != null)
                                        {
                                            localVersionNumber = GetVersionNumber(localFxmanifest);
                                        }

                                        resources.Add(new Resource
                                        {
                                            name = resource.Name,

                                            versions = new Versions
                                            {
                                                current = currentVersionNumber,
                                                local = localVersionNumber
                                            }
                                        });
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"\"{resource.Name}\" is not an official QBCore resource");
                                }
                            }

                            Console.Clear();

                            resources = SortResources(resources);

                            foreach (Resource resource in resources)
                            {

                                if (resource != null && resource.name != null)
                                {
                                    string statusLabel = "";

                                    if (resource.versions != null && resource.versions.current != null && resource.versions.local != null)
                                    {
                                        if (resource.versions.current == resource.versions.local)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            statusLabel = "The script is up to date";
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            statusLabel = "The script is outdated";
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        statusLabel = "Couldn't find a valid version";
                                    }

                                    Console.WriteLine($"{resource.name}, {statusLabel}");
                                }
                            }

                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("The server path is invalid");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The server path is invalid");
                    }
                }
                else
                {
                    Console.WriteLine("The server path is invalid");
                }
            }
            else
            {
                Console.WriteLine("The server path is invalid");
            }
        }
    }
}
