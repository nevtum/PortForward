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

            clientA.Transmit(bytes);

            Assert.AreEqual(data.Request, bytes);
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

        public override void Transmit(byte[] message)
        {
            _data.Request = message;
            base.Transmit(message);
        }

        public override void HandleRx(object sender, EventArgs e)
        {
            _data.Response = (byte[])sender;
        }
    }

}
