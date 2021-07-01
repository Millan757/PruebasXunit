using Chat.Client.Library.Helpers;
using Chat.Client.Library.Tests.TestData;
using Xunit;

namespace Chat.Client.Library.Tests
{
    public class CryptographyHelperTests
    {
        [Theory]
        [ClassData(typeof(CryptographyHelperData))]
        public void CalculateHash_ShouldBeExpectedHash(string password, string expected)
        {
            //Arrange

            //Act
            var result = CryptographyHelper.CalculateHash(password);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
