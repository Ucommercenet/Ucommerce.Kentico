task InstallKenticoLocal -depends `
    Rebuild, `
    DeployKenticoLocal, `
    RenameDotDefaultConfigFilesUnderTarget, `
    PatchInstallationModuleToWebConfigKentico {

	$Ucommerce_dir = $script:hash["Ucommerce_dir"]
	# Any missing files and moving stuff around that normal "deployToLocal" doesn't handle
	$bin_dir = $script:hash.bin_dir = "$working_dir\CMS\bin"

    #copy local copies to bin/Ucommerce. We want to resemble NuGet as close as possible, so we'll move them to the bin folder here
    & robocopy "$bin_dir\Ucommerce" "$bin_dir\" *.dll /xf Newtonsoft.Json.dll /NFL /NDL
    & robocopy "$bin_dir\Ucommerce" "$bin_dir\" *.pdb /NFL /NDL

	Remove-Item "$bin_dir\Ucommerce" -Force -Recurse

	$base_dir = $script:hash["base_dir"]
	$kentico_dir = "$base_dir\src\Ucommerce.Kentico\"

	Copy-Item "$kentico_dir\CMSFormControls\" "$working_dir\CMS\" -Include *.ascx -Force -Recurse
	Copy-Item "$kentico_dir\CmsResources\" "$working_dir\CMS\" -Force -Recurse

    Copy-Item "$Ucommerce_dir\shell\app\constants\kentico.js" "$Ucommerce_dir\shell\app\constants.js" -Force

    Copy-Item "$Ucommerce_dir\Css\Kentico\Kentico10\UcommerceInKentico.css" "$working_dir\CMS\App_Themes\Default\UcommerceInKentico.css"

	#Create folder structor
    $moduleDataPath = $working_dir + "\CMS\App_Data\CMSModules\Ucommerce\Install\" + "Ucommerce_"  +"$Script:version"
    New-Item $moduleDataPath -type directory

    #copy data for module
    $moduleDataSource = $base_dir + "\src\Ucommerce.Kentico.Installer\UcommerceModule\*"
    Copy-Item -Path $moduleDataSource -Destination $moduleDataPath -recurse

    #Update version number in cms_resource.xml
	$cmsResourceXmlPath = $moduleDataPath +"\Data\Objects\cms_resource.xml.export"
    $cmsResource = (Get-Content $cmsResourceXmlPath)
    $cmsResourceAsXml = [xml]$cmsResource
    $cmsResourceAsXml.cms_resource.NewDataSet.cms_resource.ResourceVersion = "$Script:version"
    write-host $cmsResourceAsXml.cms_resource.NewDataSet.cms_resource.ResourceVersion
    $stream = [System.IO.StreamWriter] $cmsResourceXmlPath
    $stream.WriteLine($cmsResourceAsXml.OuterXml)
    $stream.close()

    #zip
    $newFileName = $moduleDataPath+".zip"
    $packageZipFullName = $moduleDataPath+"\data"

    Exec { Invoke-Expression "& '$base_dir\tools\7zip\7z.exe' a -tZip -mx9 '$newFileName' '$packageZipFullName'" }

    #Delete the unzip folde and content
    Remove-Item $moduleDataPath\..\* -recurse -exclude *.zip

    #Create meta data folder

    $metaDataPath = $working_dir + "\CMS\App_Data\CMSModules\CMSInstallation\Packages"

	if(Test-Path $metaDataPath)
    {
	}
	else {
		New-Item $metaDataPath -type directory
	}

}

