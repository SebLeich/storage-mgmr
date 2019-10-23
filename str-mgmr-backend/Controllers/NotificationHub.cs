using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using storage.mgr.backend.Models;
using str_mgmr_backend.Core;

namespace wista.backend.Controllers.Socket
{
    /// <summary>
    /// the hub is the server's endpoint for the web socket
    /// </summary>
    public class NotificationHub : Hub
    {
        /// <summary>
        /// the attribute contains the current connections
        /// </summary>
        public readonly static ConnectionMapping<Guid> _Connections = new ConnectionMapping<Guid>();

        /// <summary>
        /// the constructor creates a new instance of the hub and registers it at the proxy
        /// </summary>
        public NotificationHub()
        {
            Hubproxy._Instance.RegisterHub(this);
        }

        /// <summary>
        /// the method is called when a new client connects to the server
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        /// <summary>
        /// the method is called when the client disconnects from the server
        /// </summary>
        public override Task OnDisconnected(bool stopCalled)
        {

            return base.OnDisconnected(true);
        }

        /// <summary>
        /// the method is called when the client reconnects to the server
        /// </summary>
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        /// <summary>
        /// the method registers a new servers task with the current connection id
        /// </summary>
        /// <returns></returns>
        public Guid RegisterTask()
        {
            Guid id = Guid.NewGuid();
            _Connections.Add(id, Context.ConnectionId);
            return id;
        }

        /// <summary>
        /// the endpoint allows to send solutions
        /// </summary>
        /// [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void SendSolution(SolutionModel solution, Guid task)
        {
            string c = _Connections.GetConnections(task).FirstOrDefault();
            if(c == null)
            {
                return;
            }
            Clients.Client(c).ReceiveSolution(solution);
        }

        /// <summary>
        /// the endpoint allows to send solutions
        /// </summary>
        /// [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void SendError(ErrorModel error, Guid task)
        {
            string c = _Connections.GetConnections(task).FirstOrDefault();
            if (c == null)
            {
                return;
            }
            Clients.Client(c).ReceiveError(error);
        }
    }
}