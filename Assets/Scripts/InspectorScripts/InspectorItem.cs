using UnityEngine;

namespace Inspector
{
    public abstract class InspectorItem
    {

        protected ComponentItem componentItem;
        protected InspectorPanel inspectorPanel;
        protected GameObject selectObj;
        public virtual void Init(Transform contentArea,GameObject _selectObj)
        {
            selectObj = _selectObj;
            componentItem = _selectObj.GetComponent<ComponentItem>();
            inspectorPanel= contentArea.GetComponentInParent<InspectorPanel>();
        }

        public virtual void RefreshValue()
        {

        }

    }
}