task PatchInstallationModuleToWebConfigKentico {
	$basedir = $script:hash.base_dir;
	$webConfigSource = Join-Path "$working_dir\CMS" "Web.config"
	Push-Location "$basedir\tools\WebconfigTransformer\"

	$binDir = $script:hash["bin_dir"]
	$cmsBase = "$binDir\CMS.Base.dll"
	$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($cmsBase).FileVersion
	$Ucommerce_dir = $script:hash["Ucommerce_dir"]

	if ($version.StartsWith("10")) {
		Write-Host "Applying transformations for Kentico 10"
		.\WebconfigTransformer.exe $webConfigSource "$basedir\tools\deploy\powershellinstall\webconfigtransformations\Kentico\10"
	}
	if ($version.StartsWith("11")) {
		Write-Host "Applying transformations for Kentico 11"

		.\WebconfigTransformer.exe $webConfigSource "$basedir\tools\deploy\powershellinstall\webconfigtransformations\Kentico\11"
	}
	if ($version.StartsWith("12")) {
		Write-Host "Applying transformations for Kentico 12"
		.\WebconfigTransformer.exe $webConfigSource "$basedir\tools\deploy\powershellinstall\webconfigtransformations\Kentico\12"
	}

	Pop-Location
}

task RenameKenticoCompatibilityFolderBasedOnVersion {
	$binDir = $script:hash["bin_dir"]
	$cmsBase = "$binDir\CMS.Base.dll"

	if (Test-Path $cmsBase) {
		$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($cmsBase).FileVersion
		$Ucommerce_dir = $script:hash["Ucommerce_dir"]
		$appsFolder = "$Ucommerce_dir\apps"

		if ($version.StartsWith("12")) {
			if(Test-Path "$appsFolder\Kentico12Compatibility"){
				Remove-Item "$appsFolder\Kentico12Compatibility" -Force -Recurse
			}
			Move-Item -Force -Path "$appsFolder\Kentico12Compatibility.disabled" -Destination "$appsFolder\Kentico12Compatibility"
		}
	}
}

