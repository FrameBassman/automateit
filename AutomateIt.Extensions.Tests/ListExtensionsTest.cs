using System.Collections.Generic;
using AutomateIt.Extensions.Extensions;
using Xunit;

namespace AutomateIt.Extensions.Tests
{
    public class ListExtensionsTest
    {
        private class ClassA
        {
            public readonly string Field;

            public ClassA(string field)
            {
                this.Field = field;
            }

            public override bool Equals(object obj)
            {
                return obj is ClassA && (obj as ClassA).Field == this.Field;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        [Fact]
        public void RandomItemTest()
        {
            var list = new List<ClassA> { new ClassA("111"), new ClassA("222"), new ClassA("111") };
            var randomItem = list.RandomItem(new ClassA("111"));
            Assert.NotEqual("111", randomItem.Field);
        }
    }
}