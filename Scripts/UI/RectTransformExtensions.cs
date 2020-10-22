using System.Collections.Generic;
using UnityEngine;

namespace HegaCore.UI
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Warning: Does not work if the rectangles are rotated
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="self"></param>
        /// <param name="viewport">If viewport is screen, then keep it as 'null'</param>
        /// <returns></returns>
        public static bool IsOverlapped(this RectTransform self, Camera camera, RectTransform viewport = null)
        {
            Vector2 viewportMinCorner;
            Vector2 viewportMaxCorner;

            if (viewport)
            {
                // so that we don't have to traverse the entire parent hierarchy (just to get screen coords relative to screen),
                // ask the camera to do it for us.
                // first get world corners of our rect:
                var corners = Pool.Provider.Array1<Vector3>(4);
                viewport.GetWorldCorners(corners); // bottom left, top left, top right, bottom right

                // or shove it back into screen space. Now the rect is relative to the bottom left corner of screen:
                viewportMinCorner = camera.WorldToScreenPoint(corners[0]);
                viewportMaxCorner = camera.WorldToScreenPoint(corners[2]);

                Pool.Provider.Return(corners);
            }
            else
            {
                // just use the scren as the viewport
                viewportMinCorner = new Vector2(0, 0);
                viewportMaxCorner = new Vector2(Screen.width, Screen.height);
            }

            // give 1 pixel border to avoid numeric issues:
            viewportMinCorner -= Vector2.one;
            viewportMaxCorner += Vector2.one;

            // do a similar procedure, to get the "element's" corners relative to screen:
            var selfCorners = Pool.Provider.Array1<Vector3>(4);
            self.GetWorldCorners(selfCorners);

            Vector2 selfMinCorner = camera.WorldToScreenPoint(selfCorners[0]);
            Vector2 selfMaxCorner = camera.WorldToScreenPoint(selfCorners[2]);

            Pool.Provider.Return(selfCorners);

            if (selfMinCorner.x > viewportMaxCorner.x) { return false; } // completelly outside to the right
            if (selfMinCorner.y > viewportMaxCorner.y) { return false; } // completelly above

            if (selfMaxCorner.x < viewportMinCorner.x) { return false; } // completelly outside to the left
            if (selfMaxCorner.y < viewportMinCorner.y) { return false; } // completelly below

            // commented out, but use it if need to check if element is completely inside:
            //var minDif = viewportMinCorner - selfMinCorner;
            //var maxDif = viewportMaxCorner - selfMaxCorner;

            //if (minDif.x < 0  &&  minDif.y < 0  &&  maxDif.x > 0  &&maxDif.y > 0) { //return "is completely inside" }

            // passed all checks, is inside (at least partially)
            return true;
        }

        /// <summary>
        /// Warning: Does not work if the rectangles are rotated
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="self"></param>
        /// <param name="viewport">If viewport is screen, then keep it as 'null'</param>
        /// <returns></returns>
        public static Vector2 CalcOutsideOffset(this RectTransform self, Camera camera, RectTransform viewport = null)
        {
            Vector2 viewportMinCorner;
            Vector2 viewportMaxCorner;

            if (viewport)
            {
                // so that we don't have to traverse the entire parent hierarchy (just to get screen coords relative to screen),
                // ask the camera to do it for us.
                // first get world corners of our rect:
                var corners = Pool.Provider.Array1<Vector3>(4);
                viewport.GetWorldCorners(corners); // bottom left, top left, top right, bottom right

                // or shove it back into screen space. Now the rect is relative to the bottom left corner of screen:
                viewportMinCorner = camera.WorldToScreenPoint(corners[0]);
                viewportMaxCorner = camera.WorldToScreenPoint(corners[2]);

                Pool.Provider.Return(corners);
            }
            else
            {
                // just use the scren as the viewport
                viewportMinCorner = new Vector2(0f, 0f);
                viewportMaxCorner = new Vector2(Screen.width, Screen.height);
            }

            // give 1 pixel border to avoid numeric issues:
            viewportMinCorner -= Vector2.one;
            viewportMaxCorner += Vector2.one;

            // do a similar procedure, to get the "element's" corners relative to screen:
            var selfCorners = Pool.Provider.Array1<Vector3>(4);
            self.GetWorldCorners(selfCorners);

            Vector2 selfMinCorner = camera.WorldToScreenPoint(selfCorners[0]);
            Vector2 selfMaxCorner = camera.WorldToScreenPoint(selfCorners[2]);

            Pool.Provider.Return(selfCorners);

            var size = selfMaxCorner - selfMinCorner;
            var minDif = viewportMinCorner - selfMinCorner;
            var maxDif = viewportMaxCorner - selfMaxCorner;
            var offset = Vector2.zero;

            // completely inside
            if (minDif.x < 0f && minDif.y < 0f && maxDif.x > 0f && maxDif.y > 0f)
                return offset;

            // completelly outside to the right
            if (selfMinCorner.x >= viewportMaxCorner.x)
                offset.x = -size.x;
            else
            // completelly outside to the left
            if (selfMaxCorner.x <= viewportMinCorner.x)
                offset.x = size.x;
            else
            // partially outside to the right
            if (selfMaxCorner.x >= viewportMaxCorner.x)
                offset.x = viewportMaxCorner.x - selfMaxCorner.x;
            else
            // partially outside to the left
            if (selfMinCorner.x <= viewportMinCorner.x)
                offset.x = -(viewportMinCorner.x - selfMinCorner.x);

            // completelly above
            if (selfMinCorner.y >= viewportMaxCorner.y)
                offset.y = -size.y;
            else
            // completelly below
            if (selfMaxCorner.y <= viewportMinCorner.y)
                offset.y = size.y;
            else
            // partially above
            if (selfMaxCorner.y >= viewportMaxCorner.y)
                offset.y = viewportMaxCorner.y - selfMaxCorner.y;
            else
            // partially below
            if (selfMinCorner.y <= viewportMinCorner.y)
                offset.y = -(viewportMinCorner.y - selfMinCorner.y);

            return offset;
        }

        public static void GetWorldSize(this RectTransform self, out Vector2 size, out Vector2 origin)
        {
            // Convert the rectangle to world corners and grab the top left
            var corners = Pool.Provider.Array1<Vector3>(4);
            self.GetWorldCorners(corners);

            var bl = corners[0];
            var tr = corners[2];

            size = new Vector2(Mathf.Abs(tr.x - bl.x), Mathf.Abs(tr.y - bl.y));
            origin = Vector2.Lerp(bl, tr, 0.5f);

            Pool.Provider.Return(corners);
        }
    }
}
