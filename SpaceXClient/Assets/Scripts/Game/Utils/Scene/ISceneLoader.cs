using System.Threading.Tasks;

namespace Game.Utils.Scene
{
    public interface ISceneLoader
    {
        public Task LoadSceneAsync(string sceneName);
    }
}