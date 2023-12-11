using System.Data;

namespace MachinVisionProgram
{

class CommunicationBase {
    public bool isPhysicallyConnected {get; set;} = false;
}

public interface ICommunication {
    void Connection();
    void Disconnection();
    void Send(string Data);
    public void Receive(string signal);

    public Task<byte[]> ReceiveImageAsync();

}
}