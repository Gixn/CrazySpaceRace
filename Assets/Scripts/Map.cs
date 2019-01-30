using UnityEngine;

public class Map
{
    private MapGenerator generator = new MapGenerator();
    private int nodeIndex = 0;
    private int nextUpdate = MapGenerator.MaxAngle*MapGenerator.NodeMultiplier;

    public void Generate(int seed)
    {
        generator.GenerateSegments(seed);
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





}