task DeployKenticoLocal -depends `
    SetKenticoVars, `
    CopyKenticoFiles, `
    PostDeployMoveBinFiles, `
    RemoveResourceFoldersFromUcommerceFolderWhenDeployToLocal, `
    DeployPaymentProviderAppsToLocal, `
    DeploySanitizationApps, `
    EnableSanitizationApps, `
    DeployExchangeRatesAPICurrencyConversionApps, `
    EnableExchangeRatesAPICurrencyConversionApps, `
    DeployKenticoCompatibilityApps, `
    RenameKenticoCompatibilityFolderBasedOnVersion {
}

task CreateKenticoPackage -depends `
    ValidateSetup, `
    CleanKenticoWorkingDirectory, `
    Rebuild, `
    CopyKenticoFiles, `
    CleanKenticoPackageForOtherCmsDependencies, `
    DeployPaymentProviderAppsToPackage, `
    DeploySanitizationApps, `
    EnableSanitizationApps, `
    DeployExchangeRatesAPICurrencyConversionApps, `
    EnableExchangeRatesAPICurrencyConversionApps, `
    MoveClientResourcesToMatchShellAndKenticoApp, `
    RenameDotDefaultConfigFilesForKentico, `
    DeployKenticoCompatibilityApps, `
    CreateKenticoNugetPackage {
}

task DeployKenticoCompatibilityApps -description "Deploy all Kentico compatibility apps 1-to-1 to the Apps folder"{
	$Ucommerce_dir = $script:hash["Ucommerce_dir"]
	$appsFolder = "$Ucommerce_dir\Apps"

	# Copy apps from respective projects
	Copy-Item -Path "$src\Ucommerce.Kentico12\Kentico12Compatibility.disabled" -Destination $appsFolder -Force -recurse

	# Create bin folder for all apps.
	$kentico12CompatibilityFolder = "$appsFolder\Kentico12Compatibility.disabled";

	New-Item "$kentico12CompatibilityFolder\bin" -Type Directory -Force

	# Copy assemblies in the respective app folders
	Copy-Item -Path "$src\Ucommerce.Kentico12\bin\$configuration\Ucommerce.Kentico12.dll" -Destination "$kentico12CompatibilityFolder\bin" -Force
}



task SetKenticoVars -description "Since path are different from Deploy.To.Local or Deploy.To.Package, we need to set them differently." {
    if($CreatePackage)
    {
        $script:hash.Ucommerce_dir = "$working_dir\UcommerceFiles\CMSModules\Ucommerce"
        $script:hash.bin_dir = "$working_dir\lib"
        $script:hash.lib_dir = "$working_dir\lib"
        $script:hash.files_root_dir = "$working_dir\content"
        $script:hash.base_dir = "$src\.."
        $script:hash.module_dir = "$src\..\tools\NuGet\Kentico\content\App_Data\CMSModules"
    }
    else
    {
        $script:hash.Ucommerce_dir = "$working_dir\CMS\CMSModules\Ucommerce"
        $script:hash.bin_dir = "$working_dir\CMS\bin"
        $script:hash.files_root_dir = "$working_dir\CMS"
		$script:hash.base_dir = "$src\.."
    }
}

task CopyKenticoFiles `
    -description "Copy all the Kentico files needed for deployment" `
    -depends `
        CopyUcommerceFiles, `
        RemoveInstallationBinariesNotNeededForNugetPackage, `
        RemoveUmbraco7CssFilesAndResources, `
        RemoveUmbraco6CssFilesAndResources {

    $Ucommerce_dir = $script:hash["Ucommerce_dir"]
    $bin_dir = $script:hash["bin_dir"]
    $files_root = $script:hash["files_root_dir"]
    $base_dir = $script:hash["base_dir"]
    $lib_dir = $script:hash["lib_dir"]

    # Installer files ... needs to be copied from the Installer Project since it's not referenced in the Main
    $UcommerceKenticoInstallerBins = @("Ucommerce.Kentico.Installer.dll", "Microsoft.Web.XmlTransform.dll")
    if($CreatePackage -eq $False)
    {
        $UcommerceKenticoInstallerBins += "Ucommerce.Installer.pdb"
        $UcommerceKenticoInstallerBins += "Ucommerce.Kentico.Installer.pdb"
		CopyFiles "$src\Ucommerce.Kentico.Installer\bin\$configuration\" "$bin_dir" "Ucommerce.Kentico.Installer.dll"
		CopyFiles "$src\Ucommerce.Kentico.Installer\bin\$configuration\" "$bin_dir" "Ucommerce.Kentico.Installer.pdb"

    }

    if($CreatePackage -eq $true)
    {
        CopyFiles "$src\Ucommerce.Kentico.Installer\bin\$configuration\" "$lib_dir" $UcommerceKenticoInstallerBins
    }

	& robocopy "$src\Ucommerce.Kentico\Configuration" "$Ucommerce_dir\Configuration" *.* /E /NFL /NDL

    CopyFiles "$src\Ucommerce.Kentico.Installer\ConfigurationTransformations" "$Ucommerce_dir\install" @("NhibernateLogging.config", "ClientDependencyKentico.config", "ExtensionlessUrlHandler.config")


    & robocopy "$src\Ucommerce.Web.Shell\App" "$Ucommerce_dir\shell\App" /E /NFL /NDL
    & robocopy "$src\Ucommerce.Web.Shell\Masterpages" "$Ucommerce_dir\shell\Masterpages" /E /NFL /NDL
    & robocopy "$src\Ucommerce.Web.Shell\Applications" "$Ucommerce_dir\shell\Applications" /E /NFL /NDL
    & robocopy "$src\Ucommerce.Web.Shell\Scripts" "$Ucommerce_dir\shell\Scripts" /E /NFL /NDL
    & robocopy "$src\Ucommerce.Web.Shell\Content" "$Ucommerce_dir\shell\Content" /E /NFL /NDL
    & robocopy "$src\Ucommerce.Web.Shell" "$Ucommerce_dir\shell" index.html /NFL /NDL
    & robocopy "$src\Ucommerce.Web.Shell" "$Ucommerce_dir\shell" index.aspx /NFL /NDL

	& robocopy "$src\Ucommerce.Kentico\CMSPages" "$Ucommerce_dir\shell" OrderManager.aspx /NFL /NDL
    & robocopy "$src\Ucommerce.Kentico\CMSPages" "$Ucommerce_dir\shell" CatalogManager.aspx /NFL /NDL
    & robocopy "$src\Ucommerce.Kentico\CMSPages" "$Ucommerce_dir\shell" PromotionManager.aspx /NFL /NDL
    & robocopy "$src\Ucommerce.Kentico\CMSPages" "$Ucommerce_dir\shell" SettingsManager.aspx /NFL /NDL
	& robocopy "$src\Ucommerce.Kentico\CMSPages" "$Ucommerce_dir\shell" DashboardWrapper.aspx /NFL /NDL

    if($CreatePackage)
    {
        & robocopy "$src\UcommerceWeb\App_GlobalResources" "$working_dir\UcommerceFiles\App_GlobalResources" *.resx /NFL /NDL
    }
    else
    {
        & robocopy "$src\UcommerceWeb\App_GlobalResources" "$files_root\App_GlobalResources" *.resx /NFL /NDL
    }

	# Copy custom activity resource strings.
    if($CreatePackage)
    {
        & robocopy "$src\Ucommerce.Kentico\bin\$configuration\CMSResources\Ucommerce" "$working_dir\UcommerceFiles\CMSResources\Ucommerce" *.resx /NFL /NDL
    }
    else
    {
        & robocopy "$src\Ucommerce.Kentico\bin\$configuration\CMSResources\Ucommerce" "$files_root\CMSResources\Ucommerce" *.resx /NFL /NDL
    }

	# Copy form control user control.
    if($CreatePackage)
    {
        & robocopy "$src\Ucommerce.Kentico\CMSFormControls\Selectors" "$working_dir\UcommerceFiles\CMSFormControls\Selectors" *.ascx /NFL /NDL
    }
    else
    {
        & robocopy "$src\Ucommerce.Kentico\CMSFormControls\Selectors" "$files_root\CMSFormControls\Selectors" *.ascx /NFL /NDL
    }

    # Copy Kentico specific assemblies
    $UcommerceKenticoBins = @("Ucommerce.Kentico.dll");
    if($CreatePackage -eq $False)
    {
        $UcommerceKenticoBins += "Ucommerce.Kentico.pdb"
    }

    CopyFiles "$src\Ucommerce.Kentico\bin\$configuration" "$bin_dir" $UcommerceKenticoBins

    # Copy Shell assembly
    $UcommerceWebShellBins = @("Ucommerce.Web.Shell.dll");
    if($CreatePackage -eq $False)
    {
        $UcommerceWebShellBins += "Ucommerce.Web.Shell.pdb"
    }

    CopyFiles "$src\Ucommerce.Web.Shell\bin" "$bin_dir" $UcommerceWebShellBins

    # Copy database SQL files
    Copy-Item "$src\..\database\UcommerceDB.*.sql" "$Ucommerce_dir\install"
    Copy-Item "$src\..\database\Kentico.*.sql" "$Ucommerce_dir\install"

	# SHELL configuration file.
    # This could be done better if we were allowed to move the configuration files.
    RemoveFiles "$Ucommerce_dir\Configuration" @("Shell.Umbraco4.config.default", "Shell.umbraco5.config.default", "Shell.Umbraco7.config.default", "Shell.Sitecore.config.default")
    RemoveFiles "$Ucommerce_dir\Configuration\Settings" @("Settings.Umbraco6.config.default", "Settings.umbraco7.config.default", "Settings.Sitecore.config.default")
    RemoveFiles "$Ucommerce_dir\install" @("UmbracoAddHttpModules.config", "Ucommerce.dependencies.umbraco.config", "UcommerceConnectionString.UaaS.config", "Ucommerce.dependencies.sitecore.config", "MergeUmbracoDashboard.config", "MergeUmbracoAppSettings.config", "MergeUmbraco6Dashboard.config", "ClientDependency.Umbraco.config")
    $configFile = "$Ucommerce_dir\Configuration\Kentico.config.default"
    if(Test-Path $configFile)
    {
        Move-Item "$configFile" "$Ucommerce_dir\Configuration\Shell.config.default" -Force
    }
    else
    {
        throw "Could not find default configuration file: $configFile"
    }

    $settingsFile = "$Ucommerce_dir\Configuration\Settings\Settings.Kentico.config.default"
    if(Test-Path $settingsFile)
    {
        Move-Item "$settingsFile" "$Ucommerce_dir\Configuration\Settings\Settings.config.default" -Force
    }
    else
    {
        throw "Could not find default settings file: $settingsFile"
    }

    #Remove Umbraco5 css
    Remove-Item -Recurse "$Ucommerce_dir\Css\Umbraco5" -Force

    #Remove Umbraco 4 and 5 JS files
    Remove-Item -Recurse "$Ucommerce_dir\Scripts\Umbraco5\" -Force
    Remove-Item "$Ucommerce_dir\Scripts\Ucommerce4_1.js" -Force

	# Move Sub-Module configuration files into Apps/Ucommerce Sections
	$sectionsFolder = "$Ucommerce_dir\Apps\Ucommerce Sections"
	$kenticoFolder = "$Ucommerce_dir\Apps\Kentico"


	if((Test-Path $sectionsFolder) -and (-not (Test-Path $kenticoFolder)))
	{
		Rename-Item $sectionsFolder "Kentico"
	}
	ElseIf(Test-Path $sectionsFolder)
	{
		Copy-Item $sectionsFolder\* $kenticoFolder
		Remove-Item $sectionsFolder -Force -Recurse
	}
}

task MoveClientResourcesToMatchShellAndKenticoApp {

    $Ucommerce_dir = $script:hash["Ucommerce_dir"]

    Move-Item "$Ucommerce_dir\shell\app\constants\kentico.js" "$Ucommerce_dir\shell\app\constants.js" -Force

    Move-Item "$Ucommerce_dir\Css\Kentico\Kentico10\UcommerceInKentico.css" "$working_dir\UcommerceFiles\App_Themes\Default\UcommerceInKentico.css"
}

task CleanKenticoWorkingDirectory `
    -description "Cleans the kentico working directory. This should NOT be used when using Deploy.To.Local" `
    -depends SetKenticoVars {

    # Create directories
    if(Test-Path $working_dir)
    {
        Remove-Item -Recurse "$working_dir\*" -Force
    }
    else
    {
        New-Item "$working_dir" -Force -ItemType Directory
    }

    New-Item "$working_dir\lib" -Force -ItemType Directory
    New-Item "$working_dir\UcommerceFiles\CMSModules\Ucommerce\install" -Force -ItemType Directory
    New-Item "$working_dir\UcommerceFiles\CMSModules\Ucommerce\shell" -Force -ItemType Directory
    New-Item "$working_dir\UcommerceFiles\App_Themes\Default" -Force -ItemType Directory
}

task CreateKenticoNugetPackage `
    -description "Creates the Kentico Nuget package" `
    -depends `
        CreateKenticoNuspecFile, `
        UpdateKenticoNuspecFile, `
        CopyKenticoInstallationScriptToTools, `
        UpdateUcommerceModule {

    $nuspecFilePath = "$working_dir\Ucommerce.Kentico.nuspec"
    $command = "$base_dir\tools\nuget\nuget.exe pack '$nuspecFilePath' -OutputDirectory c:\TMP"
	Invoke-Expression $command
}

task CreateKenticoNuspecFile {
    $NuspecFile = "$base_dir\tools\nuget\Kentico\Ucommerce.Kentico.nuspec"
    $ReadmeFile = "$base_dir\tools\nuget\Kentico\Ucommerce.Kentico.Readme.txt"

    Copy-Item -Path $NuspecFile -Destination $working_dir
    Copy-Item -Path $ReadmeFile -Destination "$working_dir\Readme.txt"

}

task UpdateKenticoNuspecFile {
    $NuspecFilePath = "$working_dir\Ucommerce.Kentico.nuspec"
    $NuspecFileText = get-content $NuspecFilePath
    $NuspecFileText = $NuspecFileText -replace "%versionNumberToReplace%","$Script:version"
    $NuspecFileText > $NuspecFilePath
}

task CopyKenticoInstallationScriptToTools {
  $toolsFolder = $working_dir + "\tools\";
  $installScriptFilePath = $base_dir + "\tools\nuget\Kentico\install.ps1"
  New-Item $toolsFolder -Type Directory
  Copy-Item -Path $installScriptFilePath -Destination $toolsFolder -Recurse -Confirm:$false -Exclude "bin","*.dll"
}

task CopyKenticoContentFolder {
    $contentFolder = "$base_dir\tools\nuget\Kentico\content"
    Copy-Item "$contentFolder" "$working_dir" -Recurse
}

task UpdateUcommerceModule -description "Updates the Ucommerce module package to ensure it will be imported to by Kentico" {
    #Create folder structor
    $moduleDataPath = $working_dir + "\UcommerceFiles\App_Data\CMSModules\Ucommerce\Install\" + "Ucommerce_"  +"$Script:version"
    New-Item $moduleDataPath -type directory

    #copy data for module
    $moduleDataSource = $base_dir + "\src\Ucommerce.Kentico.Installer\UcommerceModule\*"
    Copy-Item -Path $moduleDataSource -Destination $moduleDataPath -recurse

    #Update version number in cms_resource.xml
	$cmsResourceXmlPath = $moduleDataPath +"\Data\Objects\cms_resource.xml.export"
    $cmsResource = (Get-Content $cmsResourceXmlPath)
    $cmsResourceAsXml = [xml]$cmsResource
    $cmsResourceAsXml.cms_resource.NewDataSet.cms_resource.ResourceVersion = "$Script:version"
    write-host $cmsResourceAsXml.cms_resource.NewDataSet.cms_resource.ResourceVersion
    $stream = [System.IO.StreamWriter] $cmsResourceXmlPath
    $stream.WriteLine($cmsResourceAsXml.OuterXml)
    $stream.close()

    #zip
    $newFileName = $moduleDataPath+".zip"
    $packageZipFullName = $moduleDataPath+"\data"

    Exec { Invoke-Expression "& '$base_dir\tools\7zip\7z.exe' a -tZip -mx9 '$newFileName' '$packageZipFullName'" }

    #Delete the unzip folde and content
    Remove-Item $moduleDataPath\..\* -recurse -exclude *.zip

    #Create meta data folder
    $metaDataPath = $working_dir + "\content\App_Data\CMSModules\CMSInstallation\Packages"


    if($CreatePackage -eq $true)
    {
        $metaDataPath = $working_dir + "\UcommerceFiles\App_Data\CMSModules\CMSInstallation\Packages"
    }

    New-Item $metaDataPath -type directory

    #Create meta data file
    [xml]$moduleMetaDataXml = New-Object System.Xml.XmlDocument
    $dec = $moduleMetaDataXml.CreateXmlDeclaration("1.0",$null,$null)
    $moduleMetaDataXml.AppendChild($dec)
    $root = $moduleMetaDataXml.CreateNode("element","moduleInstallationMetaData",$null)
    $root.SetAttribute("xmlns:xsd","http://www.w3.org/2001/XMLSchema")
    $root.SetAttribute("xmlns:xsi","http://www.w3.org/2001/XMLSchema-instance")
    $moduleMetaDataXml.AppendChild($root)
    $nameNode = $moduleMetaDataXml.CreateNode("element","name",$null)
    $nameNode.InnerText = "Ucommerce"
    $versionNode = $moduleMetaDataXml.CreateNode("element","version",$null)
    $versionNode.InnerText = "$Script:version"
    $root.AppendChild($nameNode)
    $root.AppendChild($versionNode)
    $metaDataFilePath = "$metaDataPath\Ucommerce_"+"$Script:version" + ".xml"
    $moduleMetaDataXml.save($metaDataFilePath)
}

task RenameDotDefaultConfigFilesForKentico {
    Get-ChildItem $working_dir -include "*.config.default" -Exclude "*custom.config.default" -Recurse | Rename-Item -NewName {$_.name -replace ".default","" }
}


task CleanKenticoPackageForOtherCmsDependencies {
	$items = Get-ChildItem $working_dir -Recurse | Where-Object { $_.FullName.ToLower().Contains("sitecore") -or $_.FullName.ToLower().Contains("sitefinity") -or $_.FullName.ToLower().Contains("umbraco")}

	foreach($item in $items) {
		if (Test-Path $item.FullName) {
			Write-Host "Removing " + $item.FullName
			Remove-Item $item.FullName -Force -Recurse
		}
        else {
            Write-Host "test-path failed for " + $item.FullName
        }
	}
}