using System.Threading.Tasks;

namespace Game.Utils.Assets.Text
{
    public interface ITextLoader
    {
        Task<string> LoadAsync(string path);
    }
}