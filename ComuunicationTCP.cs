using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace MachinVisionProgram
{
class CommunicationTCP : CommunicationBase, ICommunication, ISubject {
    private TCP? tcp;

    public CommunicationTCP() {}
    public CommunicationTCP(string Ipaddr, int port) {
        tcp = new TCP(Ipaddr, port);
        isPhysicallyConnected = true;
    }

    private List<IObserver> observers = new List<IObserver>();
    public string? Signal { get; private set; }

    public void Connection() {
        Console.WriteLine("Connection Success");

    }
    public void Disconnection() {}
    public void Send(string Data) {
        Console.WriteLine("Sending Data to Connected Macine: " + Data);
    }
    public void Receive(string signal) {
        this.Signal = signal;
        Notify();
    }

    public Task<byte[]> ReceiveImageAsync() {
        return Task.FromResult(new byte[0]);
    }

    public async Task<byte[]> ReceiveCameraDataAsync()
{
    // TCP 클라이언트가 이미 설정되었다고 가정합니다.
    // 이를 위해 TcpClient 객체가 필요합니다.
    TcpClient client = new TcpClient("192.168.0.100", 80); // 예제 IP와 포트
    await client.ConnectAsync("192.168.0.100", 80); // 실제 연결 설정

    // 네트워크 스트림 초기화
    using NetworkStream stream = client.GetStream();

    // 완료 신호를 받기 위한 버퍼 설정
    byte[] signalBuffer = new byte[1024];
    int bytesRead = await stream.ReadAsync(signalBuffer, 0, signalBuffer.Length);

    // 신호를 문자열로 변환
    string signal = Encoding.ASCII.GetString(signalBuffer, 0, bytesRead);

    // 여기서는 "COMPLETE"라는 신호를 기다린다고 가정합니다.
    if (signal.Trim() != "COMPLETE")
    {
        throw new InvalidOperationException("올바른 신호를 받지 못했습니다.");
    }

    // 이미지 데이터 수신
    // 이미지 크기 또는 데이터 크기에 대한 정보를 미리 알고 있어야 합니다.
    // 여기서는 이미지 데이터 크기를 1024*1024 바이트라고 가정합니다.
    byte[] imageData = new byte[1024 * 1024];
    MemoryStream ms = new MemoryStream();

    // 이미지 데이터 읽기
    bytesRead = 0;
    while ((bytesRead = await stream.ReadAsync(imageData, 0, imageData.Length)) > 0)
    {
        ms.Write(imageData, 0, bytesRead);
    }

    // TCP 연결 종료
    client.Close();

    return ms.ToArray();
}

    public void Attach(IObserver observer) {
        observers.Add(observer);
    }
    public void Detach(IObserver observer) {
        observers.Remove(observer);
    }
    public void Notify() {
        foreach (var observer in observers) {
            observer.Update(this);
        }
    }

}

class TCP {
    public TCP(string Ipaddr, int port) {
        Console.WriteLine($"{Ipaddr}:{port} opened");
    }
}
}