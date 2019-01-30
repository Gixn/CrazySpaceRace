using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    public readonly List<GameObject> Nodes = new List<GameObject>();
    public GameObject Parent = new GameObject();

    public void Append(Segment segment)
    {
        var last=Nodes[Nodes.Count - 1];
        segment.Parent.transform.position = last.transform.position;
        segment.Parent.transform.rotation = last.transform.rotation;
    }

    public void AddNode(Vector3 pos, Vector3 rot)
    {
//        var node = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var node = new GameObject();
        node.transform.localScale=new Vector3(0.05f,0.05f,0.05f);
        node.transform.localPosition = pos;
        node.transform.localRotation = Quaternion.Euler(rot);
        node.transform.parent = Parent.transform;
        Nodes.Add(node);
    }

    public void Destroy()
    {
        GameObject.Destroy(Parent);
    }







}