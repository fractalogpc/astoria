float SphereSDF(float3 p, float3 center, float radius) {
  return length(p - center) - radius;
}

float4 March_float(in float FOV, in float4 ScreenPosition, in float3 CameraPosition, in float3 CameraDirection, in float2 CameraSize, out float4 Color) {
  Color = float4(0, 0, 0, 0);

  float3 rayOrigin = CameraPosition;
  float3 rayDirection = normalize(ScreenPosition.xyz - CameraPosition);

  float totalDistance = 0.0;
  const int maxSteps = 100;
  const float minDist = 0.001;
  const float maxDist = 100.0;

  for (int i = 0; i < maxSteps; i++) {
    float3 currentPosition = rayOrigin + totalDistance * rayDirection;
    float distance = SphereSDF(currentPosition, float3(0, 0, 0), 0.1);

    if (distance < minDist) {
      Color = float4(1, 0, 0, 1); // Color the sphere red
      break;
    }

    totalDistance += distance;

    if (totalDistance > maxDist) {
      break;
    }
  }

  return Color;
}