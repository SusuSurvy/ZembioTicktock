namespace UnityEngine.UI
{
    public abstract class LoopScrollRectMulti : LoopScrollRectBase
    {
        // Multi Data Source cannot support TempPool
        protected override RectTransform GetFromTempPool(int itemIdx)
        {
            RectTransform nextItem = OnItemGive.Invoke(itemIdx);
            nextItem.transform.SetParent(m_Content, false);
            nextItem.gameObject.SetActive(true);

            OnItemUpdate.Invoke(nextItem, itemIdx);
            return nextItem;
        }

        protected override void ReturnToTempPool(bool fromStart, int count)
        {
            Debug.Assert(m_Content.childCount >= count);
            if (fromStart)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    OnItemReturn.Invoke(m_Content.GetChild(i));
                }
            }
            else
            {
                int t = m_Content.childCount - count;
                for (int i = m_Content.childCount - 1; i >= t; i--)
                {
                    OnItemReturn.Invoke(m_Content.GetChild(i));
                }
            }
        }

        protected override void ClearTempPool()
        {
        }
    }
}