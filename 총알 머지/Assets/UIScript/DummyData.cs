using System.Collections.Generic;

public static class DummyData
{
    public static Dictionary<string, float> DummyHealths = new Dictionary<string, float>
    {
        // 아침 테마 (1-3 보스전)
        { "1-1", 100f },
        { "1-2", 150f },
        { "1-3", 600f },   // ⭐ 1-3 중간 보스 피통 대폭 상승!

        // 점심 테마 (1-6 보스전)
        { "1-4", 300f },
        { "1-5", 400f },
        { "1-6", 1500f },  // ⭐ 1-6 중간 보스 피통 대폭 상승!

        // 저녁 테마 (1-9 최종보스전)
        { "1-7", 800f },
        { "1-8", 1000f },
        { "1-9", 5000f }   // ⭐ 1-9 최종 보스 피통!!
    };
}