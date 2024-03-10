# Function to recursively delete bin and obj folders
function Remove-BinObjFolders {
    param (
        [string]$path
    )
    
    # Get all directories in the current path
    $directories = Get-ChildItem -Path $path -Directory -Force
    
    # Iterate through each directory
    foreach ($directory in $directories) {
        # Check if the directory name matches "bin" or "obj"
        if ($directory.Name -eq "bin" -or $directory.Name -eq "obj") {
            # Remove the directory and all its contents
            Remove-Item -Path $directory.FullName -Recurse -Force
            Write-Host "Deleted $($directory.FullName)"
        }
        else {
            # Recursively call the function for each subdirectory
            Remove-BinObjFolders -path $directory.FullName
        }
    }
}

# Get the current directory
$currentDirectory = Get-Location

# Call the function to remove bin and obj folders recursively
Remove-BinObjFolders -path $currentDirectory