using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace WH_Physics
{
    internal class WH_Physics
    {
        private List<AABB> _objects;

        public WH_Physics()
        {
            _objects = new List<AABB>();
        }

        public void update()
        {
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
        public float _x, _y, _width, _height, _xVelocity, _yVelocity;
        public Texture2D _texture;
        public AABB(float x, float y, float width, float height, float initialXVelocity, float initialYVelocity, Texture2D texture) 
        {
            _x = x;
            _y = y;
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
