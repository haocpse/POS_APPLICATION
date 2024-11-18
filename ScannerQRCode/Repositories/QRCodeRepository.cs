using ScannerQRCode.Entities;

namespace ScannerQRCode.Repositories
{
    public class QRCodeRepository 
    {
        private QRContext _context = new();

        public void Add(QRCodeScan qRCodeScan)
        {
            _context.QRCodeScans.Add(qRCodeScan);
            _context.SaveChanges();
        }
    }
}
