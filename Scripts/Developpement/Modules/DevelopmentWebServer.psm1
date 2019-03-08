<#
  PowerShell DevelopmentWebServer Module
  This module contains a set of wrapper scripts that
  enable a user to configure HTTP.sys/IIS/IIS Express.
#>

Set-StrictMode -Version Latest

# -----
# Constants
# -----

# -----
# Exported Functions
# -----

<#
.SYNOPSIS
    Reserves the urls corresponding to given fqdn and ports.
#>
function Register-HttpsUrl {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, HelpMessage = "The fqdn part of the url")]
        [string]$fqdn
        ,
        [Parameter(Mandatory = $true, HelpMessage = "The port to use")]
        [Int32]$port
        ,
        [Parameter(HelpMessage = "Only undo the installation process?" )]
        [switch]$OnlyUndo
    )
    Process {
        Write-Verbose "Suppressing existing reservation for https://$($fqdn):$($port)/"
        netsh http delete urlacl url=https://$($fqdn):$($port)/
        if ($false -eq $OnlyUndo) {
            Write-Verbose "Adding reservation for https://$($fqdn):$($port)/ for current user"
            netsh http add urlacl url=https://$($fqdn):$($port)/ user=$env:UserDomain\$env:UserName
        }
    }
}

<#
.SYNOPSIS
    Uses the specified certificate to bind to 0.0.0.0 address with the specified ports
#>
function Use-CertificateForPort {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, HelpMessage = "The certificate thumbprint")]
        [string]$certificateThumbprint
        ,
        [Parameter(Mandatory = $true, HelpMessage = "The list of ports to use")]
        [Int32[]]$ports
        ,
        [Parameter(HelpMessage = "Only undo the installation process?" )]
        [switch]$OnlyUndo
    )
    Process {
        $appId = [guid]::NewGuid()
        ForEach ($port in $ports) {
            Write-Verbose "Deleting certificate association for 0.0.0.0:$($port)"
            netsh http delete sslcert ipport="0.0.0.0:$($port)"
            if ($false -eq $OnlyUndo) {
                Write-Verbose "Associating certificate to 0.0.0.0:$($port)"
                netsh http add sslcert ipport="0.0.0.0:$($port)" appid="{$appId}" certhash="$certificateThumbprint"
            }
        }
    }
}

<#
.SYNOPSIS
    Adds or modifies the site node in user applicationhost.config
#>
function Set-SiteInApplicationHostConfig {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, HelpMessage = "The project name")]
        [string]$projectName
        ,
        [Parameter(Mandatory = $true, HelpMessage = "The project path")]
        [string]$projectPath
        ,
        [Parameter(Mandatory = $true, HelpMessage = "The fqdn of the site")]
        [string]$fqdn
        ,
        [Parameter(Mandatory = $true, HelpMessage = "The port of the site")]
        [Int32]$port
        ,
        [Parameter(HelpMessage = "Only undo the installation process?" )]
        [switch]$OnlyUndo
    )
    Process {
        [string]$appConfigPath = "$([environment]::getfolderpath("mydocuments"))/iisexpress/config/applicationhost.config"
        [xml]$appConfigDoc = Get-Content -Path $appConfigPath
        $newId = 1
        $sites = $appConfigDoc.configuration.'system.applicationHost'.sites
        Set-StrictMode -Off
        if ($null -ne $sites.site) {
            $ids = $sites.site | Select-Object -ExpandProperty id
            $newId = ($ids | Measure-Object -Maximum  | Select-Object -ExpandProperty Maximum) + 1
        }
        Set-StrictMode -Version Latest
        $siteContent = Select-Xml -Xml $appConfigDoc -XPath "//configuration/system.applicationHost/sites/site[@name='$($projectName)']"
        if ($null -ne $siteContent) {
            $site = $siteContent.Node
            $site.ParentNode.RemoveChild($site)
            $newId = $site.id
        }
        if ($false -eq $OnlyUndo) {
            $newSite = [xml]@"
            <site name="_projectname_" id="_id_">
                <application path="/" applicationPool="Clr4IntegratedAppPool">
                    <virtualDirectory path="/" physicalPath="_projectpath_" />
                </application>
                <bindings>
                    <binding protocol="https" bindingInformation="*:_port_:_fqdn_" />
                </bindings>
            </site>
"@
            $newNode = $sites.OwnerDocument.ImportNode($newSite.DocumentElement, $true)
            $sites.InsertBefore($newNode, $sites.FirstChild) > $null
            $site = $sites.FirstChild
            $site.id = [string]$newId
            $site.name = [string]$projectName
            $site.application.virtualDirectory.physicalPath = [string]$projectPath
            $site.bindings.binding.protocol = "https"
            $site.bindings.binding.bindingInformation = "*:$($port):$($fqdn)"
        }
        $appConfigDoc.Save($appConfigPath)
    }
}

Export-ModuleMember -Function Register-HttpsUrl, Use-CertificateForPort, Set-SiteInApplicationHostConfig