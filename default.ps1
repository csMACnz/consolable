properties {
    $base_dir = . resolve-path .\
}

# Probably will just build, decide later
task default

task restore {
    cd src/csMACnz.Consolable
    dotnet restore
    cd ../csMACnz.Consolable.Tests
    dotnet restore
}

task build {
    cd src/csMACnz.Consolable
    dotnet build
    cd ../csMACnz.Consolable.Tests
    dotnet build
}

task test {
    cd src/csMACnz.Consolable.Tests
    dotnet test
}

task pack {
    $package_dir = "$base_dir\pack"
    if (Test-Path $package_dir) {
        Remove-Item $package_dir -r
    }
    mkdir $package_dir

    cd src/csMACnz.Consolable
    dotnet pack -c Release -o $package_dir
}

