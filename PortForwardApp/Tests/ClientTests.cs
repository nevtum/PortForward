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
            Bridge bridge = new Bridge();

            Client clientA = new ExampleClient(data, bridge.SocketA);
            Client clientB = new EchoClient(bridge.SocketB);

            byte[] bytes = ByteStringConverter.GetBytes("Hello!");

            Assert.AreNotEqual(bytes, data.Request);
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

        public ExampleClient(TestData data, Socket socket)
            : base(socket)
        {
            _data = data;
        }

        public override void Push(byte[] data)
        {
            _data.Request = data;
            base.Push(data);
        }

        protected override void HandleResponse(byte[] data)
        {
            _data.Response = data;
        }
    }

}
