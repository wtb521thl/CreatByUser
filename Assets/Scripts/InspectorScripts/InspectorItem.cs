using UnityEngine;

namespace Inspector
{
    public abstract class InspectorItem
    {
        protected GameObject oneValue;
        protected GameObject twoValue;
        protected GameObject selectValue;
        protected GameObject selectObj;
        public virtual void Init(Transform contentArea,GameObject _selectObj)
        {
            selectObj = _selectObj;
            oneValue = Resources.Load<GameObject>("Prefabs/InspectorItemType/OneValue");
            twoValue = Resources.Load<GameObject>("Prefabs/InspectorItemType/TwoValue");
            selectValue = Resources.Load<GameObject>("Prefabs/InspectorItemType/SelectValue");
        }
    }
}

