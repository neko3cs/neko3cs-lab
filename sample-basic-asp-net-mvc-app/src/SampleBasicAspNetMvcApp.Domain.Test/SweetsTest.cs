using Xunit;

namespace SampleBasicAspNetMvcApp.Domain.Test;

public class SweetsTest
{
    public class ���O���擾����
    {
        [Fact]
        public void ���O���擾�o���邱��()
        {
            const string name = "�`�[�Y�P�[�L";

            var sweets = new Sweets(1, name, 0);

            Assert.Equal(name, sweets.Name);
        }
    }
}