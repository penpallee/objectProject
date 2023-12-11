// about Lane Camera,
// I expect it will take a shot continueos regularly while the sample on the range of camera.
// and finally computer connected LaneCamera will calculate the images and draw one full image.
// public class LaneCamera : MonoBehaviour
// {
//     private void Update()
//     {
//         if (car.transform.position.z > transform.position.z + cameraRange)
//         {
//             if (cameraShot)
//             {
//                 cameraShot = false;
//                 camera.GetComponent<Camera>().Render();
//             }
//         }
//         else
//         {
//             cameraShot = true;
//         }
//     }
// } 
namespace MachinVisionProgram
{
public class CameraLane : ICamera
{
    public CameraLane() {
        InitialSetting();
    }
    public string ImageUrl {get; set;} = "";
    public void InitialSetting() {}
    public string GetImageUrl() {return ImageUrl;}
    public void TakePicture() {
        Console.WriteLine("Take Picture for Lane Camera");
    
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