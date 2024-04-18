using System.Net;
using System.Net.Sockets;
using System.Text;

public class NetworkClass
{
    public IPAddress localAdd { get; set; }


    public NetworkClass()
    {

    }

    public async Task<Response> sending(string host, int port, List<string> message)
    {
        Response res = new Response();

        try
        {
            Stream stream = null;
            byte[] bynaryData = null;

            TcpClient tcpClnt = new TcpClient();
            await tcpClnt.ConnectAsync(host, port);
            stream = tcpClnt.GetStream();

            if (stream.CanWrite)
            {
                foreach (string item in message)
                {
                    bynaryData = Encoding.ASCII.GetBytes(item + "\n");
                    if (stream != null)
                    {
                        stream.Write(bynaryData, 0, bynaryData.Length);
                    }
                }
            }

            tcpClnt.Close();
            tcpClnt = null;
        }
        catch (Exception e)
        {
            Console.WriteLine("There was somethign wrong  with the connection: " + e.Message);

            res.status = "error";
            res.message = e.Message;
        }

        return await Task.FromResult(res);
    }

    public void init(ref TcpListener listener, string host, int port)
    {
        try
        {
            localAdd = IPAddress.Parse(host);
            listener = new TcpListener(localAdd, port);
            listener.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine($"There was somethign wrong:  {e}");
        }
    }

    public void Close(TcpListener listener)
    {
        if (listener != null)
        {
            listener.Stop();
        }
    }

    public Response testServer(string host, int port)
    {
        Response res = new Response();

        try
        {
            IPAddress localAdd = IPAddress.Parse(host);
            TcpListener listener = new TcpListener(localAdd, port);

            listener.Start();
            listener.Start();

            res.status = "ok";
        }
        catch (Exception e)
        {
            res.status = "error";
            res.message = e.Message;
        }

        return res;
    }

    public Response test(string host, int port)
    {
        Response res = new Response();

        try
        {
            TcpClient tcpClnt = new TcpClient();
            tcpClnt.Connect(host, port);
            tcpClnt.Close();

            res.status = "ok";
        }
        catch (Exception e)
        {
            res.status = "error";
            res.message = e.Message;
        }
        return res;
    }

}