using System;
using UnityEditor;
using UnityEngine;

public class Connection
{
    public Node inPoint;
    public Node outPoint;

    public Connection(Node inPoint, Node outPoint, Action<Connection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
    }

    public void Draw()
    {
        Handles.DrawLine(inPoint.rect.center, outPoint.rect.center);
        
        Vector3 toTarget = (inPoint.rect.center - outPoint.rect.center).normalized;

        Vector3 perp = Vector3.Cross(toTarget, Vector3.forward).normalized;

        Vector3 center = (inPoint.rect.center + outPoint.rect.center) * 0.5f;
        
        float size = 16f;
        Vector3[] arrowPoints = new Vector3[] {
            center + toTarget * size,
            center + perp * size * 0.4f,
            center - perp * size * 0.4f
        }; 

        Handles.DrawAAConvexPolygon(arrowPoints);
    }
}