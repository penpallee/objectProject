namespace MachinVisionProgram
{
public interface ICamera {
    public string ImageUrl {get; set;}
    void InitialSetting();
    void TakePicture();
    void ChangeResolution(int width, int height);
    void ChangeFPS(int fps);
    void ChangeQuality(int quality);
    void ChangeFormat(string format);
    void ChangeExposure(int exposure);
    void ChangeShutterSpeed(int shutterSpeed);
}
}