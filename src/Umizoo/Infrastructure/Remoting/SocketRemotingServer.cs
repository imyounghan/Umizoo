﻿

namespace Umizoo.Infrastructure.Remoting
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;

    using Umizoo.Infrastructure.Socketing;
    using Umizoo.Infrastructure.Socketing.BufferManagement;
    using Umizoo.Infrastructure.Utilities;

    public class SocketRemotingServer
    {
        private readonly ServerSocket _serverSocket;
        private readonly Dictionary<int, IRequestHandler> _requestHandlerDict;
        private readonly IBufferPool _receiveDataBufferPool;
        private readonly SocketSetting _setting;
        private bool _isShuttingdown = false;

        public IBufferPool BufferPool
        {
            get { return _receiveDataBufferPool; }
        }

        public SocketRemotingServer() : this("Server", new IPEndPoint(SocketUtil.GetLocalIPV4(), 5000)) { }
        public SocketRemotingServer(string name, IPEndPoint listeningEndPoint, SocketSetting setting = null)
        {
            _setting = setting ?? new SocketSetting();
            _receiveDataBufferPool = new BufferPool(_setting.ReceiveDataBufferSize, _setting.ReceiveDataBufferPoolSize);
            _serverSocket = new ServerSocket(listeningEndPoint, _setting, _receiveDataBufferPool, HandleRemotingRequest);
            _requestHandlerDict = new Dictionary<int, IRequestHandler>();
        }

        public SocketRemotingServer RegisterConnectionEventListener(IConnectionEventListener listener)
        {
            _serverSocket.RegisterConnectionEventListener(listener);
            return this;
        }
        public SocketRemotingServer Start()
        {
            _isShuttingdown = false;
            _serverSocket.Start();
            return this;
        }
        public SocketRemotingServer Shutdown()
        {
            _isShuttingdown = true;
            _serverSocket.Shutdown();
            return this;
        }
        public SocketRemotingServer RegisterRequestHandler(int requestCode, IRequestHandler requestHandler)
        {
            _requestHandlerDict[requestCode] = requestHandler;
            return this;
        }

        private void HandleRemotingRequest(ITcpConnection connection, byte[] message, Action<byte[]> sendReplyAction)
        {
            if (_isShuttingdown) return;

            var remotingRequest = RemotingUtil.ParseRequest(message);
            var requestHandlerContext = new SocketRequestHandlerContext(connection, sendReplyAction);

            IRequestHandler requestHandler;
            if (!_requestHandlerDict.TryGetValue(remotingRequest.Code, out requestHandler)) {
                var errorMessage = string.Format("No request handler found for remoting request:{0}", remotingRequest);
                LogManager.Default.Error(errorMessage);
                if (remotingRequest.Type != RemotingRequestType.Oneway) {
                    requestHandlerContext.SendRemotingResponse(new RemotingResponse(
                        remotingRequest,
                        -1,
                        Encoding.UTF8.GetBytes(errorMessage),
                        DateTime.Now,
                        null));
                }
                return;
            }

            try {
                var remotingResponse = requestHandler.HandleRequest(requestHandlerContext, remotingRequest);
                if (remotingRequest.Type != RemotingRequestType.Oneway && remotingResponse != null) {
                    requestHandlerContext.SendRemotingResponse(remotingResponse);
                }
            }
            catch (Exception ex) {
                var errorMessage = string.Format("Unknown exception raised when handling remoting request:{0}.", remotingRequest);
                LogManager.Default.Error(errorMessage, ex);
                if (remotingRequest.Type != RemotingRequestType.Oneway) {
                    requestHandlerContext.SendRemotingResponse(new RemotingResponse(
                        remotingRequest,
                        -1,
                        Encoding.UTF8.GetBytes(ex.Message),
                        DateTime.Now,
                        null));
                }
            }
        }
    }
}
