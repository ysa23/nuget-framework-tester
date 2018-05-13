using CommandLine;

namespace NuGetReader
{
	public class Options
	{
		[Option('n', "name", Required = true, Default = "", HelpText = "Name of the NuGet package")]
		public string Name { get; set; }
		[Option('n', "version", Required = true, Default = "", HelpText = "Version of the NuGet package")]
		public string Version { get; set; }
		[Option('f', "framework", Required = false, Default = ".NETStandard", HelpText = "Framework to test")]
		public string Framework { get; set; }
	}
}