using System;
using UnityEditor;
using UnityEngine;

public class Connection
{
    public Node inPoint;
    public Node outPoint;
    //public Action<Connection> OnClickRemoveConnection;

    public Connection(Node inPoint, Node outPoint, Action<Connection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        //this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw()
    {

        //Handles.DrawLine(inPoint.rect.center, outPoint.rect.center);
        
        
        Handles.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.down * 50f,
            outPoint.rect.center - Vector2.down * 50f,
            Color.white,
            null,
            2f
        );
        /*
        Vector3 toTarget = inPoint.rect.center - outPoint.rect.center;

        Vector3[] arrowPoints = new Vector3[] {
            
        } 

        Handles.DrawAAConvexPolygon(arrowHead);
        */
        //Handles.ArrowHandleCap(0, (inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 1000, EventType.Ignore);
        
        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.LookRotation(inPoint.rect.position) * Quaternion.Euler(180,0,0), 4, 80, Handles.ArrowHandleCap))
        {
         /*   
            if (OnClickRemoveConnection != null)
            {
                OnClickRemoveConnection(this);
            }
           */ 
        }
    }
}