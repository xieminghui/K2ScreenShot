using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PNGUploader : MonoBehaviour {

    public RenderTexture m_RenderTexture;
    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(UploadPNG());
        }

        if (SystemProperties.get("ids.screenshot","0").Equals("1"))
        {
            StartCoroutine(UploadPNG());
            SystemProperties.set("ids.screenshot","0");
        }

    }

    IEnumerator UploadPNG()
    {
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();
        m_RenderTexture = GetComponent<Camera>().targetTexture;
        RenderTexture.active = m_RenderTexture;
        // Create a texture the size of the screen, RGB24 format
        int width = m_RenderTexture.width;
        int height = m_RenderTexture.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        RenderTexture.active = null;
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
#if UNITY_EDITOR
        string path = Application.dataPath + "/../SavedScreen.png";
#else
        string path = "/sdcard/SavedScreen.png";
#endif
        File.WriteAllBytes(path, bytes);
        Debug.Log("IDS_ Save PNG successed");

        // Create a Web Form
        //WWWForm form = new WWWForm();
        //form.AddField("frameCount", Time.frameCount.ToString());
        //form.AddBinaryData("fileUpload", bytes);

        //// Upload to a cgi script
        //WWW w = new WWW("http://localhost/cgi-bin/env.cgi?post", form);
        //yield return w;

        //if (w.error != null)
        //{
        //    Debug.Log(w.error);
        //}
        //else
        //{
        //    Debug.Log("Finished Uploading Screenshot");
        //}
    }
}
