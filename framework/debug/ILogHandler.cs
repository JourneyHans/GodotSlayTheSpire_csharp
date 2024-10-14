namespace framework.debug;

public interface ILogHandler {
    void Log(string log);
    void Warning(string log);
    void Error(string log);
}