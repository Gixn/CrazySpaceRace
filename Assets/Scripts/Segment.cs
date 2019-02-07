using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Segment
{
    private Random random;
    public const float LaneOffset=(MapGenerator.OuterOffset + MapGenerator.InnerOffset) / 2;
    public readonly List<GameObject> Nodes = new List<GameObject>();
    public GameObject Parent = new GameObject();

    public Segment(Random random)
    {
        this.random = random;
    }

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
    public void PlaceObject(GameObject obj,int nodeIndex,int lane)
    {
        var scale=new Vector3(0.08f,0.08f,0.08f);
        var node = Nodes[nodeIndex].transform;
        var clone = Object.Instantiate(obj, Parent.transform, true);

        clone.transform.localScale = scale;
        clone.transform.position = node.position;
        clone.transform.rotation = node.rotation;
        clone.transform.Translate(new Vector3(lane*LaneOffset,0,0));
    }

    public void PlaceRandomObjects(List<GameObject> objects,int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var randomIndex=random.Next(0, Nodes.Count);
            var randomLane = random.Next(-1, 2);
            var randomSelection = random.Next(0, objects.Count);
            PlaceObject(objects[randomSelection],randomIndex,randomLane);
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(Parent);
    }







}