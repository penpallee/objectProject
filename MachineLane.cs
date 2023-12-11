namespace MachinVisionProgram
{
class MachineLane: IMachinePerform {
    public bool OnOffSwitch {get; set;}

    public void TurnOn() {
    }
    public void TurnOff() {
    }
    public void PerformOperation() {
        Console.WriteLine("Lane이 움직이기 시작합니다.");
    }
}
}