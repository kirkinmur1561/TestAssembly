using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Common;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] pluginPaths = new string[]
            {
                // Paths to plugins to load.
                @"S:\ReposProject\TestAssembly\Model_1\bin\Debug\net5.0\Model_1.dll",
                @"S:\ReposProject\TestAssembly\Model_2\bin\Debug\net5.0\Model_2.dll"
            };
            
            IEnumerable<IModel> commands = pluginPaths.SelectMany(pluginPath =>
            {
                Assembly pluginAssembly = LoadPlugin(pluginPath);
                return CreateCommands(pluginAssembly);
            }).ToList();
            
            foreach (IModel command in commands)
            {
                command.Exe();
                Console.WriteLine($"{command.Name}\t - {command.Description}");
            }
            
            Console.WriteLine("Hello World!");
        }
        
        static Assembly LoadPlugin(string relativePath)
        {
            // Navigate up to the solution root
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            //Console.WriteLine($"Loading commands from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }
        
        static IEnumerable<IModel> CreateCommands(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(IModel).IsAssignableFrom(type))
                {
                    IModel result = Activator.CreateInstance(type) as IModel;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                throw new ApplicationException(
                    $"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }
    }
}