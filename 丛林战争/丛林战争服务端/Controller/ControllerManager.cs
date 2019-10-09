using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using 丛林战争服务端.Servers;

namespace 丛林战争服务端.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }
        void InitController()
        {
            DefaultController defaultController= new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
            UserController userController = new UserController();
            controllerDict.Add(userController.RequestCode, userController);
            RoomController roomController = new RoomController();
            controllerDict.Add(roomController.RequestCode,roomController);
            GameController gameController = new GameController();
            controllerDict.Add(gameController.RequestCode,gameController);
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode,string data,Client client)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if (isGet == false)
            {
                Console.WriteLine("["+requestCode+"]无法获取controller,请求被拒绝");
            }
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("[警告]在Controller["+ controller.GetType() + "]中没有对应的处理方法:"+ methodName);
            }
            object[] parameters = new object[] {data,client,server};
            object o= mi.Invoke(controller, parameters);
            if (String.IsNullOrEmpty(o as string))
            {
                return;
            }
            server.SendResponse(client,actionCode,o as string);
        }
    }
}
