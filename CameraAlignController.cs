namespace MachinVisionProgram
{
public class CameraAlign : ICamera
{
    public CameraAlign() {
        InitialSetting();
    }

    public string ImageUrl {get; set;} = "";
    public void InitialSetting() {}
    public void TakePicture() {
        Console.WriteLine("Take Picture");
        ImageUrl = "Proper Align imagePath";
    }
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