using System;
using Xunit;

namespace Chat.Client.Library.Tests.TestData
{
    public class CryptographyHelperData : TheoryData<string, string>
    {
        public CryptographyHelperData()
        {
            var date = DateTime.Now;
            Add("P2ssw0rd!", "52EEB7C9152B99A843345EF76FFD6411");
            Add("How beautiful you are", "EB80AAFA5B98425B584A4F8B61503F48");
            Add("Agent 007", "CB01AFE8663B8E0F90E25FD1636C55C3");
        }
    }
}
