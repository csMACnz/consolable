using System.Collections.Generic;
using Xunit;

namespace csMACnz.Consolable.Tests
{
    public class RequiredArgumentTests
    {
        [Fact]
        public void Create_defaultMode_Successful()
        {
            var requiredArgument = new RequiredArgument('a', "alpha");
            Assert.NotNull(requiredArgument);
            Assert.Equal('a', requiredArgument.ShortName);
            Assert.Equal("alpha", requiredArgument.LongName);
            Assert.Equal(ArgumentMode.Flag, requiredArgument.ValueMode);
        }

        [Fact]
        public void Create_SingleValueMode_Successful()
        {
            var requiredArgument = new RequiredArgument('a', "alpha", ArgumentMode.SingleValue);
            Assert.Equal('a', requiredArgument.ShortName);
            Assert.Equal("alpha", requiredArgument.LongName);
            Assert.Equal(ArgumentMode.SingleValue, requiredArgument.ValueMode);
        }

        [Fact]
        public void Verify_NoArguments_NotProvided()
        {
            var requiredArgument = new RequiredArgument('a', "alpha");
            var args = new List<Argument>();
            var result = requiredArgument.Verify(args);

            Assert.Equal(VerifyMode.NotProvided, result);
        }
        
        [Fact]
        public void Verify_MatchingArgument_ExplicitlyProvided()
        {
            var requiredArgument = new RequiredArgument('a', "alpha");
            var args = new List<Argument>{new Argument{ShortName='a', LongName="alpha", ValueMode = ArgumentMode.Flag}};
            var result = requiredArgument.Verify(args);

            Assert.Equal(VerifyMode.ExplicitlyProvided, result);
        }
    }
}