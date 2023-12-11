namespace MachinVisionProgram
{
public static class ObjectFactory {
    public static List<ICamera> CreateAllCameraList() {
        return new List<ICamera> {
            new CameraLane(),
            new Camera3D(),
            new CameraAlign()
        };
    }

    public static List<ICommunication> CreateAllCommunicationList() {
        return new List<ICommunication> {
            new CommunicationSerial("data", 9600),
            new CommunicationSerial("data", 9600),
            new CommunicationCCOM("COM Port1"), //Lane camera
            new CommunicationCCOM("COM Port2"), //3d camera
            new CommunicationCCOM("COM Port3"), //Align camera
        };
    }

        public static List<ILight> CreateAllLightList() {
        return new List<ILight> {
            new Light3DCameraController(),
            new LightCaptureController(),
            new LightLaneCameraController()
        };
    }

    public static List<IMachinePerform> CreateAllMachinePerformList() {
        return new List<IMachinePerform> {
            new MachineAlign(),
            new MachineFlipper(),
            new MachineLane()
        };
    }
}   
}