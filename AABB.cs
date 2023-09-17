using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DISABLED
{
    public class IntervalManager
    {
        private Interval[] _intervalList;
        private Endpoint[] _endpointList;
        private Pair[] _pairList;
        private int[] _lookup;

        public IntervalManager(Interval[] intervals)
        {
            _intervalList = intervals;
            _endpointList= new Endpoint[2*_intervalList.Count()];

            for (int i = 0; i < _intervalList.Count(); i++)
            {
                _endpointList[2 * i]._type = Endpoint.Type.BEGIN;
                _endpointList[2 * i]._value = _intervalList[i]._end[0];
                _endpointList[2 * i]._index = i;

                _endpointList[2 * i + 1]._type = Endpoint.Type.END;
                _endpointList[2 * i + 1]._value = _intervalList[i]._end[1];
                _endpointList[2 * i + 1]._index = i;
            }

            // TODO Sort(_endpointList);
        }

        public void Set(int i, Interval interval)
        {
            _intervalList[i] = interval;
            _endpointList[_lookup[i * 2]]._value = interval._end[0];
            _endpointList[_lookup[i * 2]]._value = interval._end[1];
        }

        public Interval Get(int i)
        {
            return new Interval();
        }

        public void Update()
        {

        }

        public Pair[] GetOverlaps()
        {
            return new Pair[0];
        }
    }

    public struct Endpoint
    {
        public enum Type { BEGIN = 0, END = 1 };
        public Type _type;
        public float _value;
        public int _index;
    }

    public struct Interval
    {
        public float[] _end;
        public Interval()
        {
            _end = new float[2];
        }
    }

    public class Pair
    {
        public int _Interval0, _Interval1;

        public Pair(int interval0, int interval1)
        {
            if (interval0 < interval1)
            {
                _Interval0 = interval0;
                _Interval1 = interval1;
            }
            else
            {
                _Interval0 = interval1;
                _Interval1 = interval0;
            }
        }
    }

    internal class AABB
    {
    }
}
