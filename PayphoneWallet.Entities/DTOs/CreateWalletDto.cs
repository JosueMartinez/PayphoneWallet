using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayphoneWallet.Entities.DTOs
{
    public class CreateWalletDto
    {
        public string DocumentId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal InitialBalance { get; set; }
    }
}
