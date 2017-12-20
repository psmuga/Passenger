using System;
using System.Collections.Generic;

namespace Passenger.Core.Domain
{
    public class Route
    {
        public string Name {get; protected set;}
        public Node Start{get; protected set;}
        public Node End{get; protected set;}
        public double Distance{get; protected set;}
        protected Route()
        {
        }
        protected Route(string name, Node startNode, Node endNode, double distance)
        {
            Name = name;
            Start = startNode;
            End = endNode;
            Distance = distance;
        }
        public static Route Create(string name,Node start, Node end, double distance) 
            =>  new Route(name, start,end,distance);
    }
}