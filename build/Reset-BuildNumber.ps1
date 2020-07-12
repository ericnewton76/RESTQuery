param($buildNumber)

if([string]::IsNullOrEmpty($buildNumber) -eq $true) {
	$buildNumber = 0
}

$headers = @{
	"Authorization" = "Bearer $env:APPVEYOR_API_TOKEN"
	"Content-type" = "application/json"
	"Accept" = "application/json"
}
$build = @{
	nextBuildNumber = $buildNumber
}
$json = $build | ConvertTo-Json

if($env:APPVEYOR -eq "True") {
	Invoke-RestMethod -Method Put "https://ci.appveyor.com/api/projects/$env:APPVEYOR_ACCOUNT_NAME/$env:APPVEYOR_PROJECT_SLUG/settings/build-number" -Body $json -Headers $headers
} else {
	Write-Host $json
}