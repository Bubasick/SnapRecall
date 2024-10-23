using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapRecall.Application.Features.Users.UpdateUserCommand
{
    public class UpdateUserCommand : IRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string LastExecutedCommand { get; set; }
    }
}
