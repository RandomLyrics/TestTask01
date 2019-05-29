Get-Process `
| Where-Object {$_.company -Notlike ‘*Microsoft*’}`
| Format-Table ProcessName, Company -auto