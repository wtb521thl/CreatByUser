using UnityEngine;

namespace Tianbo.Wang
{

    public static class MouseEnterAndExit
    {
        static float times = 1;
        public static bool IsMouseEnter(RectTransform rectTransform)
        {
            times = rectTransform.GetComponentInParent<Canvas>().transform.localScale.x;
            bool isEnter = CalculateObjBoundPoints(rectTransform).Contains(Input.mousePosition);
            return isEnter;
        }

        static Bounds CalculateObjBoundPoints(RectTransform rectTransform)
        {
            Bounds objBounds = new Bounds();
            objBounds.center = rectTransform.GetCenter();
            objBounds.size = rectTransform.GetLocalSize() * times;
            GameManager.Instance.DrawBounds(objBounds, Color.yellow);
            return objBounds;
        }

    }
}