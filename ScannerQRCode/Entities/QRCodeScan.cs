using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerQRCode.Entities
{
    public class QRCodeScan
    {
        public int Id { get; set; }
        public string QRCodeText { get; set; }
        public DateTime ScanTime { get; set; }
    }

}
