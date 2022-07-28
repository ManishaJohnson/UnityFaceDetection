namespace OpenCvSharp.Demo
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using OpenCvSharp;
    using System;
    using System.IO;

    public class FaceRecog : WebCamera
    {
        public TextAsset faces;
		public TextAsset eyes;
		public TextAsset shapes;

        public GameObject detectfacetxt;

		private FaceProcessorLive<WebCamTexture> processor;

		/// <summary>
		/// Default initializer for MonoBehavior sub-classes
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			base.forceFrontalCamera = true; // we work with frontal cams here, let's force it for macOS s MacBook doesn't state frontal cam correctly

            processor = new FaceProcessorLive<WebCamTexture>();
			processor.Initialize(faces.text, eyes.text, shapes.bytes);

            // data stabilizer - affects face rects, face landmarks etc.
            processor.DataStabilizer.Enabled = true;        // enable stabilizer
			processor.DataStabilizer.Threshold = 2.0;       // threshold value in pixels
			processor.DataStabilizer.SamplesCount = 2;      // how many samples do we need to compute stable data

            // performance data - some tricks to make it work faster
            processor.Performance.Downscale = 256;          // processed image is pre-scaled down to N px by long side
			processor.Performance.SkipRate = 0;             // we actually process only each Nth frame (and every frame for skipRate = 0)
		}

		/// <summary>
		/// Per-frame video capture processor
		/// </summary>
		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
		{
            //Detect face
            FaceDetect(input, ref output);

            //Print on detection of face
            return true;
        }

        protected bool FaceDetect(WebCamTexture input, ref Texture2D output)
        {
            // detect everything we're interested in
            processor.ProcessTexture(input, TextureParameters);

            // mark detected objects
            bool val = processor.MarkDetected(0);

            // if detected object, close camera + print HelloWorld
            if(val)
            {
                input.Stop();

                //Write method to start session
                detectfacetxt.GetComponent<Text>().enabled = true;
            }

            //Debug.Log(processor.MarkDetected().ToString());
            // processor.Image now holds data we'd like to visualize
            output = Unity.MatToTexture(processor.Image, output);   // if output is valid texture it's buffer will be re-used, otherwise it will be re-created

            return true;
        }
    }
    
}
/*

    // Start is called before the first frame update
    //Create an empty texture for layer
    WebCamTexture _webCamTexture;
    public UnityEngine.TextAsset faces;
		
    public TextAsset faces;
	public TextAsset eyes;
	public TextAsset shapes;

    void Start()
    {
        //List of webdevices
        WebCamDevice[] devices = WebCamTexture.devices;

        //using device 0
        _webCamTexture = new WebCamTexture(devices[0].name);

        //Camera starts to play
        _webCamTexture.Play();

        //1. declare cascade class file, put file under Asset and load it 
        //declare a cascade class file which is pretrainied classifier officially provided by openCV 
        /*var xpath = Path.Combine(Application.dataPath, "haarcascade_frontalface_default.xml");
        bool ifexists = File.Exists(xpath);

        cascade = new CascadeClassifier(xpath); Not required 
        
    }

    // Update is called once per frame
    void Update()
    {
        //Use the webcam texture as render texture
        GetComponent<Renderer>().material.mainTexture = _webCamTexture;

        //2. Create a new Mat to store the current frame
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);

        findNewFace(frame);
    }

    void findNewFace(Mat frame)
    {
        //3 We use cascade to decide if there is matching image and then store it as faces else stop loading data
        var faces = cascade.DetectMultiScale(frame, 1, 2, HaarDetectionType.ScaleImage);

        //pass frame to function, and use default scale factor and minimum neighbour
        if(faces.Length >=1)
        {
            Debug.Log(faces[0].Location);
            //MyFace = faces[0];
        }
    }*/




/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using System;
using System.IO;

public class FaceRecog : MonoBehaviour
{
    // Start is called before the first frame update
    //Create an empty texture for layer
    WebCamTexture _webCamTexture;
    CascadeClassifier cascade;
    
    OpenCvSharp.Rect MyFace;
    
    void Start()
    {
        //List of webdevices
        WebCamDevice[] devices = WebCamTexture.devices;

        //using device 0
        _webCamTexture = new WebCamTexture(devices[0].name);

        //Camera starts to play
        _webCamTexture.Play();

        //1. declare cascade class file, put file under Asset and load it 
        //declare a cascade class file which is pretrainied classifier officieally provided by openV 
        var xpath = Path.Combine(Application.dataPath, "haarcascade_frontalface_default.xml");
        bool ifexists = File.Exists(xpath);

        cascade = new CascadeClassifier(xpath);
    }

    // Update is called once per frame
    void Update()
    {
        //Use the webcam texture as render texture
        GetComponent<Renderer>().material.mainTexture = _webCamTexture;

        //2. Create a new Mat to store the current frame
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);

        findNewFace(frame);
    }

    void findNewFace(Mat frame)
    {
        //3 We use cascade to decide if there is matching image and then store it as faces else stop loading data
        var faces = cascade.DetectMultiScale(frame, 1, 2, HaarDetectionType.ScaleImage);

        //pass frame to function, and use default scale factor and minimum neighbour
        if(faces.Length >=1)
        {
            Debug.Log(faces[0].Location);
        }
    }

*/