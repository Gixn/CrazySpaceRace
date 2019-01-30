using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MapGenerator
{
    public const float InnerOffset = 0.05f;
    public const float OuterOffset  = 0.15f;
    public const float LineWidth  = 0.01f;
    public const int NodeMultiplier  = 10;
    public const int VertexMultiplier  = 2;

    public const int MinAngle  = 30;
    public const int MaxAngle  = 90;

    public const int SegmentCount = 30;
    public const int Radius = 3;

    private Random random;
    public List<GameObject> Nodes=new List<GameObject>();
    public List<Segment> Segments = new List<Segment>();

    public MapGenerator(Random random)
    {
        this.random = random;
    }

    public void GenerateSegments()
    {
        Segment previous = GenerateSegment(0);
        Segments.Add(previous);
        
        for (int i = 0; i < SegmentCount; i++)
        {
            var segment = GenerateSegment();
            previous.Append(segment);
            Segments.Add(segment);
            previous = segment;
        }
        updateNodes();
    }


    public void AddSegments(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            var segment = GenerateSegment();
            Segments[Segments.Count-1].Append(segment);
            Segments.Add(segment);
        }
        updateNodes();
    }

    public int RemoveSegments(int count = 1)
    {
        var removedNodes = 0;
        for (int i = 0; i < count; i++)
        {
            removedNodes += Segments[0].Nodes.Count;
            Segments[0].Destroy();
            Segments.RemoveAt(0);
        }
        updateNodes();

        return removedNodes;
    }

    private void updateNodes()
    {
        Nodes = Segments.SelectMany(s => s.Nodes).ToList();
    }

 

    private Segment GenerateSegment(int type=-1)
    {
        Segment segment=null;

        var angle = random.Next(MinAngle,MaxAngle);

        if (type == -1)
        {
            type = random.Next(0, 5);
        }

        switch (type)
        {
            case 0:
            {
                segment = GenerateStraightSegment(angle);
            }
                break;
            case 1:
            {
                segment = GenerateLeftSegment(angle);
            }
                break;
            case 2:
            {
                segment = GenerateRightSegment(angle);
            }
                break;
            case 3:
            {
                segment = GenerateUpSegment(angle);
            }
                break;
            case 4:
            {
                segment = GenerateDownSegment(angle);
            }
                break;
        }
        return segment;
    }

    private Segment GenerateLeftSegment(int angle)
    {
        var segment=new Segment();
        for (int i = 0; i < angle*NodeMultiplier; i++)
        {
            var angleStep = (float)i / NodeMultiplier;
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
            for (int p = 0; p <= angle*VertexMultiplier; p++)
            {
                var angleStep = (float)p / VertexMultiplier;
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
        for (int i = 0; i < angle*NodeMultiplier; i++)
        {
            var angleStep = (float)i / NodeMultiplier;
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
            for (int p = 0; p <= angle*VertexMultiplier; p++)
            {
                var angleStep = (float)p / VertexMultiplier;
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
        for (int i = 0; i < angle*NodeMultiplier; i++)
        {
            var angleStep = (float)i / NodeMultiplier;
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
            for (int p = 0; p <= angle*VertexMultiplier; p++)
            {
                var angleStep = (float)p / VertexMultiplier;
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
        for (int i = 0; i < angle*NodeMultiplier; i++)
        {
            var angleStep = (float)i / NodeMultiplier;
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
            for (int p = 0; p <= angle*VertexMultiplier; p++)
            {
                var angleStep = (float)p / VertexMultiplier;
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

    private Segment GenerateStraightSegment(int angle)
    {
        var length = angle;
        var circumference = Radius * 2 * Mathf.PI;
        var step=circumference / 360;

        var segment=new Segment();
        for (int i = 0; i < length*NodeMultiplier; i++)
        {
            var y = (float)i/NodeMultiplier*step;
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
            for (int p = 0; p <= length*VertexMultiplier; p++)
            {
                var y = (float)p/VertexMultiplier*step;
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
        line.positionCount = vertices.Count;
        line.SetPositions(vertices.ToArray());
        line.startWidth = LineWidth;
        line.endWidth = LineWidth;
        line.useWorldSpace = false;
        return lineObject;
    }
}