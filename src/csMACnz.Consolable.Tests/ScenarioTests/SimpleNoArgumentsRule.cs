﻿using System;
using Xunit;

namespace csMACnz.Consolable.Tests.ScenarioTests
{
    public class SimpleNoArgumentsRule
    {
        private IRule[] _rules;
        public SimpleNoArgumentsRule()
        {
            _rules = Array.Empty<IRule>();
        }
        
        [Fact]
        public void EmptyArguments_Passes()
        {
            //Simulate .Net args parsing
            var args = CLIArgsParser.Parse("");
            
            var values = CommandLineParser.Parse(_rules, args, error => Assert.False(true, "Errors Not Expected."));
            
            Assert.Empty(values);
        }

        [Theory]
        [InlineData("-b")]
        [InlineData("/b")]
        [InlineData("--bravo")]
        public void IncorrectArgument_ErrorResult(string arg)
        {
            //Simulate .Net args parsing
            var args = CLIArgsParser.Parse(arg);

            bool errorWasCalled = false;
            var values = CommandLineParser.Parse(
                _rules,
                args,
                errors =>
                {
                    errorWasCalled = true;
                    Assert.Collection(
                            errors,
                            e => Assert.Equal(ErrorType.UnknownArgument, e.ErrorType));
                });

            Assert.True(errorWasCalled);
        }
    }
}
