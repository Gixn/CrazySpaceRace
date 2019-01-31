using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Map
{

    public const int SafetyOffsetStart = 500;
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
        if (nodeIndex  + offset < generator.Nodes.Count)
        {
            return generator.Nodes[nodeIndex  + offset];
        }
        return generator.Nodes[generator.Nodes.Count-1];
    }

    public void Next(int skip=0)
    {
        nodeIndex += skip+1;

        var distanceToEnd = generator.Nodes.Count - nodeIndex;
        if (distanceToEnd<SafetyOffsetEnd)
        {
            placeObjects(generator.AddSegments(3));
        }

        var firstSegment = generator.Segments[0].Nodes.Count;
        if (nodeIndex>firstSegment+SafetyOffsetStart)
        {
            nodeIndex -= generator.RemoveSegments(1);
        }
    }

    private void placeObjects(List<Segment> segments)
    {
        segments.ForEach(s =>
        {
            s.PlaceRandomObjects(objects,3);
        });
    }










}