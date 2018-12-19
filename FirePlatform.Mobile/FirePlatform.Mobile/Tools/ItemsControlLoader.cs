using System;
namespace FirePlatform.Mobile.Tools
{
    public class ItemsControlLoader
    {
        #region singletone
        private static ItemsControlLoader _itemsControlLoader;
        private ItemsControlLoader()
        {
        }
        public static ItemsControlLoader Intance()
        {
            if (_itemsControlLoader == null)
                _itemsControlLoader = new ItemsControlLoader();
            return _itemsControlLoader;
        }
        #endregion singletone
    }
}
