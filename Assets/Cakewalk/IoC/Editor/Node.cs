using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Node
{
    public Rect rect;
    public string className;
    public string title = "Derp";
    public bool isDragged;
    public bool isSelected;
    public int graphLevel;
    public int level;
    public int orderInLevel;
    
    public List<Node> incomingDeps = new List<Node>();
    public List<Node> outgoingDeps = new List<Node>();

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Node(string className, Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle/*, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint*/)
    {
        this.className = className;
        title = this.className;
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        level = -1;
        orderInLevel = -1;
    }

    public void AddIncomingDep(Node node) {
        if(incomingDeps.Contains(node)) {
            Debug.LogWarning(string.Format("Node {0} already has an incoming dep to {1}", className, node.className));
        }
        incomingDeps.Add(node);
        UpdateTitle();
    }

    public void AddOutgoingDep(Node node) {
        if(outgoingDeps.Contains(node)) {
            Debug.LogWarning(string.Format("Node {0} already has an outgoing dep to {1}", className, node.className));
        }
        outgoingDeps.Add(node);
        UpdateTitle();
    }

    private void UpdateTitle() {
        string inDepsString = "";
        string outDepsString = "";

        foreach(var dep in incomingDeps) {
            inDepsString += dep.className + " ";
        }

        foreach(var dep in outgoingDeps) {
            outDepsString += dep.className + " ";
        }

        title = className + " | in: " + inDepsString + " out: " + outDepsString;
    }
    
    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void Draw()
    {
        GUI.Box(rect, title, style);
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }
}