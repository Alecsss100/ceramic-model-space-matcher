using System;
using System.Linq;
using Newtonsoft.Json;

public sealed class MatchResultJsonBuilder
{
    public string Build(MatchRunResult runResult)
    {
        if (runResult == null)
            return "{}";

        var offsets = runResult.Result?.Offsets
            .Select(Matrix4x4JsonDto.FromMatrix4x4)
            .ToArray() ?? Array.Empty<Matrix4x4JsonDto>();

        var dto = new MatchResultJsonDto
        {
            Algorithm = runResult.AlgorithmName,
            ModelCount = runResult.ModelCount,
            SpaceCount = runResult.SpaceCount,
            WasCancelled = runResult.WasCancelled,
            OffsetsCount = offsets.Length,
            Offsets = offsets,
        };

        return JsonConvert.SerializeObject(dto, Formatting.Indented);
    }

    sealed class MatchResultJsonDto
    {
        public string Algorithm { get; set; }
        public int ModelCount { get; set; }
        public int SpaceCount { get; set; }
        public bool WasCancelled { get; set; }
        public int OffsetsCount { get; set; }
        public Matrix4x4JsonDto[] Offsets { get; set; }
    }
}
