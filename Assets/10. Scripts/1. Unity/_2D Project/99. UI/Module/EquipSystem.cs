using Jay;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _2DProject
{
    public class EquipSystem : SingletonBase<EquipSystem>
    {
        public enum EquipType
        {
            Equip,
            Unequip,
        }

        public struct EquipInfo
        {
            public EquipType EquipType;
            public ModuleSlot Start;
            public ModuleSlot End;
            public ModuleInfo Item;
        }

        [Header("PopUp Ref")]
        public GameObject Root;
        public TMP_Text ContentText;
        public Button YesButton;
        public Button NoButton;

        private bool isClick = false;
        private bool isPositiveClick = false;
        public bool IsTrading { get; private set; }


        public void RequestTradeEvent(EquipInfo info)
        {
            StartCoroutine(TriggerTradeEvent(info));
        }

        private IEnumerator TriggerTradeEvent(EquipInfo info)
        {
            if (info.Item == null)
            {
                yield break;
            }

            IsTrading = true;
            Root.SetActive(true);
            string tradeWord = (info.EquipType == EquipType.Equip) ? "equip" : "unequip";

            ContentText.SetText($"Would you like to {tradeWord} {info.Item.Description}?");

            while (true)
            {
                if (isClick) break;
                yield return null;
            }

            isClick = false; 
            Root.SetActive(false);
            IsTrading = false;

            if (isPositiveClick == false)
            {
                RollBackSlot(info.Start, info.End);
                yield break;
            }

            bool isSuccess = false;

            switch (info.EquipType)
            {
                case EquipType.Equip:
                    isSuccess = true;
                    ModuleDataManager.Instance.Equip(info.Item);
                    break;

                case EquipType.Unequip:
                    isSuccess = true;
                    ModuleDataManager.Instance.Unequip(info.Item);
                    break;
            }

            if (isSuccess == false) RollBackSlot(info.Start, info.End);
        }

        private void RollBackSlot(ModuleSlot start, ModuleSlot end)
        {
            start.SetItem(end.Icon);
            end.SetItem(null);
        }

        public void OnClickButton(bool isPositive)
        {
            isClick = true;
            isPositiveClick = isPositive;
        }

    }
}


