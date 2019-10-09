using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 丛林战争服务端.Model
{
    class Result
    {
        public Result(int id, int userId, int totalCount, int winCount)
        {
            this.Id = id;
            this.UserId = userId;
            this.TotalCount = totalCount;
            this.WinCount = winCount;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalCount { get; set; }
        public int WinCount { get; set; }
    }
}
