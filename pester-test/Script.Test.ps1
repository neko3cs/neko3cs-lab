using Module ./Script.psm1

Describe "Get-HelloWorld" {
  Context "指定した人名に対して英語であいさつ文を返す" {
    It "「Hello John」が取得されること" {
      Get-HelloWorld -PersonName "John" | Should Be "Hello John"
    }
    It "「Hello 佐藤さん」が取得されること" {
      Get-HelloWorld -PersonName "佐藤さん" | Should Be "Hello 佐藤さん"
    }
    It "「Hello Михаил」が取得されること" {
      Get-HelloWorld -PersonName "Михаил" | Should Be "Hello Михаил"
    }
    It "戻り値が文字列であること" {
      Get-HelloWorld -PersonName "太郎" | Should BeOfType System.String
    }
  }
}

Describe "Add-One" {
  Context "指定した値に1足した値を返す" {
    It "2が返されること" {
      Add-One -Number 1 | Should Be 2
    }
    It "返される値が1ではないこと" {
      Add-One -Number 1 | Should Not Be 1
    }
  }
}

Describe "Get-IsMale" {
  Context "指定した人名が男性かどうかを返す" {
    It "男性であること" {
      Get-IsMale -PersonName "田中くん" | Should Be $true
    }
    It "女性であること" {
      Get-IsMale -PersonName "佐藤さん" | Should Be $false
    }
    It "例外エラーが返されること" {
      { Get-IsMale -PersonName "Mr. John" } | Should -Throw "人名には'くん'か'さん'をつけましょう" # FIXME: なんかうまく検知されない
    }
  }
}

Describe "その他" {
  Context "NULLか空文字、空配列をであることを検知する" {
    It "NULLであること" {
      $null | Should BeNullOrEmpty
    }
    It "空文字であること" {
      "" | Should BeNullOrEmpty
    }
    It "空配列であること" {
      @() | Should BeNullOrEmpty
    }
  }
}