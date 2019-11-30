using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Editor.Code.ContextList
{
    public class SimpleReorderableList
    {
    }

    public class ReorderableList<T> : SimpleReorderableList
    {
        public List<T> List;
    }

    [Serializable]
    public class ReorderableContextList : ReorderableList<ContextVo>
    {
    }

 
}