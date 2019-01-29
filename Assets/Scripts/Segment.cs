using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    public readonly List<GameObject> Vertices = new List<GameObject>();
    public GameObject Parent = new GameObject();

    public void Append(Segment segment)
    {
        var last=Vertices[Vertices.Count - 1];
        segment.Parent.transform.position = last.transform.position;
        segment.Parent.transform.rotation = last.transform.rotation;
    }

    public void AddVertex(Vector3 pos, Vector3 rot)
    {
        var vertex=new GameObject();
        vertex.transform.localScale=new Vector3(0.05f,0.05f,0.05f);
        vertex.transform.localPosition = pos;
        vertex.transform.localRotation = Quaternion.Euler(rot);
        vertex.transform.parent = Parent.transform;
        Vertices.Add(vertex);
    }

    public void Destroy()
    {
        GameObject.Destroy(Parent);
    }







}