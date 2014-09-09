﻿using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.UnitTests.Lib.NewtonsoftJson
{
    [TestClass]
    public class SerializeAndDeserializeTests
    {
        private const string Value = "qwerty";

        [TestMethod]
        public void ModelWithAutogeneratedProperty()
        {
            var model = new AutogeneratedPropertyModel();
            model.Value = Value;
            var result = SerializeAndDeserialize(model);
            Assert.AreEqual(Value, result.Value);
        }

        [TestMethod]
        public void ModelWithListProperty()
        {
            var model = new ListPropertyModel();
            model.List = new List<string>();
            model.List.Add(Value);
            var result = SerializeAndDeserialize(model);
            Assert.AreEqual(Value, result.List[0]);
        }

        public T SerializeAndDeserialize<T>(T model)
        {
            var serializedModel = JsonConvert.SerializeObject(model);
            var deserializedModel = JsonConvert.DeserializeObject<T>(serializedModel);
            return deserializedModel;
        }
    }
}