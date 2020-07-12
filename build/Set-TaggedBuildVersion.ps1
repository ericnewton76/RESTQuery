if($env:APPVEYOR_REPO_TAG -eq "true" -And $env:APPVEYOR_REPO_TAG_NAME.StartsWith("v") -eq $true) {

	& "$PSScriptRoot\Reset-BuildNumber"

	$build = $env:APPVEYOR_REPO_TAG_NAME.Substring(1) + ".0"

	Write-Host "Setting Build to '$build'."
	
	if($env:APPVEYOR -eq "True") {
		Update-AppveyorBuild -Version $build
	}
	
}