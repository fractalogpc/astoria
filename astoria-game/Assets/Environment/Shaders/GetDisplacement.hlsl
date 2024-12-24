StructuredBuffer<float4> _SnowDisplacementBuffer;
int _PositionsCount;

void GetDisplacement_float(in float3 WorldPosition, out float Displacement) {
  Displacement = 0;
  for (int i = 0; i < _PositionsCount; i++) {
    float4 DisplacementData = _SnowDisplacementBuffer[i];
    float3 DisplacementPosition = DisplacementData.xyz;
    float DisplacementValue = DisplacementData.w;
    float PlanarDistance = distance(float2(WorldPosition.x, WorldPosition.z), float2(DisplacementPosition.x, DisplacementPosition.z));
    float PlanarMultiplier = 1 - pow(saturate(PlanarDistance), 0.1);
    float VerticalDistance = WorldPosition.y - DisplacementPosition.y;
    float Displ_Value = DisplacementValue * max(0, VerticalDistance) * saturate(PlanarMultiplier * 2);
    Displacement = max(Displacement, Displ_Value);
  }
}