using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace WH_Physics
{
    internal class WH_Physics
    {
        private List<AABB> _objects;
        internal int _screenWidth, _screenHeight;
        internal int _lastAssignedID;

        public WH_Physics(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _objects = new List<AABB>();
            _lastAssignedID = 0;
        }

        public struct EndPoint
        {
            public enum Type { BEGIN = 0, END = 1 };
            public Type _type;
            public float _value;
            public int _ID;
            public EndPoint(Type type, float value, int ID)
            {
                _type = type;
                _value = value;
                _ID = ID;
            }
        }

        public struct Intersection
        {
            public int _intervalID_1;
            public int _intervalID_2;
            public Intersection(int intervalID_1, int intervalID_2)
            {
                if (intervalID_1 < intervalID_2)
                {
                    _intervalID_1 = intervalID_1;
                    _intervalID_2 = intervalID_2;
                }
                else
                {
                    _intervalID_1 = intervalID_2;
                    _intervalID_2 = intervalID_1;
                }
            }
        }

        public void update(float deltaTime)
        {
            foreach (AABB obj in _objects)
            {
                // Process Movement
                obj.update(deltaTime);

                // Process Screen Edge Collisions
                if (obj._rightEndPoint >= _screenWidth)
                {
                    obj.move(new Vector2(2 * (_screenWidth - obj._rightEndPoint), 0));
                    obj._xVelocity *= -1;
                }
                if (obj._bottomEndPoint >= _screenHeight)
                {
                    obj.move(new Vector2(0, 2*(_screenHeight - obj._bottomEndPoint)));
                    obj._yVelocity *= -1;
                }
                if (obj._leftEndPoint <= 0)
                {
                    obj.move(new Vector2(-2*obj._leftEndPoint, 0));
                    obj._xVelocity *= -1;
                }
                if (obj._topEndPoint <= 0)
                {
                    obj.move(new Vector2(0, -2*obj._topEndPoint));
                    obj._yVelocity *= -1;
                }
            }


            // Make then sort list of end points
            // pst combine this with the loop above later
            List<EndPoint> endPoints_X = new List<EndPoint>();
            List<EndPoint> endPoints_Y = new List<EndPoint>();
            foreach (AABB obj in _objects)
            {
                obj._intersectingWith.Clear();

                endPoints_X.Add(new EndPoint(EndPoint.Type.BEGIN, obj._leftEndPoint, obj._ID));
                endPoints_X.Add(new EndPoint(EndPoint.Type.END, obj._rightEndPoint, obj._ID));
                endPoints_Y.Add(new EndPoint(EndPoint.Type.BEGIN, obj._topEndPoint, obj._ID));
                endPoints_Y.Add(new EndPoint(EndPoint.Type.END, obj._bottomEndPoint, obj._ID));
            }
            endPoints_X.Sort((x, y) => x._value.CompareTo(y._value));
            endPoints_Y.Sort((x, y) => x._value.CompareTo(y._value));

            // Initialize empty Active Interval array
            List<int> activeInterval_X = new List<int>();
            List<int> activeInterval_Y = new List<int>();
            List<Intersection> intersections_X = new List<Intersection>();
            List<Intersection> intersections_Y = new List<Intersection>();

            // Iterate through the end points :)
            foreach (EndPoint endPoint in endPoints_X)
            {
                if (endPoint._type == EndPoint.Type.BEGIN)
                {
                    // Record intersection with active intervals
                    foreach (int ID in activeInterval_X)
                        intersections_X.Add(new Intersection(ID, endPoint._ID));

                    // Add to active set
                    activeInterval_X.Add(endPoint._ID);

                }
                if (endPoint._type == EndPoint.Type.END)
                {
                    // Remove from active set
                    activeInterval_X.Remove(endPoint._ID);

                }
            }
            foreach (EndPoint endPoint in endPoints_Y)
            {
                if (endPoint._type == EndPoint.Type.BEGIN)
                {
                    // Record intersection with active intervals
                    foreach (int ID in activeInterval_Y)
                        intersections_Y.Add(new Intersection(ID, endPoint._ID));

                    // Add to active set
                    activeInterval_Y.Add(endPoint._ID);

                }
                if (endPoint._type == EndPoint.Type.END)
                {
                    // Remove from active set
                    activeInterval_Y.Remove(endPoint._ID);

                }
            }

            // Compare X and Y intersections to find true intersections
            // this can be optimized in the future
            // n^2 my beloved
            List<Intersection> intersections = new List<Intersection>();
            intersections_Y.Sort((x, y) => x._intervalID_1.CompareTo(y._intervalID_1));
            for (int i = 0; i < intersections_X.Count; i++)
            {
                for (int j = 0; j < intersections_Y.Count; j++)
                {
                    if (intersections_X[i]._intervalID_1 == intersections_Y[j]._intervalID_1 && intersections_X[i]._intervalID_2 == intersections_Y[j]._intervalID_2)
                    {
                        intersections.Add(intersections_X[i]);
                    }
                }
            }

            foreach (Intersection intersection in intersections)
            {
                Predicate<AABB> objFinder_1 = (AABB aabb) => { return aabb._ID == intersection._intervalID_1; };
                Predicate<AABB> objFinder_2 = (AABB aabb) => { return aabb._ID == intersection._intervalID_2; };
                AABB object1 = _objects.Find(objFinder_1);
                AABB object2 = _objects.Find(objFinder_2);
                object1._intersectingWith.Add(intersection._intervalID_2);
                object2._intersectingWith.Add(intersection._intervalID_1);
            }
        }

        public void addObject(int x, int y, int width, int height, float initialXVelocity, float initialYVelocity, Texture2D texture1, Texture2D texture2)
        {
            _objects.Add(new AABB(x, y, width, height, initialXVelocity, initialYVelocity, texture1, texture2, ++_lastAssignedID));
        }

        public DrawableObject[] getDrawableObjectArray()
        {
            DrawableObject[] returnArray = new DrawableObject[_objects.Count];
            for (int i = 0; i < _objects.Count; i++)
            {
                returnArray[i] = new DrawableObject(new Rectangle((int)_objects[i]._x, (int)_objects[i]._y, (int)_objects[i]._width, (int)_objects[i]._height), _objects[i]._intersectingWith.Count == 0 ? _objects[i]._texture2 : _objects[i]._texture1, Color.White);
            }
            return returnArray;
        }
    }

    internal class AABB
    {
        public float _x, _y, _width, _height, _xVelocity, _yVelocity, _leftEndPoint, _rightEndPoint, _topEndPoint, _bottomEndPoint;
        public Texture2D _texture1, _texture2;
        public int _ID;
        public List<int> _intersectingWith;
        
        public AABB(float x, float y, float width, float height, float initialXVelocity, float initialYVelocity, Texture2D texture1, Texture2D texture2, int ID) 
        {
            float velocityMultiplier = 100;

            _x = x;
            _y = y;
            _leftEndPoint = x;
            _topEndPoint = y;
            _bottomEndPoint = height + y;
            _rightEndPoint = width + x;
            _width = width;
            _height = height;
            _xVelocity = initialXVelocity * velocityMultiplier;
            _yVelocity = initialYVelocity * velocityMultiplier;
            _texture1 = texture1;
            _texture2 = texture2;
            _ID = ID;
            _intersectingWith = new List<int>();
        }

        public void move(Vector2 delta)
        {
            _x += delta.X;
            _y += delta.Y;
            _leftEndPoint = _x;
            _topEndPoint = _y;
            _bottomEndPoint = _height + _y;
            _rightEndPoint = _width + _x;
        }

        public void setPosition(Vector2 position)
        {
            _x = position.X;
            _y = position.Y;
            _leftEndPoint = _x;
            _topEndPoint = _y;
            _bottomEndPoint = _height + _y;
            _rightEndPoint = _width + _x;
        }

        public void update(float deltaTime)
        {
            move(new Vector2(_xVelocity, _yVelocity) * deltaTime);
        }
    }

    internal class DrawableObject
    {
        public Rectangle _rectangle;
        public Texture2D _texture;
        public Color _color;

        public DrawableObject(Rectangle rectangle, Texture2D texture, Color color)
        {
            _rectangle = rectangle;
            _texture = texture;
            _color = color;
        }
    }
}
