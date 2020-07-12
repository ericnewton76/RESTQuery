if($env:APPVEYOR_REPO_TAG -eq "true" -And $env:APPVEYOR_REPO_TAG_NAME.StartsWith("v") -eq $true) {

	$build = $env:APPVEYOR_REPO_TAG_NAME.Substring(1)

	Write-Host "Setting Build to '$build'."
	Update-AppveyorBuild -Version $build

	.\Reset-BuildNumber
	
}