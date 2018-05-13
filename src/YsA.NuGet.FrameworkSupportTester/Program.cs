using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using NuGet.Packaging;

namespace NuGetReader
{
	class Program
	{
		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(Run);
		}

		private static void Run(Options opt)
		{
			var userProfile = GetUserProfileDirectory();

			var packages = new List<(string Name, string Version)>
			{
				(opt.Name, opt.Version)
			};

			foreach (var packageDetails in packages)
			{
				var packagePath = Path.Combine(userProfile, ".nuget", "packages", packageDetails.Name, packageDetails.Version);

				if (!Directory.Exists(packagePath))
				{
					Console.WriteLine($"{packageDetails.Name}\t{packageDetails.Version}\tNot found locally. Please download it first.");
					continue;
				}
				
				var nugetFilePath = Directory.GetFiles(packagePath, "*.nupkg", SearchOption.AllDirectories).FirstOrDefault();
				if (nugetFilePath == null)
				{
					Console.WriteLine($"{packageDetails.Name}\t{packageDetails.Version}\tNot found locally. Please download it first.");
					continue;
				}

				CheckPackage(nugetFilePath, opt.Framework, packageDetails);
			}
		}

		private static string GetUserProfileDirectory()
		{
			var userProfile = Environment.GetEnvironmentVariable("userprofile");
			if (string.IsNullOrEmpty(userProfile))
				userProfile = "~";
			return userProfile;
		}

		private static void CheckPackage(string nugetFilePath, string framework, (string Name, string Version) packageDetails)
		{
			using (var package = new PackageArchiveReader(nugetFilePath))
			{
				var frameworks = package
					.GetSupportedFrameworks()
					.Where(f => f.DotNetFrameworkName.IndexOf(framework, StringComparison.OrdinalIgnoreCase) >= 0)
					.ToArray();

				Console.WriteLine(frameworks.Length == 0
					? $"{packageDetails.Name}\t{packageDetails.Version}\tNo support for .Net standard"
					: $"{packageDetails.Name}\t{packageDetails.Version}\t{string.Join(";", (object[]) frameworks)}");
			}
		}
	}
}
