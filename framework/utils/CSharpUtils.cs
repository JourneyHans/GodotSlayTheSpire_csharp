using System.Threading.Tasks;

namespace framework.utils;

public static class TimeUtils {
    public static Task DelaySeconds(int seconds) {
        return Task.Delay(seconds * 1000);
    }
}