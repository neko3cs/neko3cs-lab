function Get-HelloWorld {
  param (
    [string]$PersonName
  )
  
  return "Hello $PersonName"
}

function Add-One {
  param (
    [int]$Number
  )

  return $Number + 1
}

function Get-IsMale {
  param (
    [string]$PersonName
  )

  if ($PersonName.EndsWith("くん")) { 
    return $true
  }
  elseif ($PersonName.EndsWith("さん")) {
    return $false
  }
  else {
    throw "人名には'くん'か'さん'をつけましょう"
  }
}