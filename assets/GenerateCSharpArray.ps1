# PowerShell script: GenerateCSharpArray.ps1

# Path to your input text file (one name per line)
$inputFile = "last-names.txt"

# Path to output C# file
$outputFile = "LastNamesArray.cs"

# Number of names per line in the array
$namesPerLine = 10

# Read all names, trim whitespace, and filter out empty lines
$names = Get-Content $inputFile | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne "" }

# Start the array declaration
@"
public static readonly string[] LastNames = {
"@ | Set-Content $outputFile

# Write names in groups
for ($i = 0; $i -lt $names.Count; $i += $namesPerLine) {
    $group = $names[$i..([Math]::Min($i + $namesPerLine - 1, $names.Count - 1))]
    $line = ($group | ForEach-Object { "`"$($_)`"" }) -join ", "
    if ($i + $namesPerLine -lt $names.Count) {
        $line += ","
    }
    Add-Content $outputFile "    $line"
}

# End the array declaration
Add-Content $outputFile "};"