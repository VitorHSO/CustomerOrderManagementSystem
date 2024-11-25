using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMS.Application.DTOs.Transaction
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
