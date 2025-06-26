using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Utils.Assets.Text
{
    public class AddressableTextLoader: ITextLoader
    {
        public async Task <string> LoadAsync(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentException("Text asset address cannot be null or empty");
            
            try
            {
                // Await the task
                var textAsset = await Addressables.LoadAssetAsync<TextAsset>(address).Task;

                if (textAsset == null)
                    throw new FileNotFoundException($"Text asset not found at address: {address}");

                return textAsset.text;
            }
            catch (FileNotFoundException fex)
            {
                Debug.LogError($"[AddressableTextLoader] File not found: {fex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AddressableTextLoader] Error occurred while loading asset: {ex.Message}");
                throw;
            }
        }
    }
}