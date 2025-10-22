# Deploy TorneoSolar to Azure App Service using Zip Deploy
param(
    [Parameter(Mandatory = $true)] [string] $PublishProfilePath,
    [Parameter(Mandatory = $true)] [string] $ZipPath
)

if (!(Test-Path $PublishProfilePath)) {
    Write-Error "Publish profile not found: $PublishProfilePath"; exit 1
}
if (!(Test-Path $ZipPath)) {
    Write-Error "Zip not found: $ZipPath"; exit 1
}

[xml]$xml = Get-Content -Path $PublishProfilePath
$profile = $xml.SelectSingleNode('//publishProfile[@publishMethod="MSDeploy"]')
if (-not $profile) { $profile = $xml.SelectSingleNode('//publishProfile[@publishMethod="ZipDeploy"]') }
if (-not $profile) { Write-Error "No compatible publishProfile entry found (MSDeploy/ZipDeploy)."; exit 1 }

$userName = $profile.userName
$userPWD  = $profile.userPWD
$profileName = $profile.profileName
$siteName = $profile.msdeploySite
$destinationAppUrl = $xml.SelectSingleNode('//publishProfile[@publishMethod="MSDeploy"]/@destinationAppUrl')

if (-not $siteName -and $profile.profileName) { $siteName = $profile.profileName }

# Construct Kudu (SCM) URL for Zip Deploy
$scmUrl = "https://$siteName.scm.azurewebsites.net/api/zipdeploy"

Write-Host "Deploying $ZipPath to $siteName using $profileName ..."

$pair = "$userName`:$userPWD"
$bytes = [System.Text.Encoding]::ASCII.GetBytes($pair)
$token = [Convert]::ToBase64String($bytes)

$headers = @{ Authorization = "Basic $token" }

try {
    $response = Invoke-RestMethod -Uri $scmUrl -Method POST -InFile $ZipPath -ContentType 'application/zip' -Headers $headers -TimeoutSec 1800
    Write-Host "Zip Deploy request accepted."; $response | Out-Null
}
catch {
    Write-Error "Zip Deploy failed: $($_.Exception.Message)"; exit 1
}

Write-Host "Deployment triggered. Check Deployment Center logs in Azure Portal if not live yet."
