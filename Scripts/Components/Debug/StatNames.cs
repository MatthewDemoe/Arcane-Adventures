namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public static class StatNames
    {
        public static class Memory
        {
            public const string TotalUsedMemory = "Total Used Memory";
            public const string TotalReservedMemory = "Total Reserved Memory";
            public const string GCUsedMemory = "GC Used Memory";
            public const string GCReservedMemory = "GC Reserved Memory";
        }

        public static class Render
        {
            public const string SetPassCallsCount = "SetPass Calls Count";
            public const string DrawCallsCount = "Draw Calls Count";
            public const string TotalBatchesCount = "Total Batches Count";
            public const string TrianglesCount = "Triangles Count";
            public const string VerticesCount = "Vertices Count";
        }

        public static string Shorten(string statName) => statName
            .Replace(" Count", string.Empty)
            .Replace(" Memory", string.Empty)
            .Replace("Reserved", "Res.");

    }
}