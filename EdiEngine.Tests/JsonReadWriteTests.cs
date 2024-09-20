using EdiEngine.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using EdiEngine.Standards.X12_004010.Maps;
using M_940 = EdiEngine.Standards.X12_004010.Maps.M_940;
using EdiEngine.Tests.Maps;

namespace EdiEngine.Tests
{
    [TestClass]
    public class JsonReadWriteTests
    {
        [TestMethod]
        public void JsonReadWrite_JsonSerializationTest()
        {
            string contents = File.ReadAllText(@"C:\Users\Vinay\Downloads\EdiEngine-develop\EdiEngine-develop\EdiEngine.Tests\TestData\940.OK.edi");


            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.940.OK.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);
                EdiBatch b1 = r.FromString(contents);
                //Write Json using newtonsoft
                //check no exception
                JsonConvert.SerializeObject(b1);
                JsonConvert.SerializeObject(b1.Interchanges[0].Groups[0].Transactions[0]);

                //or use writer to write to string or stream
                JsonDataWriter w  = new JsonDataWriter();
                string str = w.WriteToString(b);
                Stream stream = w.WriteToStream(b);

                Assert.IsNotNull(str);

                Assert.IsNotNull(stream);
                Assert.AreEqual(0, stream.Position);
                Assert.IsTrue(stream.CanRead);

                Assert.AreEqual(str.Length, stream.Length);

                string edi =
                @"ISA*01*0000000000*01*0000000000*ZZ*ABCDEFGHIJKLMNO*ZZ*123456789012345*101127*1719*U*00400*000003438*0*P*>
                GS*OW*7705551212*3111350000*20000128*0557*3317*T*004010
                ST*940*0001
                W05*N*538686**001001*538686
                LX*1
                W01*12*CA*000100000010*VN*000100*UC*DEC0199******19991205
                G69*11.500 STRUD BLUBRY
                W76*56*500*LB*24*CF
                SE*7*0001
                GE*1*3317
                IEA*1*000003438";

                EdiDataReader rs = new EdiDataReader();
                EdiBatch bs = rs.FromString(edi);

                //Serialize the whole batch to JSON
                JsonDataWriter w1 = new JsonDataWriter();
                string json = w1.WriteToString(b);

                //OR Serialize selected EDI message to Json
                string jsonTrans = JsonConvert.SerializeObject(bs.Interchanges[0].Groups[0].Transactions[0]);

                //Serialize the whole batch to XML
                XmlDataWriter w2 = new XmlDataWriter();
                string xml = w2.WriteToString(b);

            }
        }

        [TestMethod]
        public void JsonReadWrite_DeserializeJsonOK()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.940.OK.json");

            M_940 map = new M_940();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(0, t.ValidationErrors.Count);
        }


        [TestMethod]
        public void JsonReadWrite_DeserializeJsonWithValidationErrors()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.940.ERR.json");

            M_940 map = new M_940();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(2, t.ValidationErrors.Count);
        }

        [TestMethod]
        public void JsonReadWrite_JsonSerializationHlLoopTest()
        {
            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.856.Crossdock.OK.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);

                JsonDataWriter jsonWriter = new JsonDataWriter();
                jsonWriter.WriteToString(b);
            }
        }

        [TestMethod]
        public void JsonReadWrite_DeserializeJsonHlLoopOk()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.856.Crossdock.OK.json");

            M_856 map = new M_856();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(0, t.ValidationErrors.Count);

            //string edi = TestUtils.WriteEdiEnvelope(t, "SH");
        }

        [TestMethod]
        public void JsonReadWrite_SerializeComposite()
        {
            using (Stream s = GetType().Assembly.GetManifestResourceStream("EdiEngine.Tests.TestData.850.Composite.SLN.OK.edi"))
            {
                EdiDataReader r = new EdiDataReader();
                EdiBatch b = r.FromStream(s);

                JsonDataWriter jsonWriter = new JsonDataWriter();
                jsonWriter.WriteToString(b);
            }
        }

        [TestMethod]
        public void JsonReadWrite_DeserializeComposite()
        {
            string json = TestUtils.ReadResourceStream("EdiEngine.Tests.TestData.001.Fake.Composite.json");

            M_001 map = new M_001();
            JsonMapReader r = new JsonMapReader(map);

            EdiTrans t = r.ReadToEnd(json);

            Assert.AreEqual(0, t.ValidationErrors.Count);

            var sln = (EdiSegment)t.Content.First();
            Assert.IsTrue(sln.Content[4] is EdiCompositeDataElement);
            Assert.AreEqual(6, ((EdiCompositeDataElement)sln.Content[4]).Content.Count);

        }
    }
}
