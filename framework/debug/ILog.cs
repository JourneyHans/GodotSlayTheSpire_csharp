namespace framework.debug;

public interface ILog {
    void Print(string log);
    void PrintWarning(string log);
    void PrintErr(string log);
}