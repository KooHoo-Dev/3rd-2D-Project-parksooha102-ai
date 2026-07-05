using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public class ModuleInspectorUI : MonoBehaviour
    {
        // 예시: 패널을 열 때나 갱신할 때 호출
        public void DrawInspector()
        {
            List<ModuleInfo> equipped = ModuleDataManager.Instance.EquippedModules;

            foreach (ModuleInfo module in equipped)
            {
                List<ModuleFieldInfo> fields = module.GetEditableFields();

                foreach (ModuleFieldInfo field in fields)
                {
                    // 여기서 슬라이더/인풋필드 실제로 그리고
                    // 값 바뀌면:
                    // module.SetFieldValue(field.Label, 새로운값);
                }
            }
        }
    }
}