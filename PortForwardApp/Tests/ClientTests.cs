using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortForward;
using PortForward.Utilities;

namespace PortForwardApp.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void ShouldEchoRequest()
        {
            TestData data = new TestData();

            Client clientA = new ExampleClient(data);
            Client clientB = new EchoClient();

            Bridge bridge = new Bridge(clientA, clientB);

            byte[] bytes = ByteStringConverter.GetBytes("Hello!");

            clientA.Push(bytes);

            Assert.AreEqual(bytes, data.Request);
            Assert.AreEqual(data.Response, data.Request);
            Assert.IsNotNull(data.Response);
        }
    }

    public class TestData
    {
        public byte[] Request { get; set; }
        public byte[] Response { get; set; }
    }

    public class ExampleClient : Client
    {
        private TestData _data;

        public ExampleClient(TestData data)
        {
            _data = data;
        }

        public override void Push(byte[] data)
        {
            _data.Request = data;
            base.Push(data);
        }

        public override void HandleResponse(byte[] data)
        {
            _data.Response = data;
        }
    }

}
