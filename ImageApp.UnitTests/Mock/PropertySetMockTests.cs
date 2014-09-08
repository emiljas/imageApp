using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.UnitTests.Mock
{
    [TestClass]
    public class PropertySetMockTests
    {
        [TestMethod]
        public void ContainsKey_KeyNotExists_ReturnFalse()
        {
            var set = MakeEmptySet();
            Assert.IsFalse(set.ContainsKey("key"));
        }

        [TestMethod]
        public void ContainsKey_KeyExists_ReturnsTrue()
        {
            var set = MakeSetWithKey();
            Assert.IsTrue(set.ContainsKey("key"));
        }

        [TestMethod]
        public void Indexer_KeyNotExists_ReturnsNull()
        {
            var set = MakeEmptySet();
            Assert.IsNull(set["key"]);
        }

        [TestMethod]
        public void Indexer_KeyExists_ReturnValue()
        {
            var set = MakeSetWithKey();
            Assert.AreEqual("value", set["key"]);
        }

        private PropertySetMock MakeEmptySet()
        {
            return new PropertySetMock();
        }

        private PropertySetMock MakeSetWithKey()
        {
            var set = new PropertySetMock();
            set["key"] = "value";
            return set;
        }
    }
}
