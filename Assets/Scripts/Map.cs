using UnityEngine;
using Random = System.Random;

public class Map
{
    public const float LaneOffset=(MapGenerator.OuterOffset + MapGenerator.InnerOffset) / 2;

    private MapGenerator generator;
    private Random random;
    private int nodeIndex = 0;
    private int nextUpdate = MapGenerator.MaxAngle*MapGenerator.NodeMultiplier;


    public Map(int seed)
    {
        if (seed<0)
        {
            random=new Random();
        }
        else
        {
            random = new Random(seed);
        }
        generator=  new MapGenerator(random);

    }

    public void GenerateMap()
    {
        generator.GenerateSegments();
    }

    public GameObject getNode(int offset=0)
    {
        if (nodeIndex  + offset < generator.Nodes.Count)
        {
            return generator.Nodes[nodeIndex  + offset];
        }
        return generator.Nodes[generator.Nodes.Count-1];
    }

    public void next(int skip=0)
    {
        nodeIndex += skip+1;
    }

    public void PlaceObject(GameObject obj,int nodeIndex,int lane)
    {
        var scale=new Vector3(0.05f,0.05f,0.05f);
        var node = generator.Nodes[nodeIndex].transform;
        var clone = Object.Instantiate(obj);
        clone.transform.localScale = scale;
        clone.transform.position = node.position;
        clone.transform.rotation = node.rotation;
        clone.transform.Translate(new Vector3(lane*LaneOffset,0,0));
    }

    public void PlaceRandomObjects(GameObject obj,int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var randomIndex=random.Next(0, generator.Nodes.Count);
            var randomLane = random.Next(-1, 2);
            PlaceObject(obj,randomIndex,randomLane);
        }
    }






}