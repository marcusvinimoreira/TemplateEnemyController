using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyVisionController))]
public class ShowFov : Editor
{
    private void OnSceneGUI()
    {
        EnemyVisionController fovM = (EnemyVisionController)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fovM.transform.position, Vector2.up, Vector2.up, 360, fovM.fov);

        Vector3 vA = fovM.DirectionAngle(-fovM.fov / 2);
        Vector3 vB = fovM.DirectionAngle(fovM.fov / 2);

        Handles.DrawLine(fovM.transform.position, fovM.transform.position + vA * -fovM.rayVision);
        Handles.DrawLine(fovM.transform.position, fovM.transform.position + vB * -fovM.rayVision);



    }
}
