using UnityEngine;

namespace Menu
{
    public class Credits : MonoBehaviour
    {
        public void GitHub() => Application.OpenURL("https://github.com/Owl-Man");
        public void Telegram() => Application.OpenURL("https://t.me/fireowlhaven");
        public void PlayMarket() => Application.OpenURL("https://play.google.com/store/apps/developer?id=Fire+Owl");
    }
}
