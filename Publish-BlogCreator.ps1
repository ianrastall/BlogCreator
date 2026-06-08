param(
    [string]$Configuration = "Release",
    [string]$RuntimeIdentifier = "win-x64",
    [string]$OutputDirectory = "Build"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

function Test-IsPathInside {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [string]$ParentPath
    )

    $normalizedPath = [System.IO.Path]::GetFullPath($Path)
    $normalizedParent = [System.IO.Path]::GetFullPath($ParentPath).TrimEnd('\', '/')

    return $normalizedPath.Equals($normalizedParent, [System.StringComparison]::OrdinalIgnoreCase) -or
        $normalizedPath.StartsWith(
            $normalizedParent + [System.IO.Path]::DirectorySeparatorChar,
            [System.StringComparison]::OrdinalIgnoreCase)
}

$repositoryRoot = $PSScriptRoot
$projectPath = Join-Path $repositoryRoot "src\BlogCreator.WinUI\BlogCreator.WinUI.csproj"

if ([System.IO.Path]::IsPathRooted($OutputDirectory)) {
    $outputPath = [System.IO.Path]::GetFullPath($OutputDirectory)
}
else {
    $outputPath = [System.IO.Path]::GetFullPath((Join-Path $repositoryRoot $OutputDirectory))
}

if (-not (Test-IsPathInside -Path $outputPath -ParentPath $repositoryRoot)) {
    throw "OutputDirectory must be inside the repository. Refusing to publish to '$outputPath'."
}

if (Test-Path -LiteralPath $outputPath) {
    Remove-Item -LiteralPath $outputPath -Recurse -Force
}

New-Item -ItemType Directory -Path $outputPath | Out-Null

$publishArguments = @(
    "publish",
    $projectPath,
    "-c",
    $Configuration,
    "-r",
    $RuntimeIdentifier,
    "--self-contained",
    "true",
    "-o",
    $outputPath,
    "/p:PublishSingleFile=true",
    "/p:EnableCompressionInSingleFile=false",
    "/p:IncludeNativeLibrariesForSelfExtract=true",
    "/p:DebugType=None",
    "/p:DebugSymbols=false"
)

& dotnet @publishArguments
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

$executablePath = Join-Path $outputPath "BlogCreator.WinUI.exe"
if (-not (Test-Path -LiteralPath $executablePath)) {
    throw "Publish completed, but '$executablePath' was not created."
}

$publishedFiles = @(Get-ChildItem -LiteralPath $outputPath -File)
if ($publishedFiles.Count -ne 1) {
    Write-Warning "Expected one published executable, but found $($publishedFiles.Count) files in '$outputPath'."
}

Write-Host ""
Write-Host "Published BlogCreator:"
Write-Host $executablePath
