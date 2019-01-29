using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MapGenerator
{
    public float InnerOffset { get; set; } = 0.05f;
    public float OuterOffset { get; set; } = 0.15f;
    public float LineWidth { get;set;  } = 0.01f;
    public int Points { get; set; } = 50;
    public int MinAngle { get;set;  } = 30;
    public int MaxAngle { get;set;  } = 90;
    public int MinLen { get; set; } = 30;
    public int MaxLen { get; set; } = 100;
    public int SegmentCount { get; set; } = 10;

    private const int Radius = 3;
    private Random random;
    public List<GameObject> Nodes=new List<GameObject>();
    private List<Segment> segments = new List<Segment>();


    public void GenerateSegments(int seed)
    {
        random=new Random(seed);
        Segment previous = GenerateSegment();
        segments.Add(previous);
        
        for (int i = 0; i < SegmentCount; i++)
        {
            var segment = GenerateSegment();
            previous.Append(segment);
            segments.Add(segment);
            previous = segment;
        }
        updateNodes();
    }

    public void UpdateSegments(int segmentCount)
    {
        for (int i = 0; i < segmentCount; i++)
        {
            var segment = GenerateSegment();
            segments[segments.Count-1].Append(segment);
            segments.Add(segment);
            segments[0].Destroy();
            segments.RemoveAt(0);
        }
        updateNodes();
    }

    private void updateNodes()
    {
        Nodes = segments.SelectMany(s => s.Vertices).ToList();
    }

 

    private Segment GenerateSegment(int type=-1)
    {
        Segment segment=null;

        var length = random.Next(MinLen, MaxLen);
        var angle = random.Next(MinAngle,MaxAngle);

        if (type == -1)
        {
            type = random.Next(0, 5);
        }

        switch (type)
        {
            case 0: //right
            {
                segment = GenerateRightSegment(angle);
            }
                break;
            case 1: //left
            {
                segment = GenerateLeftSegment(angle);
            }
                break;
            case 2: //up
            {
                segment = GenerateUpSegment(angle);
            }
                break;
            case 3: //down
            {
                segment = GenerateDownSegment(angle);
            }
                break;
            case 4: //straight
            {
                segment = GenerateStraightSegment(length);
            }
                break;
        }
        return segment;
    }

    private Segment GenerateLeftSegment(int angle)
    {
        var segment=new Segment();
        for (int i = 0; i < Points; i++)
        {
            var angleStep = (float) i / Points * angle;
            var x = Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius-Radius;
            var y = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;

            var pos = new Vector3(x, y, 0);
            var rot=new Vector3(0,0,angleStep);
            segment.AddVertex(pos,rot);
        }
        var baseLine = GenerateLine(segment.Vertices);
        baseLine.transform.parent = segment.Parent.transform;

        for (int i = 0; i < 4; i++)
        {
            float radius = Radius;
            if (i == 0) radius += -OuterOffset;
            if (i == 1) radius += -InnerOffset;
            if (i == 2) radius += InnerOffset;
            if (i == 3) radius += OuterOffset;

            var vertices = new List<Vector3>();
            for (int p = 0; p < Points; p++)
            {
                var angleStep = (float) p / Points * angle;
                var x = Mathf.Cos(Mathf.Deg2Rad * angleStep) * radius - radius;
                var y = Mathf.Sin(Mathf.Deg2Rad * angleStep) * radius;
                vertices.Add(new Vector3(x, y, 0));
            }
            var line = GenerateLine(vertices);
            line.transform.localPosition = new Vector3(radius-Radius, 0, 0);
            line.transform.parent = segment.Parent.transform;
        }

        baseLine.SetActive(false);
        return segment;
    }

    private Segment GenerateRightSegment(int angle)
    {
        var segment=new Segment();
        for (int i = 0; i < Points; i++)
        {
            var angleStep = (float)i / Points * angle;
            var x = -Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius+Radius;
            var y = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;

            var pos = new Vector3(x, y, 0);
            var rot=new Vector3(0,0,-angleStep);
            segment.AddVertex(pos,rot);
        }
        var baseLine = GenerateLine(segment.Vertices);
        baseLine.transform.parent = segment.Parent.transform;

        for (int i = 0; i < 4; i++)
        {
            float radius = Radius;
            if (i == 0) radius += OuterOffset;
            if (i == 1) radius += InnerOffset;
            if (i == 2) radius += -InnerOffset;
            if (i == 3) radius += -OuterOffset;

            var vertices = new List<Vector3>();
            for (int p = 0; p < Points; p++)
            {
                var angleStep = (float) p / Points * angle;
                var x = -Mathf.Cos(Mathf.Deg2Rad * angleStep) * radius + radius;
                var y = Mathf.Sin(Mathf.Deg2Rad * angleStep) * radius;
                vertices.Add(new Vector3(x, y, 0));
            }
            var line = GenerateLine(vertices).transform;
            line.localPosition = new Vector3(Radius-radius, 0, 0);
            line.parent = segment.Parent.transform;
        }

        baseLine.SetActive(false);
        return segment;
    }

    private Segment GenerateUpSegment(int angle)
    {
        var segment=new Segment();
        for (int i = 0; i < Points; i++)
        {
            var angleStep = (float)i / Points * angle;
            var x = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;
            var y = Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius-Radius;

            var pos=new Vector3(0,x,y);
            var rot=new Vector3(-angleStep,0,0);
            segment.AddVertex(pos,rot);
        }
        var baseLine = GenerateLine(segment.Vertices);
        baseLine.transform.parent = segment.Parent.transform;

        InstantiateWithOffset(baseLine,segment);

        baseLine.SetActive(false);
        return segment;
    }

    private Segment GenerateDownSegment(int angle)
    {
        var segment=new Segment();
        for (int i = 0; i < Points; i++)
        {
            var angleStep = (float)i / Points * angle;
            var x = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;
            var y = -Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius+Radius;

            var pos=new Vector3(0,x,y);
            var rot=new Vector3(angleStep,0,0);
            segment.AddVertex(pos,rot);
        }
        var baseLine = GenerateLine(segment.Vertices);
        baseLine.transform.parent = segment.Parent.transform;

        InstantiateWithOffset(baseLine,segment);

        baseLine.SetActive(false);
        return segment;
    }

    private Segment GenerateStraightSegment(int length)
    {
        var segment=new Segment();
        for (int i = 0; i < Points; i++)
        {
            var y = (float)i/Points*length/100;
            var pos=new Vector3(0,  y, 0);
            var rot=new Vector3(0,0,0);
            segment.AddVertex(pos,rot);
        }
        var baseLine = GenerateLine(segment.Vertices);
        baseLine.transform.parent = segment.Parent.transform;

        InstantiateWithOffset(baseLine,segment);

        baseLine.SetActive(false);

        return segment;
    }

    private void InstantiateWithOffset(GameObject baseLine,Segment segment)
    {
        for (int i = 0; i < 4; i++)
        {
            float offset = 0;
            if (i == 0) offset = -OuterOffset;
            if (i == 1) offset = -InnerOffset;
            if (i == 2) offset = InnerOffset;
            if (i == 3) offset = OuterOffset;

            var line = Object.Instantiate(baseLine).transform;
            line.localPosition=new Vector3(offset,0,0);
            line.parent = segment.Parent.transform;
        }
    }

    private GameObject GenerateLine(List<GameObject> vertices)
    {
        return GenerateLine(vertices.ConvertAll(obj => obj.transform.position));
    }

    private GameObject GenerateLine(List<Vector3> vertices)
    {
        var lineObject = new GameObject("Line");
        var line = lineObject.AddComponent<LineRenderer>();
        line.positionCount = Points;
        line.SetPositions(vertices.ToArray());
        line.startWidth = LineWidth;
        line.endWidth = LineWidth;
        line.useWorldSpace = false;
        return lineObject;
    }
}