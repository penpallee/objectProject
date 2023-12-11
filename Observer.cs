namespace MachinVisionProgram {
public interface ISubject {
    public void Attach(IObserver observer);
    public void Detach(IObserver observer);
    public void Notify();
}

// IObserver 인터페이스
public interface IObserver {
    void Update(ISubject subject);
}
}