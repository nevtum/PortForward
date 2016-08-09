namespace PortForward.Builders
{
    public static class ClientBuilder
    {
        public static ISetSerialSettings UsingSerialClient(Socket socket)
        {
            return new SerialClientBuilder(socket);
        }

        public static ConsoleClientBuilder UsingConsoleClient(Socket socket)
        {
            return new ConsoleClientBuilder(socket);
        }
    }
}
