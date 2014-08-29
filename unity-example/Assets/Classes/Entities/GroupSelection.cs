using System.Collections;
using System.Collections.Generic;

namespace ST
{
    public class GroupSelection 
    {
        private List<Entity> selectedEntities;
        private UnityEngine.Rect selection;
        private byte entityBitMask; // mask for type of entities in selection, or allowed in selection
    }
}