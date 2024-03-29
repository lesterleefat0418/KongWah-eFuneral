using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SFB;

public class PeoplePhotoLoader : MonoBehaviour
{
    public string getImageUrl = "http://localhost/kongwahServer/getPhoto.php";
    public RawImage image;
    private string currentImageUrl;

    IEnumerator Start()
    {
        while (true)
        {
            // Create a UnityWebRequest to get the image URL
            using (UnityWebRequest www = UnityWebRequest.Get(getImageUrl))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Failed to get image URL: " + www.error);
                }
                else
                {
                    string imageUrl = www.downloadHandler.text;

                    // Check if a new image URL is received
                    if (!string.IsNullOrEmpty(imageUrl) && imageUrl != currentImageUrl)
                    {
                        currentImageUrl = imageUrl;
                        yield return LoadImage(imageUrl);
                    }
                }
            }

            // Wait for a certain amount of time before checking again
            yield return new WaitForSeconds(1f);
        }
    }

    public Texture2D rotateTexture(Texture2D image)
    {

        Texture2D target = new Texture2D(image.height, image.width, image.format, false);    //flip image width<>height, as we rotated the image, it might be a rect. not a square image

        Color32[] pixels = image.GetPixels32(0);
        pixels = rotateTextureGrid(pixels, image.width, image.height);
        target.SetPixels32(pixels);
        target.Apply();

        //flip image width<>height, as we rotated the image, it might be a rect. not a square image

        return target;
    }


    public Color32[] rotateTextureGrid(Color32[] tex, int wid, int hi)
    {
        Color32[] ret = new Color32[wid * hi];      //reminder we are flipping these in the target

        for (int y = 0; y < hi; y++)
        {
            for (int x = 0; x < wid; x++)
            {
                //ret[(hi - 1) - y + x * hi] = tex[x + y * wid];         //juggle the pixels around
                ret[y + (wid - 1 - x) * hi] = tex[x + y * wid];

            }
        }

        return ret;
    }


    public void LoadImageFromDesktop()
    {
        var extensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" )
            };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select Image", "", extensions, false);

        if (paths.Length > 0)
        {
            string path = paths[0];

            // Load and display the image
            StartCoroutine(LoadImage(path));
        }
    }

    public void ResetPhoto()
    {
        if (this.image != null)
        {
            this.image.texture = null;
            AspectRatioFitter ratioFitter = this.image.GetComponent<AspectRatioFitter>();
            ratioFitter.aspectRatio = 1f;
        }
    }

    private IEnumerator LoadImage(string path)
    {
        using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(path))
        {
            yield return imageRequest.SendWebRequest();

            if (imageRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Failed to download image: " + imageRequest.error);
            }
            else
            {
                Debug.Log("downloaded image");
                // Get the downloaded texture
                Texture2D originalTexture = DownloadHandlerTexture.GetContent(imageRequest);

                // Apply the texture to the image renderer
                if (this.image != null && originalTexture != null)
                {
                    Debug.Log(originalTexture.width + ";" + originalTexture.height);
                    float ratio = (float)originalTexture.width / (float)originalTexture.height;
                    Debug.Log("ratio: " + ratio);
                    AspectRatioFitter ratioFitter = this.image.GetComponent<AspectRatioFitter>();
                    ratioFitter.aspectRatio = ratio;
                    this.image.texture = originalTexture;
                }
            }
        }
    }
}
