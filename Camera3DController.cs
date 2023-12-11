// If i expected working principle of 3d camera, as i saw It has multiple camera at the same location 
// ( not it process direction, but widely at some point )
// So it means, when some point each camera will take a shot, so it can draw 3 dimension at some point of sample.
namespace MachinVisionProgram
{
class Camera3D : ICamera
{
        public Camera3D() {
        InitialSetting();
    }

    public string ImageUrl {get; set;} = "";
    public void InitialSetting() {}
    public void TakePicture() {}
    public void ChangeResolution(int width, int height) {
        Console.WriteLine("", width, height);
    }
    public void ChangeFPS(int fps) {
        Console.WriteLine("", fps);
    }
    public void ChangeQuality(int quality) {
        Console.WriteLine(quality);
    }
    public void ChangeFormat(string format){
        Console.WriteLine(format);
    }
    public void ChangeExposure(int exposure){
        Console.WriteLine(exposure);
    }
    public void ChangeShutterSpeed(int shutterSpeed){
        Console.WriteLine(shutterSpeed);
    }
}
}