using System.Runtime.InteropServices;

namespace MachinVisionProgram
{

public interface IMachinePerform {

    public bool OnOffSwitch {get; set;}

    public void TurnOn() {
    }
    public void TurnOff() {
    }
    public void PerformOperation();
}
}