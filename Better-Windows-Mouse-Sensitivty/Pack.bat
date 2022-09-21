:: %1 ProjectPath		"$(ProjectPath)"
:: %2 SolutionDir		"$(SolutionDir)"
:: %3 PackageID			"$(TargetName)" 
:: %4 PackageVersion	"$(PackageVersion)" 
:: %5 FrameworkVersion	"$(TargetFramework.TrimEnd('-windows'))" Squirrel needs net6.0 and not net6.0-windows
:: %6 PackDirectory		"$(TargetDir.TrimEnd('\''))" Squirrel needs the TargetDir withouth the trailing '\'
:: %7 ReleaseDir		"$(ProjectDir)Releases"

FOR /F "skip=1 tokens=2,4" %%a in ('dotnet list %1 package') DO IF "%%a" == "Clowd.Squirrel" set squirrelVersion=%%b

set "temp=%2""
set solutionDir=%temp:"=%

set "exePath=%solutionDir%Packages\clowd.squirrel\%squirrelVersion%\tools\Squirrel.exe"



"%exePath%" pack --packId %3 --packVersion %4 --framework %5 --packDirectory %6 --releaseDir %7