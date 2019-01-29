using UnityEngine;

public class Map
{
    public const int SegmentOffset = 2;

    private MapGenerator generator = new MapGenerator();
    private int index = 0;

    public void Generate(int seed)
    {
        generator.GenerateSegments(seed);
    }

    public GameObject getNode(int offset=0)
    {
        return generator.Nodes[index + getNodeOffset() + offset];
    }

    public void next(int skip=0)
    {
        index += skip+1;
    }

    public void Update()
    {
        if (index % getNodeOffset() == 0)
        {
            generator.UpdateSegments(SegmentOffset);
            index -= getNodeOffset();
        }
    }

    private int getNodeOffset()
    {
        return MapGenerator.NodeCount * SegmentOffset;
    }

}