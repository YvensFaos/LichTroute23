using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Control;
using Model;
using UnityEngine;
using Utils;

namespace Server
{
    // ReSharper disable once IdentifierTypo
    public class LichServer : Singleton<LichServer>
    {
        [SerializeField] private string serverURL;
        [SerializeField] private int port = 8000;
        [SerializeField] private string url = $"127.0.0.1";
        [SerializeField] private bool serverIsUp;

        private HttpListener listener;
        private Thread serverThread;

        private Queue<HttpListenerContext> _contextQueue;
        private static Mutex mutex;
        
        [SerializeField]
        private bool DEBUG;

        private void Start()
        {
            InitializeServer();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.O))
            {
                var musical = MusicalControl.GetSingleton();
                var musicalCharacter = SpawnRandomMusicalCharacter(out _);
                musical.QueueMusicalCharacterSpawning(musicalCharacter);
            }

            if (Input.GetKeyUp(KeyCode.P))
            {
                var musicalCharacter = AnimateRandomCharacter(out var animation);
                DebugUtils.DebugLogMsg($"Animate: {musicalCharacter.name} - animation: {animation}.");
            }
        }

        private void InitializeServer()
        {
            mutex = new Mutex();
            
            listener = new HttpListener();
            serverURL = $"{url}:{port}/";
            DebugUtils.DebugLogMsg($"Server URL: {serverURL}");
            listener.Prefixes.Add(serverURL);
            listener.Start();
            DebugUtils.DebugLogMsg("Listening for connections on " + serverURL);
            serverIsUp = true;
        
            serverThread = new Thread (StartServerThread);
            serverThread.Start();
        
            _contextQueue = new Queue<HttpListenerContext>();
            StartCoroutine(ServerCoroutine());
        }

        private void StartServerThread()
        {
            while (serverIsUp)
            {
                var result = listener.BeginGetContext(ServerCallback, listener);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private IEnumerator ServerCoroutine()
        {
            while (serverIsUp)
            {
                yield return new WaitUntil(() => _contextQueue.Count > 0);
                while (_contextQueue.Count > 0)
                {
                    ResolveContext(_contextQueue.Dequeue());
                }
            }
        }
    
        private void ServerCallback(IAsyncResult result)
        {
            var context = listener.EndGetContext(result);
            mutex.WaitOne();
            _contextQueue.Enqueue(context);
            mutex.ReleaseMutex();
        }

        private void ResolveContext(HttpListenerContext context)
        {
            void ReturnMessage(string content)
            {
                context.Response.ContentType = "text/plain";
                var bytes = System.Text.Encoding.UTF8.GetBytes(content);
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }

            var method = context.Request.HttpMethod;
            var localPath = context.Request.Url.LocalPath;

            var contentType = context.Request.ContentType;
            string content;
            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                content = reader.ReadToEnd();
            }
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
            context.Response.AppendHeader("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            context.Response.AppendHeader("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
            context.Response.StatusCode = 200;
        
            if (DEBUG)
            {
                DebugUtils.DebugLogMsg($"Method {method} - Local Path: {localPath}");
                DebugUtils.DebugLogMsg($"Content Type {contentType} - Content: {content}");
            }
            
            void MessageCheck(Func<string> onSuccess)
            {
                context.Response.ContentType = "application/json";
                var responseJson = onSuccess();
                var bytes = System.Text.Encoding.UTF8.GetBytes(responseJson);
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
        
            try
            {
                switch (method)
                {
                    case "GET":
                    {
                        switch (localPath)
                        {
                            case "/ping":
                            {
                                /*
                                 * curl localhost:8000/ping
                                 */
                                DebugUtils.DebugLogMsg($"Ping from {context.Request.UserHostName} - {context.Request.UserHostAddress}.");
                                ReturnMessage("Ping sent successfully.");
                            }
                                break;
                            case "/queueSize":
                            {
                                /*
                                 * curl localhost:8000/queueSize
                                 */
                                var queueSize = MusicalControl.GetSingleton().QueueSize();
                                ReturnMessage(queueSize.ToString());
                            }
                                break;
                            case "/queueRandomCharacter":
                            {
                                /*
                                 * curl localhost:8000/queueRandomCharacter
                                 * curl panfun.ngrok.io/queueRandomCharacter
                                 */
                                var musicalCharacter = SpawnRandomMusicalCharacter(out var queueSize);
                                ReturnMessage(JsonUtility.ToJson(new UIDResponse
                                    { UID = musicalCharacter.UID, queueSize = queueSize }));
                            }
                                break;
                            case "/animateRandomCharacter":
                            {
                                var musicalCharacter = AnimateRandomCharacter(out var animation);
                                ReturnMessage(JsonUtility.ToJson(new UIDAnimate()
                                    { UID = musicalCharacter.UID, action = animation }));
                            }
                                break;
                        }
                    }
                        break;
                    case "POST":
                    {
                        if (contentType is "application/json")
                        {
                            switch (localPath)
                            {
                                case "/queueMusicalCharacter":
                                {
                                    /*
                                     * Examples:
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"character":"bob", "parameter":"Aulos", "value":1.0}'
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"character":"bob", "parameter":"Dulcimer", "value":1.0}'
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"character":"bob", "parameter":"Karamuza", "value":1.0}'
                                         
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"character":"bob", "parameter":"Harp", "value":1.0}'
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"character":"bob", "parameter":"DoubleBass", "value":1.0}'
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"character":"bob", "parameter":"Bendir", "value":1.0}'
                                     */
                                    MessageCheck(() =>
                                    {
                                        //Instantiates a musicalCharacter with the JSON data received from the content.
                                        var musicalCharacter = new MusicalCharacter(content);
                                        var queueSize = MusicalControl.GetSingleton()
                                            .QueueMusicalCharacterSpawning(musicalCharacter);
                                        return JsonUtility.ToJson(new UIDResponse
                                            { UID = musicalCharacter.UID, queueSize = queueSize });
                                    });
                                }
                                    break;
                                case "/getCharacterInfo":
                                {
                                    /*
                                     * curl localhost:8000/getCharacterInfo -H 'Content-Type: application/json' -d '{"UID":"f75ce180-4bc2-4e5e-bd2e-7f8f49ecb304"}'
                                     */
                                    MessageCheck(() =>
                                    {
                                        //Queries the information for a character
                                        var musicalCharacterInfo = MusicalControl.GetSingleton().GetCharacterInfo(content);
                                        return JsonUtility.ToJson(musicalCharacterInfo);
                                    });
                                }
                                    break;
                                case "/animateCharacter":
                                {
                                    /*
                                     * curl localhost:8000/animateCharacter -H 'Content-Type: application/json' -d '{"UID":"f75ce180-4bc2-4e5e-bd2e-7f8f49ecb304", "action":1}'
                                     */
                                    MessageCheck(() =>
                                    {
                                        //Queries the information for a character
                                        var result = MusicalControl.GetSingleton().AnimateCharacterWithJson(content);
                                        return $"{{'animate'={(result ? 1 : 0)}}}";
                                    });
                                }
                                    break;
                            }
                        }
                    }
                        break;

                }
            }
            catch (Exception e)
            {
                var errorMessage = $"Something went wrong with the spawning. Error message was: {e.Message}.";
                DebugUtils.DebugLogErrorMsg(errorMessage);
                ReturnMessage(errorMessage);
            }

            context.Response.Close();
        }

        private static MusicalCharacterBehaviour AnimateRandomCharacter(out int animation)
        {
            var musical = MusicalControl.GetSingleton();
            var musicalCharacter = musical.RandomWaitingCharacter();
            animation = musicalCharacter.AnimateRandomly();
            return musicalCharacter;
        }

        private static MusicalCharacter SpawnRandomMusicalCharacter(out int queueSize)
        {
            var musical = MusicalControl.GetSingleton();
            var musicalCharacter = new MusicalCharacter("bob", musical.RandomInstrument(), 0.0f);
            queueSize = musical.QueueMusicalCharacterSpawning(musicalCharacter);
            return musicalCharacter;
        }

        //Getters
        public bool DebugIsOn() => DEBUG;
    }
}