using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WH_Physics
{
    internal class WH_Physics
    {
        private List<AABB> _objects;
        private int _screenWidth, _screenHeight;

        public WH_Physics(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _objects = new List<AABB>();
        }

        public void update()
        {
            foreach (AABB obj in _objects)
            {
                // Process Movement
                obj.updatePosition();

                // Process Screen Edge Collisions
                if (obj._rightEndPoint > _screenWidth)
                {
                    obj.
                }

                // Process Inter-AABB Collisions
            }
        }

        public void addObject(int x, int y, int width, int height, float initialXVelocity, float initialYVelocity, Texture2D texture)
        {
            _objects.Add(new AABB(x, y, width, height, initialXVelocity, initialYVelocity, texture));
        }

        public DrawableObject[] getDrawableObjectArray()
        {
            DrawableObject[] returnArray = new DrawableObject[_objects.Count];
            for (int i = 0; i < _objects.Count; i++)
            {
                returnArray[i] = new DrawableObject(new Rectangle((int)_objects[i]._x, (int)_objects[i]._y, (int)_objects[i]._width, (int)_objects[i]._height), _objects[i]._texture);
            }
            return returnArray;
        }
    }

    internal class AABB
    {
        public float _x, _y, _width, _height, _xVelocity, _yVelocity, _leftEndPoint, _rightEndPoint, _topEndPoint, _bottomEndPoint;
        public Texture2D _texture;
        
        public AABB(float x, float y, float width, float height, float initialXVelocity, float initialYVelocity, Texture2D texture) 
        {
            _x = x = _leftEndPoint;
            _y = y = _topEndPoint;
            _bottomEndPoint = texture.Height + y;
            _rightEndPoint = texture.Width + x;
            _width = width;
            _height = height;
            _xVelocity = initialXVelocity;
            _yVelocity = initialYVelocity;
            _texture = texture;
        }



    }

    internal class DrawableObject
    {
        public Rectangle _rectangle;
        public Texture2D _texture;
        public bool _colliding;

        public DrawableObject(Rectangle rectangle, Texture2D texture)
        {
            _rectangle = rectangle;
            _texture = texture;
            _colliding = false;
        }
    }
}
