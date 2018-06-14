using System;
using UnityEditor;
using UnityEngine;

public class Connection
{
    public Node incoming;
    public Node outgoing;

    public Connection(Node incoming, Node outgoing, Action<Connection> OnClickRemoveConnection)
    {
        this.incoming = incoming;
        this.outgoing = outgoing;
        this.incoming.AddIncomingDep(incoming);
        this.outgoing.AddOutgoingDep(outgoing);
    }

    public void Draw()
    {
        Handles.DrawLine(incoming.rect.center, outgoing.rect.center);
        
        Vector3 toTarget = (incoming.rect.center - outgoing.rect.center).normalized;

        Vector3 perp = Vector3.Cross(toTarget, Vector3.forward).normalized;

        Vector3 center = (incoming.rect.center + outgoing.rect.center) * 0.5f;
        
        float size = 16f;
        Vector3[] arrowPoints = new Vector3[] {
            center + toTarget * size,
            center + perp * size * 0.4f,
            center - perp * size * 0.4f
        }; 

        Handles.DrawAAConvexPolygon(arrowPoints);
    }
}