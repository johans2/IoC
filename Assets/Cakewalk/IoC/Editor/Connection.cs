using System;
using UnityEditor;
using UnityEngine;

public class Connection
{
    public Node outgoing;
    public Node incoming;

    public Connection(Node outgoing, Node incoming, Action<Connection> OnClickRemoveConnection)
    {
        this.outgoing = outgoing;
        this.incoming = incoming;
        this.outgoing.AddIncomingDep(incoming);
        this.incoming.AddOutgoingDep(outgoing);
    }

    public void Draw()
    {
        Handles.DrawLine(outgoing.rect.center, incoming.rect.center);
        
        Vector3 toTarget = (outgoing.rect.center - incoming.rect.center).normalized;

        Vector3 perp = Vector3.Cross(toTarget, Vector3.forward).normalized;

        Vector3 center = (outgoing.rect.center + incoming.rect.center) * 0.5f;
        
        float size = 16f;
        Vector3[] arrowPoints = new Vector3[] {
            center + toTarget * size,
            center + perp * size * 0.4f,
            center - perp * size * 0.4f
        }; 

        Handles.DrawAAConvexPolygon(arrowPoints);
    }
}