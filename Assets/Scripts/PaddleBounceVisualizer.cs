using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Transform))]
public class PaddleBounceVisualizer : MonoBehaviour
{
    [Header("Bounce Preview")]
    [Range(0f, 90f)]
    public float maxBounceAngle = 75f;

    [Header("Ray Settings")]
    public float rayLength = 2f;
    public Color centerColor = Color.green;
    public Color angleColor = Color.yellow;
    public Color zoneColorStart = Color.red;
    public Color zoneColorEnd = Color.green;

    [Header("Impact Zones")]
    [Range(1, 10)]
    public int impactZoneLines = 7;
    public float impactZoneWidth = 0.4f;

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;

        DrawBounceAngleLines(origin);
        DrawImpactZones(origin);
    }

    private void DrawBounceAngleLines(Vector3 origin)
    {
        // Línea central (recta hacia arriba)
        Gizmos.color = centerColor;
        Gizmos.DrawRay(origin, Vector3.up * rayLength);

        // Líneas anguladas
        Gizmos.color = angleColor;
        DrawBounceAngleRay(origin, -maxBounceAngle);
        DrawBounceAngleRay(origin, maxBounceAngle);
    }

    private void DrawBounceAngleRay(Vector3 origin, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f).normalized;
        Gizmos.DrawRay(origin, direction * rayLength);
    }

    private void DrawImpactZones(Vector3 origin)
    {
        if (impactZoneLines < 2) return;

        float paddleHeight = transform.localScale.y;
        int halfLines = impactZoneLines / 2;

        for (int i = -halfLines; i <= halfLines; i++)
        {
            float t = (i + halfLines) / (float)(impactZoneLines - 1);
            float yOffset = i * (paddleHeight / (impactZoneLines - 1));
            Gizmos.color = Color.Lerp(zoneColorStart, zoneColorEnd, t);

            Vector3 left = origin + Vector3.up * yOffset + Vector3.left * (impactZoneWidth / 2f);
            Vector3 right = origin + Vector3.up * yOffset + Vector3.right * (impactZoneWidth / 2f);
            Gizmos.DrawLine(left, right);
        }
    }
}
