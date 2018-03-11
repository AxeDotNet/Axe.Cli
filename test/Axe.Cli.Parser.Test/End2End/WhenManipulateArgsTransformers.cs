using System;
using System.Collections.Generic;
using Xunit;

namespace Axe.Cli.Parser.Test.End2End
{
    public class WhenManipulateArgsTransformers
    {
        class DummyTransformer : ValueTransformer
        {
            protected override IList<string> SplitArgument(string argument)
            {
                return null;
            }

            protected override object TransformSingleArgument(string argument)
            {
                return null;
            }
        }

        [Theory]
        [InlineData("my_transformer")]
        [InlineData("my_transformer_123")]
        [InlineData("_my_transformer_123")]
        [InlineData("_")]
        [InlineData("a")]
        public void should_register_transformer(string validName)
        {
            var expected = new DummyTransformer();

            ArgsTransformers.Register(validName, expected);
            ValueTransformer transformer = ArgsTransformers.Get(validName);

            Assert.Same(expected, transformer);
        }

        [Fact]
        public void should_replace_existed_transformer()
        {
            const string key = "transformer_key";
            var transformer1 = new DummyTransformer();
            var transformer2 = new DummyTransformer();

            ArgsTransformers.Register(key, transformer1);
            ValueTransformer originalTransformer = ArgsTransformers.Get(key);
            ArgsTransformers.Register(key, transformer2);
            ValueTransformer afterReplaced = ArgsTransformers.Get(key);

            Assert.NotSame(originalTransformer, afterReplaced);
            Assert.Same(transformer2, afterReplaced);
        }

        [Fact]
        public void should_throw_if_name_or_transformer_is_null()
        {
            Assert.Throws<ArgumentNullException>(
                () => ArgsTransformers.Register(null, new DummyTransformer()));
            Assert.Throws<ArgumentNullException>(
                () => ArgsTransformers.Register("name", null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("9")]
        [InlineData("()")]
        [InlineData("abc_123_?")]
        public void should_throw_if_name_is_not_obey_the_pattern(string invalidPattern)
        {
            Assert.Throws<ArgumentException>(
                () => ArgsTransformers.Register(invalidPattern, new DummyTransformer()));
        }

        [Fact]
        public void should_throw_if_name_is_null_when_get_transformer()
        {
            Assert.Throws<ArgumentNullException>(() => ArgsTransformers.Get(null));
        }

        [Fact]
        public void should_throw_if_key_to_transformer_does_not_exist()
        {
            Assert.Throws<ArgumentException>(() => ArgsTransformers.Get("not_exist"));
        }
    }
}