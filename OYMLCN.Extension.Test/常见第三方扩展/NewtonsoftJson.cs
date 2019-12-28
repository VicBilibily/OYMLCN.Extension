using Microsoft.VisualStudio.TestTools.UnitTesting;
using OYMLCN.Extensions;

namespace OYMLCN.Extension.Test
{
    [TestClass]
    public class NewtonsoftJsonTest
    {
        class TestData
        {
            public string st { get; set; }
            public int? it { get; set; }
            public bool? bl { get; set; }
            public System.DateTime? dt { get; set; }
        }
        readonly TestData obj = new TestData()
        {
            st = "hi",
            it = 100,
            bl = true,
            dt = new System.DateTime(2017, 01, 22, 12, 25, 20)
        };
        const string target = "{\"st\":\"hi\",\"it\":100,\"bl\":true,\"dt\":\"2017-01-22T12:25:20\"}";

        [TestMethod]
        public void JsonTest()
        {
            Assert.AreEqual(obj.ToJsonString(), target);
            var rt = target.DeserializeJsonToObject<TestData>();
            Assert.AreEqual(rt.st, obj.st);
            Assert.AreEqual(rt.it, obj.it);
            Assert.AreEqual(rt.bl, obj.bl);
            Assert.AreEqual(rt.dt, obj.dt);
        }
    }
}
