using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Map
{

    public const int SafetyOffsetStart = 1000;
    public const int SafetyOffsetEnd = 1000;

    private MapGenerator generator;
    private Random random;
    private int nodeIndex = 0;
    private List<GameObject> objects;

    public Map(int seed,List<GameObject> objects)
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
        this.objects = objects;

    }

    public void GenerateMap()
    {
        placeObjects(generator.GenerateSegments());
    }

    public GameObject GetNode(int offset=0)
    {
        var lastNode = generator.Nodes.Count - 1;
        if (nodeIndex+offset>lastNode-SafetyOffsetEnd)
        {
            placeObjects(generator.AddSegments(10));
        }
        var firstNodeOfSecondSegment = generator.Segments[0].Nodes.Count;
        if (nodeIndex>firstNodeOfSecondSegment+SafetyOffsetStart)
        {
            nodeIndex -= generator.RemoveSegments(1);
        }
        return generator.Nodes[nodeIndex  + offset];
    }

    public void Next(int skip=0)
    {
        nodeIndex += skip+1;
    }

    private void placeObjects(List<Segment> segments)
    {
        segments.ForEach(s =>
        {
            s.PlaceRandomObjects(objects,3);
        });
    }










}