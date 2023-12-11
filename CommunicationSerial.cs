namespace MachinVisionProgram
{

    class CommunicationSerial : CommunicationBase, ICommunication {
    SerialPort? serialPort;
    
    public CommunicationSerial(string portName, int baudRate) {
        serialPort = new SerialPort(portName, baudRate);
    }
    public CommunicationSerial() {
        isPhysicallyConnected = true;
    }

    public void Connection() {}
    public void Disconnection() {}
    public void Send(string message) {
        Console.WriteLine("Serial message: " + message);
    }
    public void Receive(string signal) {}
    public Task<byte[]> ReceiveImageAsync() {
        return Task.FromResult(new byte[0]);
    }

}

class SerialPort {
    public SerialPort(string portName, int baudRate) {
        Console.WriteLine("Serial port created: " + portName + " " + baudRate);
    }
    }
}