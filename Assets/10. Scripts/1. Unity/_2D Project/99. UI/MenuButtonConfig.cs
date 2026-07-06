using System;
using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    [Serializable]
    public class MenuButtonConfig
    {
        public string Label;
        public RectTransform Target;
        public List<ScenePositionInfo> ScenePositions = new List<ScenePositionInfo>();
    }
}