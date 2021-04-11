//using System.Drawing;
//using System.Drawing.Imaging;
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;


//public class AnimatedGifDrawer : SingletonObject<AnimatedGifDrawer>
//{
//    public float speed = 5;
//    private string loadingGifPath;
 
//    private UnityEngine.UI.RawImage raw;
//    private List<Texture2D> gifFrames = new List<Texture2D>();



//    public void PlayGif(string name,RawImage raw)
//    {
//        this.raw = raw;

//        loadingGifPath = Application.streamingAssetsPath+ "/Gif/" + name;


//        try
//        {
//            System.Drawing.Image gifImage = System.Drawing.Image.FromFile(loadingGifPath);
//            var dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);

//            int frameCount = gifImage.GetFrameCount(dimension);

//            for (int i = 0; i < frameCount; i++)
//            {
//                gifImage.SelectActiveFrame(dimension, i);
//                var frame = new Bitmap(gifImage.Width, gifImage.Height);
//                System.Drawing.Graphics.FromImage(frame).DrawImage(gifImage, Point.Empty);
//                var frameTexture = new Texture2D(frame.Width, frame.Height);
//                for (int x = 0; x < frame.Width; x++)
//                {
//                    for (int y = 0; y < frame.Height; y++)
//                    {
//                        System.Drawing.Color sourceColor = frame.GetPixel(x, y);
//                        frameTexture.SetPixel(frame.Width - 1 - x, y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for some reason, x is flipped  
//                                                                                                                                                //frameTexture.SetPixel(frame.Width - 1 - x+1, y+1, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for some reason, x is flipped  
//                    }
//                }
//                frameTexture.Apply();
//                gifFrames.Add(frameTexture);
//            }

//            for (int i = 0; i < gifFrames.Count; i++)
//            {
//                raw.texture = gifFrames[i];
//            }
//        }
//        catch (System.Exception ex)
//        {
//            Debug.LogError(ex);
//        }
       

       
//    }




//    private void Update()
//    {
//        //if (Input.GetKeyDown(KeyCode.P))
//        //{
//        //    //播放一次  
//        //    StartCoroutine(PlayGif());
//        //}
//        //循环播放  
//        //raw.texture = gifFrames[(int)((Time.frameCount * speed) % gifFrames.Count)];
//    }

//}
