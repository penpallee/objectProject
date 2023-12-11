using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using MachinVisionProgram;

class Program : IObserver
{
    
    private static CommunicationTCP? ComWithPLC;
    
    private static string expectedSignal = "start";
    
    public static void Main(String[] args) {
        ComWithPLC = new CommunicationTCP("192.168.0.100", 80);
        // Machine들을 제어하는 컴퓨터와 연결합니다.
        
        ComWithPLC.Connection();

        var program = new Program();
        ComWithPLC.Attach(program);
        ComWithPLC.Receive("start");
        
    }

    // Observer를 통해 특정 TCP연결이 되어 있고 특정 Signal이 전달될때 StartVIsionProcess()를 실행한다.

    public async void Update(ISubject subject) {
        if (subject is CommunicationTCP TcpClient && TcpClient.Signal == expectedSignal) 
        {
            await StartVisionProcess();
        }
    }

    // 여기서부터 VisionProcess가 시작됩니다.

    static async Task StartVisionProcess() {

        // VisionProcess에 필요한 모든 Class들의 객체를 한번에 미리 생성합니다.
        
        var CameraLists = ObjectFactory.CreateAllCameraList();
        var LightLists = ObjectFactory.CreateAllLightList();
        var MachineLists = ObjectFactory.CreateAllMachinePerformList();
        var ComLists = ObjectFactory.CreateAllCommunicationList();


        // Align Light    - ComLists[0]
        // Lane Light     - ComLists[1]
        // Lane camera    - ComLists[2]
        // 3d camera      - ComLists[3]
        // Align camera   - ComLists[4]
        //생성된 통신 객체들을 각각 정의된 방법에 따라서 연결합니다.
        foreach (var Communication in ComLists)  {
            Console.WriteLine(Communication);
            Communication.Connection();
        }


        // 생성한 모든 객체들을 사용할수있게끔 On시키거나 초기설정이 필요한 기기들은 미리 설정해둔 초기설정을 통해서 세팅합니다.
        //LigntLists[0] = 3DCameraLight
        //LigntLists[1] = AlignLight
        //LigntLists[2] = LaneLight
        foreach (var LgihtObject in LightLists)  {
            Console.WriteLine(LgihtObject);
            LgihtObject.On();
        }

        //CameraLists[0] = LaneCamera
        //CameraLists[1] = 3DCamera
        //CameraLists[2] = AlignCamera
        foreach (var CameraObject in CameraLists)  {
            Console.WriteLine(CameraObject);
            CameraObject.InitialSetting();
        }


        //MahineLists[0] = AlignMachine
        //MahineLists[1] = FlipperMachine
        //MahineLists[2] = LaneMachine
        foreach (var MachineObject in MachineLists)  {
            Console.WriteLine(MachineObject);
            MachineObject.TurnOn();
        }

        // sample이 Align위치에 오면 카메라 찍기
        ComLists[4].Send("Align Camera Work");

        //Align카메라가 찍은 이미지를 AlignCameraCommunication을(ComLists[4])를 이용해서 PC로 전달해준다.
        // 다만 이 전달받을 이미지가 어느 타이밍에 전달될지 정해져 있지 않으므로 비동기 방식으로 처리할지 새로운 스레드로 처리할지 등 정해야 하는데
        // 내 생각에 이번에는 기다리는 동안 특별히 병렬적으로 처리되어야 하는 일이 없이 이미지가 와야 다음 프로세스가 진행이 가능한 상태이기 때문에 
        // 비동기적으로 처리하면 되지 않을까 생각한다.
        
        //비동기적으로 align카메라가 사진을 찍고 반환해주는 값을 받는다.
        await ComLists[4].ReceiveImageAsync();

        //결과로 받은 이미지를 align을 할 수 있도록 이미지 처리를 해주는 함수에 넘겨준다.
        // AlignedPosition에 align할 값이 저장된다.
        var AlignedPosition = await ImageProcess.GetAlignPosition("imageUrl");

        // AlignedPosition을 PLC Align 머신에 전달하여 Align할 수 있게끔 한다.
        if (ComWithPLC != null) ComWithPLC.Send(AlignedPosition.ToString());


        // 위 과정을 오차범위내까지 반복한다.
        
        // Align후에 Align머신으로부터 수행완료 신호를 받는다.
        // 신호를 받으면 Align이 제대로 되었는지 다시한번 같은 프로세스를 통해 확인한다. 만약
        // 만약 새로 계산한 AlignPosition값이 오차범위 내라면 다음 프로세스를 진행할 수 있게 신호를 전달한다.
        bool RepeatSignal = true;

        while (RepeatSignal) 
        {
            if ( AlignedPosition.Item1 < 0.5 || AlignedPosition.Item2 < 0.5)
            {
                Console.WriteLine("Align성공 오차범위 내에 존재");
                RepeatSignal = false;
            } else
            {
                if (ComWithPLC != null) ComWithPLC.Send("Align실패 repeat Align Process");
                else Console.WriteLine("ComWIthPLC is null");
            } 
            AlignedPosition = await ImageProcess.GetAlignPosition(CameraLists[1].ImageUrl);
        }

        // Align이 성공하면 RepeatSignal이 false가 되어 
        // PLC에 Lane을 가동하여 sample을 이동시킬수 있는 신호를 준다.
        if (ComWithPLC != null) ComWithPLC.Send("Align성공 LaneMachine작동");
        else Console.WriteLine("ComWIthPLC is null");

        //MahineLists[2] = LaneMachine
        // 머신 내부적으로는 이런 형태로 작동한다고 가정 MachineLists[2].PerformOperation();
        


        // Lane카메라는 어떤 신호를 기준으로 찍을까 생각하다가 카메라 내장기능으로 카메라 화면에 비추는 이미지가 순간적인 변화가 생기면
        // 스스로 신호를 주어 이미지에 순간적인변화가 없을때까지 찍고 찍힌 이미지를 전달해 줄 수 있다고 가정하고 LaneCommunication으로 해당 기능을 On하는 신호를
        // 준다고 가정했다.
        
        // Lane CameraCommunication 을 통해서 Lane카메라에 신호를 주어 대기하다가 물체가 보이는순간부터 보이지않을때까지 스스로 일정간격으로
        //사진을 찍어준다.
        ComLists[2].Send("wait sample on Lane Camera FOV, if on Camera take shot every second.");
        
        // 찍은 사진을 LaneCameraImage란 변수에 저장하고 ReceiveImageAsync함수를 통해 PC로 전달해줍니다.
        var LaneCameraImage = await ComLists[2].ReceiveImageAsync();
        //전달받은 이미지를 ImageProcess class를 사용해 정해진 검사를 할 수 있도록 해줍니다.
        ImageProcess.LaneImageCalculate(LaneCameraImage);

        // 위의 구문이 실행이 끝나면 3d카메라로 넘어 갑니다.

        // 3d카메라는 어떤 신호를 기준으로 사진을 찍게 될까 생각하다
        // Lane카메라와 붙어있으므로 Lane카메라 자동 사진찍기 기능이 끝나면 PC에 신호를 준다고 가정하고, 신호가 왔을때를 기준으로 
        // 3d카메라에 신호를 주면 3d카메라가 사진을 찍는 기능을 수행하게 한다. (다만 여기서 통신시간의 오차가 있을 수 있으므로 3d 카메라의 
        // FOV를 조금 여유를 두어 약간의 오차범위 내에서 사진이 찍혀도 sample을 검사하는데는 이상이 없다고 가정한다.)

        // Lane카메라로부터 신호를 받으면 아래 코드로 3d 카메라 작동을 하게 한다.

        ComLists[3].Send("3D Camera start");
        var Camera3DImage = await ComLists[3].ReceiveImageAsync();
        ImageProcess.DimensionCameraCalculate(Camera3DImage);


        bool Decision = ImageProcess.FinalDecision();
        // 이렇게 충주에서 봤던 전체 flow는 다시 뒤집어서 돌아가는것까지 했지만 위 과정을 역순으로 진행하면 되므로
        // 코드는 이렇게 해서 Vision Process가 완료되었다고 가정한다.
        if (Decision) ComWithPLC.Send("Vision Process Success");
        else ComWithPLC.Send("Vision Process Fail");

    } // VisionProcess

} // Program