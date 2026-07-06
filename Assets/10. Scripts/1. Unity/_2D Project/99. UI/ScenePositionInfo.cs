using System;
using UnityEngine;

namespace _2DProject
{
    [Serializable]
    public class ScenePositionInfo
    {
        public SceneController.GameScene Scene;
        public bool IsActive = true;
        public Vector2 AnchoredPosition;
    }
}