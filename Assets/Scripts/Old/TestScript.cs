using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Firebase.Firestore;
using System.IO;

public class TestScript : MonoBehaviour
{
    string pngType = NativeFilePicker.ConvertExtensionToFileType("png");
    string jpgType = NativeFilePicker.ConvertExtensionToFileType("jpg");

    [SerializeField] Image displayImage;

    public void SelectFile()
    {
        RequestPermissionAsynchronously(); //Request for file permission on android

        NativeFilePicker.PickFile(CheckImage, pngType, jpgType);
    }

    public void CheckImage(string path)
    {
        if (path == null)
        {
            //Nothing was picked
        }
        else
        {
            byte[] imageData = File.ReadAllBytes(path); //Load Image Bytes
            Texture2D texture = new Texture2D(2,2);

            if (ImageConversion.LoadImage(texture, imageData) != true)
            {
                //Inavlid Image
            }
            else //Valid Image
            {
                displayImage.sprite = Sprite.Create(texture,
                                new Rect(Vector2.zero, new Vector2(texture.width, texture.height)),
                                Vector2.zero);
                displayImage.preserveAspect = true;
            }
        }
    }

    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
        Debug.Log("Permission result: " + permission);
    }
}
