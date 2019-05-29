#Requires -Version 3.0

Param(
    [string] 
    <# [Parameter(Mandatory=$true)]  #>
    $ResourceGroupName = 'TEST_GROUP',

    [string[]] 
    [Parameter(ValueFromRemainingArguments=$true)]
    $ResourceTypes = @('Microsoft.Web/serverFarms', 'Microsoft.Web/sites')
)

<# Connect-AzureRmAccount #>

<# NOT WORKING TILL ver 6.0.0 #>
<# Get-AzureRmResource -ResourceGroupName $ResourceGroupName #>

$RS = Get-AzureRmResource -ODataQuery "`$filter=resourcegroup eq '$ResourceGroupName'"
foreach ($r in $RS) {
    if ($ResourceTypes.Contains($r.ResourceType)) {
        Remove-AzureRmResource -ResourceId $r.ResourceId
    }
}