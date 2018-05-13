## Example of usage

```powershell
Import-Csv <LIST OF PACKAGES (Name, Version columns)> | ForEach-Object { dotnet .\NuGet.FrameworkSupportTester.dll -- --name $_.Name --version $_.Version } | Out-File result.txt
```