using System.Collections.Generic;

public static class DummyData
{
    public static Dictionary<string, float> DummyHealths = new Dictionary<string, float>
    {
        // ----- CHAPTER 1 -----
        { "1-1", 100f },
        { "1-2", 200f },
        { "1-3", 300f },
        { "1-4", 400f },
        { "1-5", 1500f }, // 1챕터 보스

        // ----- CHAPTER 2 (1스테이지보다 체력이 더 높음!) -----
        { "2-1", 500f },  // 1-1보다 훨씬 높음
        { "2-2", 700f },
        { "2-3", 1000f },
        { "2-4", 1400f },
        { "2-5", 5000f }  // 2챕터 보스 (대폭 상승)
    };
}