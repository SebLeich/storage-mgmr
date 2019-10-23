using storage.mgr.backend.Models;
using System;
using wista.backend.Controllers.Socket;

namespace str_mgmr_backend.Core
{
    /// <summary>
    /// the class connects the client and the server vua web socket
    /// </summary>
    public class Hubproxy
    {
        /// <summary>
        /// the field contains the instance of the hubproxy
        /// </summary>
        public static readonly Hubproxy _Instance = new Hubproxy();
        /// <summary>
        /// the constructor creates a new instance of a hub proxy
        /// </summary>
        public Hubproxy() { }
        /// <summary>
        /// the attribute contains the hub to communicate via the web socket
        /// </summary>
        private NotificationHub hub;
        /// <summary>
        /// the method registers a new hub
        /// </summary>
        /// <param name="_Hub"></param>
        public void RegisterHub(NotificationHub hub)
        {
            this.hub = hub;
        }
        /// <summary>
        /// the method registers a task at the current notification hub
        /// </summary>
        /// <returns>guid of the task</returns>
        public Guid RegisterTask()
        {
            return hub.RegisterTask();
        }
        /// <summary>
        /// the method sends a new solution to the client
        /// </summary>
        /// <param name="solution">calculated solution</param>
        public void SendSolution(SolutionModel solution, Guid task)
        {
            if (hub != null)
            {
                hub.SendSolution(solution, task);
            }
        }
        /// <summary>
        /// the method sends an error to the client
        /// </summary>
        /// <param name="solution">calculated solution</param>
        public void SendError(ErrorModel error, Guid task)
        {
            if (hub != null)
            {
                hub.SendError(error, task);
            }
        }
    }
}