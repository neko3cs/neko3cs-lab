using FluentAssertions;

namespace xUnitFluentAssertions.Tests;

public class PersonFixture
{
    [Fact]
    public void Beの挙動を確認する_値型()
    {
        var message1 = "Hello, World!";
        var message2 = "Hello, World!";

        // 値型のため、通る
        message1.Should().Be(message2);
    }

    [Fact]
    public void Beの挙動を確認する_参照型()
    {
        var taro = new Person { Id = 1, FirstName = "Taro", LastName = "Tanaka", Age = 20 };
        var taroClone = new Person { Id = 1, FirstName = "Taro", LastName = "Tanaka", Age = 20 };

        // 参照型の場合、参照が異なるため、通らない
        taroClone.Should().Be(taro);
    }

    [Fact]
    public void BeSameAsの挙動を確認する_値型()
    {
        var message1 = "Hello, World!";
        var message2 = "Hello, World!";

        // BeSameAsは参照を厳格に比較するため、Be同様通らない
        message1.Should().BeSameAs(message2);
    }

    [Fact]
    public void BeSameAsの挙動を確認する_参照型()
    {
        var taro = new Person { Id = 1, FirstName = "Taro", LastName = "Tanaka", Age = 20 };
        var taroClone = new Person { Id = 1, FirstName = "Taro", LastName = "Tanaka", Age = 20 };

        // BeSameAsは参照を厳格に比較するため、Be同様通らない
        taroClone.Should().BeSameAs(taro);
    }

    [Fact]
    public void BeEquivalentToの挙動を確認する_値型()
    {
        var message1 = "Hello, World!";
        var message2 = "Hello, World!";

        // 値型のため、通る
        message1.Should().BeEquivalentTo(message2);
    }

    [Fact]
    public void BeEquivalentToの挙動を確認する_参照型()
    {
        var taro = new Person { Id = 1, FirstName = "Taro", LastName = "Tanaka", Age = 20 };
        var taroClone = new Person { Id = 1, FirstName = "Taro", LastName = "Tanaka", Age = 20 };

        // 参照は異なるが、設定された値が同じなため、通る
        taroClone.Should().BeEquivalentTo(taro);
    }
}
