using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var rt = target.DeserializeJsonString<TestData>();
            Assert.AreEqual(rt.st, obj.st);
            Assert.AreEqual(rt.it, obj.it);
            Assert.AreEqual(rt.bl, obj.bl);
            Assert.AreEqual(rt.dt, obj.dt);
        }

        [TestMethod]
        public void JTokenTest()
        {
            var jtoken = target.ParseToJToken();
            Assert.AreEqual(jtoken.GetString("st"), "hi");
            Assert.AreEqual(jtoken.GetInt32("it"), 100);
            Assert.AreEqual(jtoken.GetBoolean("bl"), true);
            Assert.AreEqual(jtoken.GetDateTime("dt"), obj.dt);

            jtoken = "{}".ParseToJToken();
            Assert.IsNull(jtoken.GetString("st"));
            Assert.IsNull(jtoken.GetInt32("it"));
            Assert.IsNull(jtoken.GetBoolean("bl"));
            Assert.IsNull(jtoken.GetDateTime("dt"));

            jtoken = "{}".ParseToJToken();
            CollectionAssert.AreEqual(jtoken.ToIntArray(), new int[0]);
            CollectionAssert.AreEqual(jtoken.GetIntArray(""), new int[0]);
            CollectionAssert.AreEqual(jtoken.ToStringArray(), new string[0]);
            CollectionAssert.AreEqual(jtoken.GetStringArray(""), new string[0]);
            CollectionAssert.AreEqual(jtoken.ToObjectArray(), new object[0]);
            CollectionAssert.AreEqual(jtoken.GetObjectArray(""), new object[0]);

            jtoken = "{\"sa\":[0,1,\"2\",3,4]}".ParseToJToken();
            var demoIntArray = new int[] { 0, 1, 2, 3, 4 };
            CollectionAssert.AreEqual(jtoken["sa"].ToIntArray(), demoIntArray);
            CollectionAssert.AreEqual(jtoken.GetIntArray("sa"), demoIntArray);
            var demoStringArray = new string[] { "0", "1", "2", "3", "4" };
            CollectionAssert.AreEqual(jtoken["sa"].ToStringArray(), demoStringArray);
            CollectionAssert.AreEqual(jtoken.GetStringArray("sa"), demoStringArray);
            var demoObjArray = new object[] { 0L, 1L, "2", 3L, 4L };
            CollectionAssert.AreEqual(jtoken["sa"].ToObjectArray(), demoObjArray);
            CollectionAssert.AreEqual(jtoken.GetObjectArray("sa"), demoObjArray);
        }
    }

}
