using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MapGenerator
{
    public const float InnerOffset = 0.05f;
    public const float OuterOffset  = 0.15f;
    public const float LineWidth  = 0.01f;
    public const int NodeCount  = 100;
    public const int VertexCount  = 50;

    public const int MinAngle  = 30;
    public const int MaxAngle  = 90;
    public const int MinLen  = 30;
    public const int MaxLen  = 100;
    public const int SegmentCount = 10;
    public const int Radius = 3;

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
            var segment = GenerateSegment(4);
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
        Nodes = segments.SelectMany(s => s.Nodes).ToList();
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
        for (int i = 0; i < NodeCount; i++)
        {
            var angleStep = (float) i / NodeCount * angle;
            var x = Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius-Radius;
            var y = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;

            var pos = new Vector3(x, y, 0);
            var rot=new Vector3(0,0,angleStep);
            segment.AddNode(pos,rot);
        }

        for (int i = 0; i < 4; i++)
        {
            float radius = Radius;
            if (i == 0) radius += -OuterOffset;
            if (i == 1) radius += -InnerOffset;
            if (i == 2) radius += InnerOffset;
            if (i == 3) radius += OuterOffset;

            var vertices = new List<Vector3>();
            for (int p = 0; p < VertexCount; p++)
            {
                var angleStep = (float) p / VertexCount * angle;
                var x = Mathf.Cos(Mathf.Deg2Rad * angleStep) * radius - radius;
                var y = Mathf.Sin(Mathf.Deg2Rad * angleStep) * radius;
                vertices.Add(new Vector3(x, y, 0));
            }
            var line = GenerateLine(vertices);
            line.transform.localPosition = new Vector3(radius-Radius, 0, 0);
            line.transform.parent = segment.Parent.transform;
        }

        return segment;
    }

    private Segment GenerateRightSegment(int angle)
    {
        var segment=new Segment();
        for (int i = 0; i < NodeCount; i++)
        {
            var angleStep = (float)i / NodeCount * angle;
            var x = -Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius+Radius;
            var y = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;

            var pos = new Vector3(x, y, 0);
            var rot=new Vector3(0,0,-angleStep);
            segment.AddNode(pos,rot);
        }

        for (int i = 0; i < 4; i++)
        {
            float radius = Radius;
            if (i == 0) radius += OuterOffset;
            if (i == 1) radius += InnerOffset;
            if (i == 2) radius += -InnerOffset;
            if (i == 3) radius += -OuterOffset;

            var vertices = new List<Vector3>();
            for (int p = 0; p < VertexCount; p++)
            {
                var angleStep = (float) p / VertexCount * angle;
                var x = -Mathf.Cos(Mathf.Deg2Rad * angleStep) * radius + radius;
                var y = Mathf.Sin(Mathf.Deg2Rad * angleStep) * radius;
                vertices.Add(new Vector3(x, y, 0));
            }
            var line = GenerateLine(vertices).transform;
            line.localPosition = new Vector3(Radius-radius, 0, 0);
            line.parent = segment.Parent.transform;
        }

        return segment;
    }

    private Segment GenerateUpSegment(int angle)
    {
        var segment=new Segment();
        for (int i = 0; i < NodeCount; i++)
        {
            var angleStep = (float)i / NodeCount * angle;
            var x = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;
            var y = Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius-Radius;

            var pos=new Vector3(0,x,y);
            var rot=new Vector3(-angleStep,0,0);
            segment.AddNode(pos,rot);
        }

        for (int i = 0; i < 4; i++)
        {
            float offset = 0;
            if (i == 0) offset = -OuterOffset;
            if (i == 1) offset = -InnerOffset;
            if (i == 2) offset = InnerOffset;
            if (i == 3) offset = OuterOffset;

            var vertices = new List<Vector3>();
            for (int p = 0; p < VertexCount; p++)
            {
                var angleStep = (float)p / VertexCount * angle;
                var x = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;
                var y = Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius-Radius;
                vertices.Add(new Vector3(0,x,y));
            }
            var line = GenerateLine(vertices).transform;
            line.localPosition=new Vector3(offset,0,0);
            line.parent = segment.Parent.transform;
        }
        return segment;
    }

    private Segment GenerateDownSegment(int angle)
    {
        var segment=new Segment();
        for (int i = 0; i < NodeCount; i++)
        {
            var angleStep = (float)i / NodeCount * angle;
            var x = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;
            var y = -Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius+Radius;

            var pos=new Vector3(0,x,y);
            var rot=new Vector3(angleStep,0,0);
            segment.AddNode(pos,rot);
        }
        for (int i = 0; i < 4; i++)
        {
            float offset = 0;
            if (i == 0) offset = -OuterOffset;
            if (i == 1) offset = -InnerOffset;
            if (i == 2) offset = InnerOffset;
            if (i == 3) offset = OuterOffset;

            var vertices = new List<Vector3>();
            for (int p = 0; p < VertexCount; p++)
            {
                var angleStep = (float)p / VertexCount * angle;
                var x = Mathf.Sin(Mathf.Deg2Rad * angleStep) * Radius;
                var y = -Mathf.Cos(Mathf.Deg2Rad * angleStep) * Radius+Radius;
                vertices.Add(new Vector3(0,x,y));
            }
            var line = GenerateLine(vertices).transform;
            line.localPosition=new Vector3(offset,0,0);
            line.parent = segment.Parent.transform;
        }
        return segment;
    }

    private Segment GenerateStraightSegment(int length)
    {
        var segment=new Segment();
        for (int i = 0; i < NodeCount; i++)
        {
            var y = (float)i/NodeCount*length/100;
            var pos=new Vector3(0,  y, 0);
            var rot=new Vector3(0,0,0);
            segment.AddNode(pos,rot);
        }
        for (int i = 0; i < 4; i++)
        {
            float offset = 0;
            if (i == 0) offset = -OuterOffset;
            if (i == 1) offset = -InnerOffset;
            if (i == 2) offset = InnerOffset;
            if (i == 3) offset = OuterOffset;

            var vertices = new List<Vector3>();
            for (int p = 0; p < VertexCount; p++)
            {
                var y = (float)p/VertexCount*length/100;
                vertices.Add(new Vector3(0,y,0));
            }
            var line = GenerateLine(vertices).transform;
            line.localPosition=new Vector3(offset,0,0);
            line.parent = segment.Parent.transform;
        }
        return segment;
    }

    private GameObject GenerateLine(List<Vector3> vertices)
    {
        var lineObject = new GameObject("Line");
        var line = lineObject.AddComponent<LineRenderer>();
        line.positionCount = VertexCount;
        line.SetPositions(vertices.ToArray());
        line.startWidth = LineWidth;
        line.endWidth = LineWidth;
        line.useWorldSpace = false;
        return lineObject;
    }
}