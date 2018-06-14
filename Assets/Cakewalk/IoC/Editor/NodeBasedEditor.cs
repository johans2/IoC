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

        List<Type> testTypes = new List<Type>();

        for(int i = 0; i < types.Length; i++) {
            if(types[i].IsSubclassOf(typeof(TestClass))) {
                testTypes.Add(types[i]);
            }
        }
        
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

        int row_ = 0;

        for (int i = 0; i < testTypes.Count; i++)
        {
            
            parentNode = null;

            Debug.Log(testTypes[i].Name);
            column = 0;

            if(alreadyDrawnNodes.ContainsKey(testTypes[i])) {
                row_++;
            }


            DrawNodeRecursive(testTypes[i], row_);

        }
    }

    Vector2 startPos = new Vector2(0, 0);
    int counterX = 0;
    int column = 0;
    Node parentNode;
    Dictionary<Type, Node> alreadyDrawnNodes;

    // TODO: DO this.. https://en.wikipedia.org/wiki/Coffman%E2%80%93Graham_algorithm
    // 1. Lägg till incoming out outgoing nodes i node klassen.
    // 2. Skriv ut detta på boxarna och verifiera att det funkar.
    // 3. Implementera algoritmen i länken.

    private void DrawNodeRecursive(Type type, int row) {
        // Ta en type.
        // Rita låda för type.

        if(type.Name == "F") {
            int a = 1;
        }


        // Draw the node if it has not already been drawn.
        Node node;
        if(!alreadyDrawnNodes.TryGetValue(type, out node)) {
            node = new Node(type.Name, startPos + new Vector2(50f * row, 100f * column), 200, 50, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle/*, OnClickInPoint, OnClickOutPoint*/);
            nodes.Add(node);
            alreadyDrawnNodes.Add(type, node);
        }

        // If a parent is set, make a connection to it.
        if(parentNode != null) {
            connections.Add(new Connection(parentNode, node, null));
        }

        // Go lower
        column ++;

        // Get all dependencies.
        FieldInfo[] depFields = Container.GetDependencyFields(type);

        // Recursion 
        // Go over all child nodes and do the same thing.
        for(int i = 0; i < depFields.Length; i++) {
            parentNode = node;
            DrawNodeRecursive(depFields[i].FieldType, i);
        }
    }

    
    private void OnGUI()
    {
        DrawConnections();
        DrawNodes();

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