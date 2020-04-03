using System;
using Microsoft.Xna.Framework;

namespace Occasus.Core.Maths
{
    public delegate float Easer(float t);

    public static class Ease
    {
        public static readonly Easer NullEase = (float t) => { return 0; };
        public static readonly Easer QuadIn = (float t) => { return t * t; };
        public static readonly Easer QuadOut = (float t) => { return 1 - QuadIn(1 - t); };
        public static readonly Easer QuadInOut = (float t) => { return (t <= 0.5f) ? QuadIn(t * 2) / 2 : QuadOut(t * 2 - 1) / 2 + 0.5f; };
        public static readonly Easer CubeIn = (float t) => { return t * t * t; };
        public static readonly Easer CubeOut = (float t) => { return 1 - CubeIn(1 - t); };
        public static readonly Easer CubeInOut = (float t) => { return (t <= 0.5f) ? CubeIn(t * 2) / 2 : CubeOut(t * 2 - 1) / 2 + 0.5f; };
        public static readonly Easer BackIn = (float t) => { return t * t * (2.70158f * t - 1.70158f); };
        public static readonly Easer BackOut = (float t) => { return 1 - BackIn(1 - t); };
        public static readonly Easer BackInOut = (float t) => { return (t <= 0.5f) ? BackIn(t * 2) / 2 : BackOut(t * 2 - 1) / 2 + 0.5f; };
        public static readonly Easer ExpoIn = (float t) => { return (float)Math.Pow(2, 10 * (t - 1)); };
        public static readonly Easer ExpoOut = (float t) => { return 1 - ExpoIn(t); };
        public static readonly Easer ExpoInOut = (float t) => { return t < .5f ? ExpoIn(t * 2) / 2 : ExpoOut(t * 2) / 2; };
        public static readonly Easer SineIn = (float t) => { return -(float)Math.Cos(MathHelper.PiOver2 * t) + 1; };
        public static readonly Easer SineOut = (float t) => { return (float)Math.Sin(MathHelper.PiOver2 * t); };
        public static readonly Easer SineInOut = (float t) => { return -(float)Math.Cos(MathHelper.Pi * t) / 2f + .5f; };
        
        public static readonly Easer Linear = (o) => o;
        //public static readonly Easer QuadIn = (o) => o * o;
        //public static readonly Easer QuadOut = (o) => 1 - QuadIn(1 - o);
        //public static readonly Easer QuadInOut = (o) => (o <= 0.5f) ? QuadIn(o * 2) / 2 : QuadOut(o * 2 - 1) / 2 + 0.5f;
        //public static readonly Easer CubeIn = (o) => o * o * o;
        //public static readonly Easer CubeOut = (o) => 1 - CubeIn(1 - o);
        //public static readonly Easer CubeInOut = (o) => (o <= 0.5f) ? CubeIn(o * 2) / 2 : CubeOut(o * 2 - 1) / 2 + 0.5f;
        //public static readonly Easer BackIn = (o) => o * o * (2.70158f * o - 1.70158f);
        //public static readonly Easer BackOut = (o) => 1 - BackIn(1 - o);
        //public static readonly Easer BackInOut = (o) => (o <= 0.5f) ? BackIn(o * 2) / 2 : BackOut(o * 2 - 1) / 2 + 0.5f;
        //public static readonly Easer ExpoIn = (o) => (float)Math.Pow(2, 10 * (o - 1));
        //public static readonly Easer ExpoOut = (o) => 1 - ExpoIn(o);
        //public static readonly Easer ExpoInOut = (o) => o < .5f ? ExpoIn(o * 2) / 2 : ExpoOut(o * 2) / 2;
        //public static readonly Easer SineIn = (o) => (float)(-Math.Cos(Math.PI / 2 * o) + 1);
        //public static readonly Easer SineOut = (o) => (float)Math.Sin(Math.PI / 2 * o);
        //public static readonly Easer SineInOut = (o) => (float)(-Math.Cos(Math.PI * o) / 2f + .5f);
        public static readonly Easer ElasticIn = (o) => 1 - ElasticOut(1 - o);
        public static readonly Easer ElasticOut = (o) => (float)(Math.Pow(2, -10 * o) * Math.Sin((o - 0.075f) * (2 * Math.PI) / 0.3f) + 1);
        public static readonly Easer ElasticInOut = (o) => (o <= 0.5f) ? ElasticIn(o * 2) / 2 : ElasticOut(o * 2 - 1) / 2 + 0.5f;

        public static Easer FromType(EaseType type)
        {
            switch (type)
            {
                case EaseType.Linear:
                    return Linear;
                case EaseType.QuadIn:
                    return QuadIn;
                case EaseType.QuadOut:
                    return QuadOut;
                case EaseType.QuadInOut:
                    return QuadInOut;
                case EaseType.CubeIn:
                    return CubeIn;
                case EaseType.CubeOut:
                    return CubeOut;
                case EaseType.CubeInOut:
                    return CubeInOut;
                case EaseType.BackIn:
                    return BackIn;
                case EaseType.BackOut:
                    return BackOut;
                case EaseType.BackInOut:
                    return BackInOut;
                case EaseType.ExpoIn:
                    return ExpoIn;
                case EaseType.ExpoOut:
                    return ExpoOut;
                case EaseType.ExpoInOut:
                    return ExpoInOut;
                case EaseType.SineIn:
                    return SineIn;
                case EaseType.SineOut:
                    return SineOut;
                case EaseType.SineInOut:
                    return SineInOut;
                case EaseType.ElasticIn:
                    return ElasticIn;
                case EaseType.ElasticOut:
                    return ElasticOut;
                case EaseType.ElasticInOut:
                    return ElasticInOut;
            }

            return QuadIn;
            //return Linear;
        }
    }
}
