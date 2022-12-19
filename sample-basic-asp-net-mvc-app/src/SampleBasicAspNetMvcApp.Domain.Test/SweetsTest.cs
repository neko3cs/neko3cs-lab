using Xunit;

namespace SampleBasicAspNetMvcApp.Domain.Test;

public class SweetsTest
{
    public class 名前を取得する
    {
        [Fact]
        public void 名前が取得出来ること()
        {
            const string name = "チーズケーキ";

            var sweets = new Sweets(1, name, 0);

            Assert.Equal(name, sweets.Name);
        }
    }
}