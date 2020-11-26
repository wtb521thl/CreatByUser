using UnityEngine;

namespace Tianbo.Wang
{
    public interface IOutLine
    {
        void Init(GameObject go);
        void RefreshRect(float lineWidth, Color lineColor);
        void DestroySelf();
    }
}