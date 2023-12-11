using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Control;
using Model;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Server
{
    // ReSharper disable once IdentifierTypo
    public class LichServer : Singleton<LichServer>
    {
        [SerializeField] private string serverURL;
        [SerializeField] private int port = 8000;
        [SerializeField] private string url = $"127.0.0.1";
        [SerializeField] private bool serverIsUp;
        [SerializeField] private LoggerUtils logger;

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
                SpawnRandomMusicalCharacter(out _);
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

            do
            {
                try
                {
                    listener = new HttpListener();
                    serverURL = $"{url}:{port}/";
                    logger.LogMessage($"Server URL: {serverURL}");
                    listener.Prefixes.Add(serverURL);
                    listener.Start();
                    logger.LogMessage("Listening for connections on " + serverURL);
                    serverIsUp = true;
                }
                catch (SocketException socket)
                {
                    logger.LogMessage($"Socket error: {socket.Message}");
                    port++;
                    serverIsUp = false;
                }
            } while (!serverIsUp);

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
                                logger.LogMessage($"Ping from {context.Request.UserHostName} - {context.Request.UserHostAddress}.");
                                ReturnMessage("Ping sent successfully.");
                            }
                                break;
                            case "/lastLog":
                            {
                                /*
                                 * curl localhost:8000/lastLog
                                 */
                                ReturnMessage(logger.LastLog());
                            }
                                break;
                            case "/toggleRandom":
                            {
                                /*
                                 * curl localhost:8000/toggleRandom
                                 */
                                var toggle = MusicalControl.GetSingleton().ToggleSpawnRandom();
                                var status = toggle ? "activated" : "deactivated";
                                var message = $"Random character generation {status}.";
                                logger.LogMessage(message);
                                ReturnMessage(message);
                            }
                                break;
                            case "/randomStatus":
                            {
                                /*
                                 * curl localhost:8000/randomStatus
                                 */
                                var toggle = MusicalControl.GetSingleton().IsSpawningRandomly();
                                var status = toggle ? "active" : "deactivated";
                                var message = $"Random character generation is currently {status}.";
                                logger.LogMessage(message);
                                ReturnMessage(message);
                            }
                                break;
                            case "/queueSize":
                            {
                                /*
                                 * curl localhost:8000/queueSize
                                 */
                                var queueSize = MusicalControl.GetSingleton().QueueSize();
                                logger.LogMessage($"Queue size requested: {queueSize}");
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
                                logger.LogMessage($"Random character created: {musicalCharacter}");
                                ReturnMessage(JsonUtility.ToJson(new UIDResponse
                                    { UID = musicalCharacter.UID, queueSize = queueSize }));
                            }
                                break;
                            case "/animateRandomCharacter":
                            {
                                var musicalCharacter = AnimateRandomCharacter(out var animation);
                                logger.LogMessage($"Random character animated: {animation}");
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
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"head":"1", "body":"1", "parameter":"Aulos"}'
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"head":"1", "body":"1", "parameter":"Dulcimer"}'
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"head":"1", "body":"1", "parameter":"Karamuza"}'
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"head":"1", "body":"1", "parameter":"Harp"}'
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"head":"1", "body":"1", "parameter":"Doublebass"}'
                                         curl localhost:8000/queueMusicalCharacter -H 'Content-Type: application/json' -d '{"head":"1", "body":"1", "parameter":"Bendir"}'
                                     */
                                    MessageCheck(() =>
                                    {
                                        //Instantiates a musicalCharacter with the JSON data received from the content.
                                        var musicalCharacter = new MusicalCharacter(content);
                                        var queueSize = MusicalControl.GetSingleton()
                                            .QueueMusicalCharacterSpawning(musicalCharacter);
                                        logger.LogMessage($"POST character created: {musicalCharacter}");
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
                                        logger.LogMessage($"Query character info: {musicalCharacterInfo}");
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
                                        logger.LogMessage($"POST character animated: {content}");
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
                logger.LogMessage(errorMessage);
                ReturnMessage(errorMessage);
            }

            context.Response.Close();

            if (!DEBUG) return;
            logger.LogMessage($"Method {method} - Local Path: {localPath}");
            logger.LogMessage($"Content Type {contentType} - Content: {content}");
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
            var musicalCharacter = MusicalCharacter.GenerateRandomCharacter();
            queueSize = musical.QueueMusicalCharacterSpawning(musicalCharacter);
            return musicalCharacter;
        }

        //Getters
        public bool DebugIsOn() => DEBUG;
    }
}