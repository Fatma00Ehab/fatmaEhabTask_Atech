using Xunit;
using fatmaEhabTask_Atech.Models;
using fatmaEhabTask_Atech.Repositories;

namespace AtechApiTests
{
    public class InMemoryCountryRepositoryTests
    {
        [Fact]
        public void CanBlockAndUnblockCountry()
        {
             
            var repo = new InMemoryCountryRepository();

             
            var blockResult = repo.BlockCountry("US", "United States");
            var duplicateBlock = repo.BlockCountry("US", "United States");
            var unblockResult = repo.UnblockCountry("US");
            var unblockAgain = repo.UnblockCountry("US");
 
            Assert.True(blockResult);            
            Assert.False(duplicateBlock);       
            Assert.True(unblockResult);          
            Assert.False(unblockAgain);          
        }
    }
}
