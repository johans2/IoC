using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using Cakewalk.IoC.Core;
using Cakewalk.IoC;
using System;

public class NodeBasedEditor : EditorWindow
{
    private List<Node> nodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    [MenuItem("Window/Node Based Editor")]
    private static void OpenWindow()
    {
        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Dependency graph");
    }

    private void OnEnable()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
        nodeStyle.alignment = TextAnchor.MiddleCenter;

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
        selectedNodeStyle.alignment = TextAnchor.MiddleCenter;

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

        CreateNodeForAllClassesDEBUG();
    }

    private void CreateNodeForAllClassesDEBUG() {
        Type[] types = Assembly.GetAssembly(typeof(BootStrapper)).GetTypes();



        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        if(connections == null) {
            connections = new List<Connection>();
        }
        if(alreadyDrawnNodes == null) {
            alreadyDrawnNodes = new Dictionary<Type, Node>();
        }

        for (int i = 0; i < types.Length; i++)
        {
            if(!types[i].IsSubclassOf(typeof(TestClass))) {
                continue;
            }
            parentNode = null;
            DrawNodeRecursive(types[i]);
            /*
            FieldInfo[] depFields = Container.GetDependencyFields(types[i]);

            List<Type> dependencyChain = new List<Type>();

            if(depFields.Length == 0) {
                continue;
            }

            //dependencyChain.Add(depFields[i].FieldType);
            
            Debug.Log(types[i].Name);
            nodes.Add(new Node(types[i].Name, startPos + new Vector2(i *200,0), 200, 50, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint));
            */

        }
    }

    Vector2 startPos = new Vector2(1, 1);
    int counterX = 0;
    int counterY = 0;
    Node parentNode;
    Dictionary<Type, Node> alreadyDrawnNodes;

    private void DrawNodeRecursive(Type type) {
        // Ta en type.
        // Rita låda för type.

        Node node;

        if(!alreadyDrawnNodes.TryGetValue(type, out node)) {
            node = new Node(type.Name, startPos /*+ new Vector2(0, 50 * -counterY)*/, 200, 50, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle/*, OnClickInPoint, OnClickOutPoint*/);
            nodes.Add(node);
            alreadyDrawnNodes.Add(type, node);


        }
        
        if(parentNode != null) {
            connections.Add(new Connection(node, parentNode, null));
        }

        parentNode = node;
        
        counterY += 1;

        // Ta alla dependencies.
        // Gör samma sak rekursivt.
        FieldInfo[] depFields = Container.GetDependencyFields(type);

        for(int i = 0; i < depFields.Length; i++) {
            DrawNodeRecursive(depFields[i].FieldType);
        }





    }

    
    private void OnGUI()
    {
        DrawNodes();
        DrawConnections();

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawNodes()
    {
        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        //nodes.Add(new Node(mousePosition, 200, 50, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint));
    }
    /*
    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }*/
    /*
    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
    */
    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    private void CreateConnection(Node nodeA, Node nodeB)
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(nodeA, nodeB, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
}