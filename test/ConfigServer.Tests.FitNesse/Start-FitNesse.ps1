$FitJarPath = "./fitness-standalone.jar"
$FitJarUrl = "http://www.fitnesse.org/fitnesse-standalone.jar?responder=releaseDownload&release=20161106"

if (!(Test-Path $FitJarPath)) {
    Invoke-WebRequest -Uri $FitJarUrl -OutFile $FitJarPath
}

Get-Command java -ErrorAction SilentlyContinue | Out-Null

if ($?) {
    # java is installed
} else {
    throw "Java is not installed! Chocolatey makes this really easy: https://chocolatey.org/packages/javaruntime"
}

java -jar $FitJarPath -p 32700
