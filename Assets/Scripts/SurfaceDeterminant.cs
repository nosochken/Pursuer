using UnityEngine;

public static class SurfaceDeterminant
{
    private static float _rayLength = 1.5f;
    private static float _dotThreshold = 0.1f;

    public static SurfaceState GetSurfaceState(Vector3 position, Vector3 velocity)
    {
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hitInfo, _rayLength))
        {
            Vector3 normal = hitInfo.normal;

            Vector3 downSlopeDirection = Vector3.ProjectOnPlane(Vector3.down, normal).normalized;

            float dot = Vector3.Dot(velocity.normalized, downSlopeDirection);

            if (dot > _dotThreshold)
                return SurfaceState.Descent;
            else if (dot < -_dotThreshold)
                return SurfaceState.Rise;
        }

        return SurfaceState.Flat;
    }

    public static bool CanOvercomeSurface(Vector3 position, float surfaceMaxAngle)
    {
        if (Physics.Raycast(position, Vector3.up, out RaycastHit hitInfo, _rayLength))
        {
            float surfaceAngle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (surfaceAngle <= surfaceMaxAngle)
                return true;
        }

        return false;
    }
}