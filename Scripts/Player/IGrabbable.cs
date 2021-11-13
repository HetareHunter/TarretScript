using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    /// <summary>
    /// このインターフェースを実装するクラスは握ることができるオブジェクトとする
    /// </summary>
    public interface IGrabbable
    {
        void GrabBegin(OVRInput.Controller controller,Transform transform);
        void GrabEnd();
    }
}