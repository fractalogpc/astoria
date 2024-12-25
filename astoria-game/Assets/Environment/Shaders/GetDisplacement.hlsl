Texture2D _SnowDisplacementTexture;
float2 _Center;
float _SnowDisplacementTextureSize;
SamplerState sampler_SnowDisplacementTexture {
  Filter = MIN_MAG_MIP_LINEAR;
  AddressU = Wrap;
  AddressV = Wrap;
};

void GetDisplacement_float(in float3 WorldPosition, out float Displacement) {
  float2 uv = WorldPosition.xz - _Center;
  uv /= _SnowDisplacementTextureSize;
  uv += 0.5;
  Displacement = _SnowDisplacementTexture.SampleLevel(sampler_SnowDisplacementTexture, uv, 0).r;
}