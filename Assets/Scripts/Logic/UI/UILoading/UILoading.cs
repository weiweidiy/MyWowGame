
using Cysharp.Threading.Tasks;
using Framework.UI;
using Logic.States.Game;

namespace Logic.UI.UILoading
{
    public class UILoading : UIPage
    {
        private async void Start()
        {
            while (GS_Main.LoadProcess < 1f)
            {
                await UniTask.NextFrame();
            }
            await UniTask.Delay(200);
            Close();
        }
    }
}
