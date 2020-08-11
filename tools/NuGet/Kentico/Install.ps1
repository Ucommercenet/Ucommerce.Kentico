param($installPath, $toolsPath, $package, $project)

Write-Host "installPath:" "${installPath}"
Write-Host "toolsPath:" "${toolsPath}"

if ($project) {
	$dateTime = Get-Date -Format yyyyMMdd-HHmmss
	
	# Create paths and list them
	$projectPath = (Get-Item $project.Properties.Item("FullPath").Value).FullName.Trim("\")
	
	$projectName = Split-Path $projectPath -Leaf
	 Write-Host "Current working location:"
	 Write-Host "Ucommerce installing in:"
	 Write-Host "projectPath:" "$projectPath"
	 Write-Host "projectName:" "$projectName"#

	# Clean up old meta data files from Ucommerce modules
	Remove-Item $projectPath\App_Data\CMSModules\CMSInstallation\Packages\Ucommerce_* ;

	# Copy Ucommerce and Ucommerce_files from package to project folder
	$UcommerceFolderSource = Join-Path $installPath "UcommerceFiles"	
	robocopy "$UcommerceFolderSource" "$projectPath" /is /it /e /xf UI.xml 

	#Enable Kentico 12 compatibility app
	$cmsBase = $projectPath + "\bin\CMS.Core.dll"
	$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($cmsBase).FileVersion
	if ($version.StartsWith("12")) {
		$path = $projectPath  + "\CMSModules\Ucommerce\Apps\Kentico12Compatibility.disabled"
		$destination = $projectPath + "\CMSModules\Ucommerce\Apps\Kentico12Compatibility"

		if (Test-Path $path) {
			if(Test-Path $destination){
				Remove-Item -path $destination -Force -Recurse
			}
			Rename-Item -Path $path -NewName "Kentico12Compatibility" -Force
		}
	}

	$webConfigSource = Join-Path $projectPath "Web.config"
    $webConfig = New-Object XML
    $webConfig.Load($webConfigSource)

    $UcommerceInstallerModule = $webConfig.CreateElement('add')
    $UcommerceInstallerModule.SetAttribute('name', 'UcommerceInstallationModule') 
    $UcommerceInstallerModule.SetAttribute('type', 'Ucommerce.Kentico.Installer.App_Start.Installer, Ucommerce.Kentico.Installer')
  
    $UcommerceInstallerRemoveModule = $webConfig.CreateElement('remove')
    $UcommerceInstallerRemoveModule.SetAttribute('name', 'UcommerceInstallationModule') 

    $modules = $webConfig.SelectSingleNode("//system.webServer//modules")
    $modules.AppendChild($UcommerceInstallerRemoveModule);
    $modules.AppendChild($UcommerceInstallerModule);

	#Remove Castle.Windsor dependency in web.config to avoid conflicts doing upgrades when castle has been upgrade. 
	$perRequestLifestyleModulesElements = $webConfig.SelectNodes("//system.webServer//modules//add[@name='PerRequestLifestyle']")
	if($perRequestLifestyleModulesElements.Count -eq 1){
	    $modules.RemoveChild($perRequestLifestyleModulesElements[0])
	}

	$perRequestLifestyleHttpModulesElement = $webConfig.SelectNodes("//system.web//httpModules//add[@name='PerRequestLifestyle']")[0]
	if($perRequestLifestyleHttpModulesElement.Count -eq 1){
	    $webConfig.SelectSingleNode("//system.web//httpModules").RemoveChild($perRequestLifestyleHttpModulesElement)
	}
    
    $webConfig.Save($webConfigSource);		
}

# Set build action of all module resx files to "Content"
$project.ProjectItems | where-object {$_.Name -eq "CMSResources"} | 
    foreach-object { $_.ProjectItems } | where-object {$_.Name -eq $package.id} | 
    foreach-object { $_.ProjectItems } | where-object {$_.Name -like "*.resx"} | 
    foreach-object {$_.Properties} | where-object {$_.Name -eq "BuildAction"} | foreach-object {$_.Value = [int]2}