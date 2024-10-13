using System;

namespace framework.math;

public static class Ease {
    public static float EaseOutCubic(this float num) {
        return 1.0f - MathF.Pow(1.0f - num, 3.0f);
    }
}