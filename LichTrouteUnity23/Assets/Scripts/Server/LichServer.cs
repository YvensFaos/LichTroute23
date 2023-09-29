using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using Utils;

namespace Server
{
    public class LichServer : Utils.Singleton<LichServer>
    {
        [SerializeField] private string serverURL;
        [SerializeField] private int port = 8000;
        [SerializeField] private string url = $"127.0.0.1";
        [SerializeField] private bool serverIsUp;
        [SerializeField] private float lastTimeRequest;

        [SerializeField, TextArea(15,45)]
        private string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>HttpListener Example</title>" +
            "  </head>" +
            "  <body>" +
            "    <p>Page Views: {0}</p>" +
            "    <form method=\"post\" action=\"shutdown\">" +
            "      <input type=\"submit\" value=\"Shutdown\" {1}>" +
            "    </form>" +
            "  </body>" +
            "</html>";

        private HttpListener listener;
        private Thread serverThread;

        private Queue<HttpListenerContext> _contextQueue;

        [SerializeField]
        private bool DEBUG;

        private void Start()
        {
            InitializeServer();
        }

        private void InitializeServer()
        {
            listener = new HttpListener();
            serverURL = $"{url}:{port}/";
            Debug.Log($"Server URL: {serverURL}");
            listener.Prefixes.Add(serverURL);
            listener.Start();
            Debug.Log("Listening for connections on " + serverURL);
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
                var result = listener.BeginGetContext (ServerCallback, listener);
                result.AsyncWaitHandle.WaitOne ();
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
            var context = listener.EndGetContext (result);
            _contextQueue.Enqueue(context);
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
                Debug.Log($"Method {method} - Local Path: {localPath}");
                Debug.Log($"Content Type {contentType} - Content: {content}");
            }
        
            try
            {
                switch (method)
                {
                    case "GET": //REST
                    {
                        switch (localPath)
                        {
                            case "/ping":
                            {
                                /*
                             * curl https://fishyfishmcfish.eu.ngrok.io/ping
                             */
                                DebugUtils.DebugLogMsg($"Ping from {context.Request.UserHostName} - {context.Request.UserHostAddress}.");
                                ReturnMessage("Ping sent successfully.");
                            }
                                break;
                        }
                    }
                        break;
                    case "POST":
                    {
                        switch (localPath)
                        {
                            case "/random":
                            {
                                /*
                                 * curl -X POST http://fishyfishmcfish.eu.ngrok.io/randomFish -H 'Content-Type: application/json' -d '{"count":10}'
                                 */
                                if (contentType != null && contentType.Equals("application/json"))
                                {

                                }
                            }
                                break;
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
    
        //Getters
        public float LastTimeRequest => lastTimeRequest;
        public bool DebugIsOn() => DEBUG;
    }
}