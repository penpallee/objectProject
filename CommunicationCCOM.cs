namespace MachinVisionProgram
{
class CommunicationCCOM : CommunicationBase, ICommunication {
    private CameraCommunication CCom;
    
    public CommunicationCCOM(string COMPORT) {
        CCom = new CameraCommunication(COMPORT);
        isPhysicallyConnected = true;
    }

    public void Connection() {}
    public void Disconnection() {}
    public void Send(string message) {
        Console.WriteLine("Sending message: " + message);
    }
    public void Receive(string signal) {}

    public async Task<byte[]> ReceiveImageAsync() {
        return await Task.FromResult(new byte[10]);
    }

}

//Camera와 데이터를 주고 받기 위한 통신방법을 임의로 만들어놓은 클래스
class CameraCommunication {
    private string ComPort;
    public CameraCommunication(string COMPORT) {
        ComPort = COMPORT;
    }
}
}