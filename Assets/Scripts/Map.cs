using UnityEngine;

public class Map
{
    public int SegmentOffset { get; set; } = 2;

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

    public void Update()
    {
        index += 1;
        if (index % getNodeOffset() == 0)
        {
            generator.UpdateSegments(SegmentOffset);
            index -= getNodeOffset();
        }
    }

    private int getNodeOffset()
    {
        return generator.Points * SegmentOffset;
    }

}